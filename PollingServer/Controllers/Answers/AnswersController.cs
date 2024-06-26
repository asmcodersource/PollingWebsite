using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollingServer.Controllers.Answers.DTOs;
using PollingServer.Filters;
using PollingServer.Models;
using PollingServer.Models.Poll;
using PollingServer.Models.Poll.Answer;
using PollingServer.Models.Poll.Question;
using PollingServer.Services.PollAccessService;
using PollingServer.Services.UserFetchService;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace PollingServer.Controllers.Answers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnswersController: Controller
    {
        public DatabaseContext databaseContext { get; protected set; }
        public IPollAccessService pollAccessService { get; protected set; }
        public IUserFetchService userFetchService { get; protected set; }

        public AnswersController(DatabaseContext databaseContext, IPollAccessService pollAccessService, IUserFetchService userFetchService)
        {
            this.databaseContext = databaseContext;
            this.pollAccessService = pollAccessService;
            this.userFetchService = userFetchService;
        }

        [HttpPost("{pollId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IEnumerable<List<ValidationErrorResponse>>), 400)]
        [ReadableBodyStream, MaxBodyStreamLength(1 * 1024 * 1024)]
        public async Task<IActionResult> CreatePollAnswer(int pollId, [FromBody] List<BaseNewAnswersDTO> newAnswersDTOs )
        {
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            var poll = databaseContext.Polls
                    .Where(p => p.Id == pollId)
                    .Include(p => p.Questions!)
                    .Include(p => p.Answers!)
                    .FirstOrDefault();
            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);

            if (pollAccessService.IsUserHasAccessToPoll(poll, HttpContext) is not true)
                return StatusCode(StatusCodes.Status403Forbidden);


            // Ensure that client send correct count, and type of answers
            // At this moment lets assume type just by QuestionId property
            var foundedQuestions = new HashSet<int>();
            var questions = poll.Questions!.ToList();
            if( newAnswersDTOs.Count != questions.Count )
                return StatusCode(StatusCodes.Status400BadRequest);
            foreach (var answer in newAnswersDTOs)
                foundedQuestions.Add(answer.QuestionId);
            foreach ( var question in  questions )
                if( foundedQuestions.Contains(question.Id) is not true )
                    return StatusCode(StatusCodes.Status400BadRequest);

            // So fast verify tell us that answers seems correct? Then lets parse it from stream
            Request.Body.Seek(0, SeekOrigin.Begin);
            var document = await JsonDocument.ParseAsync(Request.Body);
            if( document.RootElement.ValueKind != JsonValueKind.Array )
                return StatusCode(StatusCodes.Status400BadRequest);

            int jsonCurrentElementIndex = 0;
            List<BaseAnswer> parsedAnswers = new List<BaseAnswer>();
            List<ValidationErrorResponse> validationErrorResponses = new List<ValidationErrorResponse>();
            foreach ( var jsonElement in document.RootElement.EnumerateArray())
            {
                var truncatedAnswer = newAnswersDTOs[jsonCurrentElementIndex]; // it serrialized only properties of base class
                var question = questions.Find((question) => question.Id == truncatedAnswer.QuestionId);
                var parsedAnswer = BaseAnswer.ParseJsonByExplicitType(jsonElement.GetRawText(), question!.AnswerType);
                parsedAnswer.FieldName = question.FieldName;
                parsedAnswer.Description = question.Description;
                List<ValidationResult> validationResults = new List<ValidationResult>();
                validationResults.AddRange(parsedAnswer.ValidateObjectByModel());
                validationResults.AddRange(parsedAnswer.ValidateByQuestion(question));
                if (validationResults.Count != 0) 
                {
                    var validationErrorResponse = new ValidationErrorResponse(validationResults, question.Id);
                    validationErrorResponses.Add(validationErrorResponse);
                }
                parsedAnswers.Add(parsedAnswer);
                jsonCurrentElementIndex = jsonCurrentElementIndex + 1;
            }

            if (validationErrorResponses.Count != 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, Json(validationErrorResponses));
            }
            else
            {
                var newAcceptedAnswer = new PollAnswers()
                {
                    AnswerTime = DateTime.UtcNow,
                    BaseAnswers = parsedAnswers,
                    UserId = user.Id,
                    User = user
                };
                poll.Answers!.Add(newAcceptedAnswer);
                databaseContext.Update(poll);
                databaseContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK);
            }
        }

        [HttpGet("{pollId}/{answerId}"), Authorize]
        [ProducesResponseType(typeof(IEnumerable<AnswerDTO>), 200)]
        public IActionResult GetPollAnswer(int pollId, int? answerId)
        {
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            var poll = databaseContext.Polls
                .Where(p => p.Id == pollId)
                .Include(p => p.Answers!)
                    .ThenInclude(a => a.BaseAnswers)
                .FirstOrDefault();
            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (poll.OwnerId != user!.Id)
                return StatusCode(StatusCodes.Status403Forbidden);
            var answer = poll.Answers?.Where((answer) => answer.Id == answerId).Select((answers) => new AnswerDTO(answers));
            if (answer is null || answer?.Count() == 0)
                return StatusCode(StatusCodes.Status404NotFound);
            return Json(answer);
        }

        [HttpGet("{pollId}"), Authorize]
        [ProducesResponseType(typeof(IEnumerable<AnswersItemDTO>), 200)]
        public IActionResult GetPollAnswers(int pollId)
        {
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            var poll = databaseContext.Polls
                .Where(p => p.Id == pollId)
                .Include(p => p.Answers!)
                .FirstOrDefault();
            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (poll.OwnerId != user!.Id)
                return StatusCode(StatusCodes.Status403Forbidden);
            return Json(poll.Answers?.Select((answer) => new AnswersItemDTO(answer)));
        }

        [HttpDelete("{pollId}/{answerId}"), Authorize]
        [ProducesResponseType(200)]
        public IActionResult DeletePollAnswer(int pollId, int? answerId)
        {
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            var poll = databaseContext.Polls
                .Where(p => p.Id == pollId)
                .Include(p => p.Answers!)
                .FirstOrDefault();
            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (poll.OwnerId != user!.Id)
                return StatusCode(StatusCodes.Status403Forbidden);
            var answers = poll.Answers?.Where((answer) => answer.Id == answerId);
            if (answers is null || answers?.Count() == 0)
                return StatusCode(StatusCodes.Status404NotFound);
            poll.Answers?.Remove(poll.Answers.Where((answer) => answer.Id == answerId).First());
            databaseContext.Polls.Update(poll);
            databaseContext.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpDelete("{pollId}"), Authorize]
        [ProducesResponseType(200)]
        public IActionResult DeletePollAnswers(int pollId)
        {
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            var poll = databaseContext.Polls
                .Where(p => p.Id == pollId)
                .Include(p => p.Answers!)
                .FirstOrDefault();
            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (poll.OwnerId != user!.Id)
                return StatusCode(StatusCodes.Status403Forbidden);
            poll.Answers?.Clear();
            databaseContext.Polls.Update(poll);
            databaseContext.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}

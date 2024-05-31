using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollingServer.Filters;
using PollingServer.Models.Poll.Question;
using PollingServer.Models;
using System.Security.Claims;
using System.Text;
using PollingServer.Controllers.Question.DTOs;
using PollingServer.Services.PollAccessService;
using PollingServer.Services.UserFetchService;

namespace PollingServer.Controllers.Questions
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : Controller
    {
        public DatabaseContext databaseContext { get; protected set; }
        public IPollAccessService pollAccessService { get; protected set; }
        public IUserFetchService userFetchService { get; protected set; }

        public QuestionsController(DatabaseContext databaseContext, IPollAccessService pollAccessService, IUserFetchService userFetchService)
        {
            this.databaseContext = databaseContext;
            this.pollAccessService = pollAccessService;
            this.userFetchService = userFetchService;
        }


        [HttpDelete]
        [Authorize]
        [Route("{pollId}/{questionId}")]
        public IActionResult DeletePollQuestion(int pollId, int questionId)
        {
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            var poll = databaseContext.Polls
                .Where(p => p.Id == pollId)
                .Include(p => p.Questions!)
                .FirstOrDefault();

            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (pollAccessService.IsUserHasAccessToPoll(poll, HttpContext) is not true)
                return StatusCode(StatusCodes.Status403Forbidden);

            var question = poll.Questions!.Where((q) => q.Id == questionId).FirstOrDefault();
            if (question is null)
                return StatusCode(StatusCodes.Status404NotFound);

            poll.Questions!.Remove(question);
            databaseContext.Polls.Update(poll);
            databaseContext.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpPut]
        [Authorize]
        [ReadableBodyStream]
        [Route("{pollId}")]
        public IActionResult UpdatePollQuestion(int pollId, [FromBody] BaseQuestion question)
        {
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            var poll = databaseContext.Polls
                .Where(p => p.Id == pollId)
                .Include(p => p.Questions!)
                .FirstOrDefault();

            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (pollAccessService.IsUserHasAccessToPoll(poll, HttpContext) is not true)
                return StatusCode(StatusCodes.Status403Forbidden);

            Request.Body.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var bodyString = reader.ReadToEndAsync().Result;
                var parsedQuestion = BaseQuestion.ParseJsonByDiscriminator(bodyString, question.Discriminator);
                var pollQuestion = poll.Questions!.FirstOrDefault(q => q.Id == parsedQuestion.Id);

                if (pollQuestion != null)
                {
                    databaseContext.Entry(pollQuestion).State = EntityState.Detached;
                    poll.Questions.Remove(pollQuestion);
                }

                poll.Questions.Add(parsedQuestion);
                databaseContext.Polls.Update(poll);
                databaseContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("{pollId}")]
        [ProducesResponseType(typeof(BaseQuestion), 200)]
        public IActionResult CreatePollQuestion(int pollId, [FromBody] CreateQuestionDTO createQuestionDTO)
        {
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            databaseContext.ChangeTracker.LazyLoadingEnabled = false;
            var poll = databaseContext.Polls
                .Where(p => p.Id == pollId)
                .Include(p => p.Questions!)
                .FirstOrDefault();

            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (pollAccessService.IsUserHasAccessToPoll(poll, HttpContext) is not true)
                return StatusCode(StatusCodes.Status403Forbidden);

            var createdQuestion = BaseQuestion.CreateByDiscriminator(createQuestionDTO.QuestionDiscriminator);
            createdQuestion.Description = createQuestionDTO.QuestionDescription;
            createdQuestion.FieldName = createQuestionDTO.QuestionName;
            poll.Questions!.Add(createdQuestion);
            databaseContext.Polls.Update(poll);
            databaseContext.SaveChanges();
            return Json(createdQuestion);
        }

        [HttpPut]
        [Route("{pollId}/order")]
        public IActionResult GetPollQuestionsOrder(int pollId, [FromBody] List<QuestionOrderDTO> questionOrders)
        {
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            databaseContext.ChangeTracker.LazyLoadingEnabled = false;
            var poll = databaseContext.Polls
                .Where(p => p.Id == pollId)
                .Include(p => p.Questions!)
                .FirstOrDefault();

            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (pollAccessService.IsUserHasAccessToPoll(poll, HttpContext) is not true)
                return StatusCode(StatusCodes.Status403Forbidden);

            foreach (var order in questionOrders)
            {
                var question = poll.Questions!.FirstOrDefault((q) => q.Id == order.QuestionId);
                if (question is not null)
                    question.OrderRate = order.OrderRate;
            }

            databaseContext.Polls.Update(poll);
            databaseContext.SaveChanges();
            return Json(poll.Questions);
        }


        [HttpGet]
        [Route("{pollId}")]
        [ProducesResponseType(typeof(ICollection<BaseQuestion>), 200)]
        public IActionResult GetPollQuestions(int pollId)
        {
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            databaseContext.ChangeTracker.LazyLoadingEnabled = false;
            var poll = databaseContext.Polls
                .Where(p => p.Id == pollId)
                .Include(p => p.Questions!)
                .FirstOrDefault();

            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (pollAccessService.IsUserHasAccessToPoll(poll, HttpContext) is not true)
                return StatusCode(StatusCodes.Status403Forbidden);

            return Json(poll.Questions);
        }
    }
}

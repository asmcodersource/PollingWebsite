﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollingServer.Controllers.Poll.DTOs;
using PollingServer.Models;
using PollingServer.Models.Poll;
using PollingServer.Models.Poll.Answer;
using PollingServer.Models.Poll.Question;
using System.Security.Claims;
using System.Text;
using PollingServer.Filters;
using System.Text.Json;

namespace PollingServer.Controllers.Polls
{
    [ApiController]
    [Route("api/[controller]")]
    public class PollController : Controller
    {
        protected DatabaseContext databaseContext { get; set; }

        public PollController(IConfiguration configuration, DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }


        //
        //          QUESTIONS
        //


        [HttpDelete]
        [Authorize]
        [Route("{pollId}/question/{questionId}")]
        public IActionResult DeletePollQuestion(int pollId, int questionId)
        {
            Models.User.User? user = null;
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if (userId is not null)
                user = databaseContext.Users.FirstOrDefault((user) => user.Id == Convert.ToInt32(userId));
            var poll = databaseContext.Polls
                .Where(p => p.Id == pollId)
                .Include(p => p.Questions!)
                .FirstOrDefault();

            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (IsUserHasAccessToPoll(poll, user) is not true)
                return StatusCode(StatusCodes.Status403Forbidden);

            var question = poll.Questions.Where((q) => q.Id == questionId).FirstOrDefault();
            if( question is null )
                return StatusCode(StatusCodes.Status404NotFound);

            poll.Questions.Remove(question);
            databaseContext.Polls.Update(poll);
            databaseContext.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpPut]
        [Authorize]
        [ReadableBodyStream]
        [Route("{pollId}/question")]
        public IActionResult UpdatePollQuestion(int pollId, [FromBody] BaseQuestion question)
        {
            Models.User.User? user = null;
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if (userId is not null)
                user = databaseContext.Users.FirstOrDefault((user) => user.Id == Convert.ToInt32(userId));
            var poll = databaseContext.Polls
                .Where(p => p.Id == pollId)
                .Include(p => p.Questions!)
                .FirstOrDefault();

            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (IsUserHasAccessToPoll(poll, user) is not true)
                return StatusCode(StatusCodes.Status403Forbidden);

            Request.Body.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var bodyString = reader.ReadToEndAsync().Result;
                var parsedQuestion = BaseQuestion.ParseJsonByDiscriminator(bodyString, question.Discriminator);
                var pollQuestion = poll.Questions.FirstOrDefault(q => q.Id == parsedQuestion.Id);

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
        [Route("{pollId}/question")]
        [ProducesResponseType(typeof(BaseQuestion), 200)]
        public IActionResult CreatePollQuestion(int pollId, [FromBody] CreateQuestionDTO createQuestionDTO)
        {
            Models.User.User? user = null;
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if (userId is not null)
                user = databaseContext.Users.FirstOrDefault((user) => user.Id == Convert.ToInt32(userId));
            databaseContext.ChangeTracker.LazyLoadingEnabled = false;
            var poll = databaseContext.Polls
                .Where(p => p.Id == pollId)
                .Include(p => p.Questions!)
                .FirstOrDefault();

            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (IsUserHasAccessToPoll(poll, user) is not true)
                return StatusCode(StatusCodes.Status403Forbidden);

            var createdQuestion = BaseQuestion.CreateByDiscriminator(createQuestionDTO.QuestionDiscriminator);
            createdQuestion.Description = createQuestionDTO.QuestionDescription;
            createdQuestion.FieldName = createQuestionDTO.QuestionName;
            poll.Questions.Add(createdQuestion);
            databaseContext.Polls.Update(poll);
            databaseContext.SaveChanges();
            return Json(createdQuestion);
        }

        [HttpPost]
        [Route("{pollId}/questions/ordersupdate")]
        public IActionResult GetPollQuestionsOrder(int pollId, [FromBody] List<QuestionOrderDTO> questionOrders)
        {
            Models.User.User? user = null;
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if (userId is not null)
                user = databaseContext.Users.FirstOrDefault((user) => user.Id == Convert.ToInt32(userId));
            databaseContext.ChangeTracker.LazyLoadingEnabled = false;
            var poll = databaseContext.Polls
                .Where(p => p.Id == pollId)
                .Include(p => p.Questions!)
                .FirstOrDefault();

            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (IsUserHasAccessToPoll(poll, user) is not true)
                return StatusCode(StatusCodes.Status403Forbidden);

            foreach( var order in questionOrders)
            {
                var question = poll.Questions.FirstOrDefault((q) => q.Id == order.QuestionId);
                if( question is not null )
                    question.OrderRate = order.OrderRate;
            }

            databaseContext.Polls.Update(poll);
            databaseContext.SaveChanges();
            return Json(poll.Questions);
        }


        [HttpGet]
        [Route("{pollId}/questions")]
        [ProducesResponseType(typeof(ICollection<BaseQuestion>), 200)]
        public IActionResult GetPollQuestions(int pollId)
        {
            Models.User.User? user = null;
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if (userId is not null)
                user = databaseContext.Users.FirstOrDefault((user) => user.Id == Convert.ToInt32(userId));
            databaseContext.ChangeTracker.LazyLoadingEnabled = false;
            var poll = databaseContext.Polls
                .Where(p => p.Id == pollId)
                .Include(p => p.Questions!)
                .FirstOrDefault();


            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (IsUserHasAccessToPoll(poll, user) is not true)
                return StatusCode(StatusCodes.Status403Forbidden);

            return Json(poll.Questions);
        }


        //
        //          ANSWERS
        //


        [Authorize]
        [HttpGet]
        [Route("{pollId}/answers/{answerId}")]
        [ProducesResponseType(typeof(IEnumerable<PollAnswersDTO>), 200)]
        public IActionResult GetPollAnswer(int pollId, int? answerId)
        {
            Models.User.User? user = null;
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if (userId is not null)
                user = databaseContext.Users.FirstOrDefault((user) => user.Id == Convert.ToInt32(userId));
            Models.Poll.Poll? poll = databaseContext.Polls.Find(pollId);
            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (poll.OwnerId != Convert.ToInt32(userId) )
            return StatusCode(StatusCodes.Status403Forbidden);
            var answer = poll.Answers?.Where((answer) => answer.Id == answerId).Select((answers) => new PollAnswersDTO(answers));
            if (answer is null || answer?.Count() == 0)
                return StatusCode(StatusCodes.Status404NotFound);
            return Json(answer);
        }


        [Authorize]
        [HttpGet]
        [Route("{pollId}/answers")]
        [ProducesResponseType(typeof(IEnumerable<PollAnswersDTO>), 200)]
        public IActionResult GetPollAnswers(int pollId)
        {
            Models.User.User? user = null;
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if (userId is not null)
                user = databaseContext.Users.FirstOrDefault((user) => user.Id == Convert.ToInt32(userId));
            Models.Poll.Poll? poll = databaseContext.Polls.Find(pollId);
            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (poll.OwnerId != Convert.ToInt32(userId))
                return StatusCode(StatusCodes.Status403Forbidden);
            return Json(poll.Answers?.Select((answer) => new PollAnswersDTO(answer)));
        }

        [Authorize]
        [HttpDelete]
        [Route("{pollId}/answers/{answerId}")]
        [ProducesResponseType(200)]
        public IActionResult DeletePollAnswer(int pollId, int? answerId)
        {
            Models.User.User? user = null;
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if (userId is not null)
                user = databaseContext.Users.FirstOrDefault((user) => user.Id == Convert.ToInt32(userId));
            Models.Poll.Poll? poll = databaseContext.Polls.Find(pollId);
            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (poll.OwnerId != Convert.ToInt32(userId))
                return StatusCode(StatusCodes.Status403Forbidden);
            var answers = poll.Answers?.Where((answer) => answer.Id == answerId);
            if (answers is null || answers?.Count() == 0)
                return StatusCode(StatusCodes.Status404NotFound);
            poll.Answers?.Remove(poll.Answers.Where((answer) => answer.Id == answerId).First());
            databaseContext.Polls.Update(poll);
            databaseContext.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize]
        [HttpDelete]
        [Route("{pollId}/answers")]
        [ProducesResponseType(200)]
        public IActionResult DeletePollAnswers(int pollId)
        {
            Models.User.User? user = null;
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if (userId is not null)
                user = databaseContext.Users.FirstOrDefault((user) => user.Id == Convert.ToInt32(userId));
            Models.Poll.Poll? poll = databaseContext.Polls.Find(pollId);
            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (poll.OwnerId != Convert.ToInt32(userId))
                return StatusCode(StatusCodes.Status403Forbidden);
            poll.Answers?.Clear();
            databaseContext.Polls.Update(poll);
            databaseContext.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }




        //
        //          INDEX
        //



        [HttpGet]
        [Route("{pollId}")]
        [ProducesResponseType(typeof(PollDTO), 200)]
        public IActionResult GetPollDescription( int pollId )
        {
            Models.User.User? user = null;
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if( userId is not null )
                user = databaseContext.Users.FirstOrDefault((user) => user.Id == Convert.ToInt32(userId));
            Models.Poll.Poll? poll = databaseContext.Polls.Find(pollId);
            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if( IsUserHasAccessToPoll(poll, user) is not true )
                return StatusCode(StatusCodes.Status403Forbidden);
            var pollDTO = new PollDTO(poll);
            return Json(pollDTO);
        }


        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ICollection<PollDTO>), 200)]
        public IActionResult GetPolls()
        {
            Models.User.User? user = null;
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if (userId is not null)
            {
                user = databaseContext.Users.FirstOrDefault((user) => user.Id == Convert.ToInt32(userId));
                var polls = databaseContext.Polls.Where((poll) => poll.OwnerId == user.Id).Select((poll) => new PollDTO(poll));
                return Json(polls.ToList());
            }
            return base.StatusCode(StatusCodes.Status400BadRequest);
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(typeof(PollDTO), 200)]
        public IActionResult Create([FromBody] PollCreateModel pollCreateModel)
        {
            // Verify model field attributes
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var id = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            var user = databaseContext.Users.FirstOrDefault((user) => user.Id == Convert.ToInt32(id));
            var poll = new Models.Poll.Poll
            {
                Title = pollCreateModel.Name,
                Description = pollCreateModel.Description,
                Owner = user,
                OwnerId = Convert.ToInt32(id),
                Type = pollCreateModel.Type,
                Access = pollCreateModel.Access
            };
            databaseContext.Polls.Add(poll);
            databaseContext.SaveChanges();
            var pollDTO = new PollDTO(poll);
            return Json(pollDTO);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            var user = databaseContext.Users.FirstOrDefault((user) => user.Id == Convert.ToInt32(userId));
            Models.Poll.Poll? poll = databaseContext.Polls.Find(id);
            if (poll is null )
                return StatusCode(StatusCodes.Status404NotFound);
            if (poll.OwnerId != Convert.ToInt32(userId))
                return StatusCode(StatusCodes.Status403Forbidden);
            databaseContext.Polls.Remove(poll);
            databaseContext.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }


        //
        //          IMAGES
        //


        [HttpGet]
        [Route("{pollId}/image")]
        public IActionResult GetImage(int pollId)
        {
            Models.User.User? user = null;
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if (userId is not null)
                user = databaseContext.Users.FirstOrDefault((user) => user.Id == Convert.ToInt32(userId));
            Models.Poll.Poll? poll = databaseContext.Polls.Find(pollId);
            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (IsUserHasAccessToPoll(poll, user) is not true)
                return StatusCode(StatusCodes.Status403Forbidden);
            if( poll.Image is not null  )
                return base.File(poll.Image.Bytes, poll.Image.ContentType);
            else 
                return base.NoContent();
        }

        [HttpPost]
        [Route("image")]
        public IActionResult GetImage()
        {
            return base.Problem("not implemented yet");
        }

        [NonAction]
        protected bool IsUserHasAccessToPoll(Models.Poll.Poll poll, Models.User.User? user)
        {
            switch (poll.Type)
            {
                case Models.Poll.PollingType.Anyone:
                    return true;
                case Models.Poll.PollingType.Auhorized:
                    return user is not null;
                case Models.Poll.PollingType.OnlyOwner:
                    return poll.OwnerId == user?.Id;
                case Models.Poll.PollingType.OnlyAllowed:
                    return poll.AllowedUsers?.Where((allowFor) => allowFor.UserId == user.Id).Count() > 0;
                default:
                    throw new ApplicationException("Something wen't wrong?");
            }
        }
    }
}

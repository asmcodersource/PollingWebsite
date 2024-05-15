using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollingServer.Controllers.Poll;
using PollingServer.Models;
using PollingServer.Models.Poll;
using PollingServer.Models.Poll.Answer;
using PollingServer.Models.Poll.Question;
using System.Security.Claims;

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

        [HttpGet]
        [Route("{pollId}/questions")]
        [ProducesResponseType(typeof(ICollection<PollQuestion>), 200)]
        public IActionResult GetPollQuestions(int pollId)
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
            return Json(poll.Questions);
        }

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

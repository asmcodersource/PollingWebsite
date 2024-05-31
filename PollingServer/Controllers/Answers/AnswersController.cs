using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PollingServer.Controllers.Answers.DTOs;
using PollingServer.Models;
using PollingServer.Services.PollAccessService;
using PollingServer.Services.UserFetchService;
using System.Security.Claims;

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

        [Authorize]
        [HttpGet]
        [Route("{pollId}/{answerId}")]
        [ProducesResponseType(typeof(IEnumerable<PollAnswersDTO>), 200)]
        public IActionResult GetPollAnswer(int pollId, int? answerId)
        {
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            Models.Poll.Poll? poll = databaseContext.Polls.Find(pollId);
            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (poll.OwnerId != user!.Id)
                return StatusCode(StatusCodes.Status403Forbidden);
            var answer = poll.Answers?.Where((answer) => answer.Id == answerId).Select((answers) => new PollAnswersDTO(answers));
            if (answer is null || answer?.Count() == 0)
                return StatusCode(StatusCodes.Status404NotFound);
            return Json(answer);
        }

        [Authorize]
        [HttpGet]
        [Route("{pollId}")]
        [ProducesResponseType(typeof(IEnumerable<PollAnswersDTO>), 200)]
        public IActionResult GetPollAnswers(int pollId)
        {
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            Models.Poll.Poll? poll = databaseContext.Polls.Find(pollId);
            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (poll.OwnerId != user.Id)
                return StatusCode(StatusCodes.Status403Forbidden);
            return Json(poll.Answers?.Select((answer) => new PollAnswersDTO(answer)));
        }

        [Authorize]
        [HttpDelete]
        [Route("{pollId}/{answerId}")]
        [ProducesResponseType(200)]
        public IActionResult DeletePollAnswer(int pollId, int? answerId)
        {
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            Models.Poll.Poll? poll = databaseContext.Polls.Find(pollId);
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

        [Authorize]
        [HttpDelete]
        [Route("{pollId}")]
        [ProducesResponseType(200)]
        public IActionResult DeletePollAnswers(int pollId)
        {
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            Models.Poll.Poll? poll = databaseContext.Polls.Find(pollId);
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

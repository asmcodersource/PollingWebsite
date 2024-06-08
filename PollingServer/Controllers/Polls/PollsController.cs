using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollingServer.Controllers.Poll.DTOs;
using PollingServer.Controllers.Polls.DTOs;
using PollingServer.Models;
using PollingServer.Models.Poll;
using PollingServer.Services.PollAccessService;
using PollingServer.Services.UserFetchService;

namespace PollingServer.Controllers.Polls
{
    [ApiController]
    [Route("api/[controller]")]
    public class PollsController : Controller
    {
        public DatabaseContext databaseContext { get; protected set; }
        public IPollAccessService pollAccessService { get; protected set; }
        public IUserFetchService userFetchService { get; protected set; }

        public PollsController(DatabaseContext databaseContext, IPollAccessService pollAccessService, IUserFetchService userFetchService)
        {
            this.databaseContext = databaseContext;
            this.pollAccessService = pollAccessService;
            this.userFetchService = userFetchService;
        }

        [HttpGet("{pollId}")]
        [ProducesResponseType(typeof(PollDTO), 200)]
        public IActionResult GetPollDescription(int pollId)
        {
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            Models.Poll.Poll? poll = databaseContext.Polls.Find(pollId);
            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if (pollAccessService.IsUserHasAccessToPoll(poll, HttpContext) is not true)
                return StatusCode(StatusCodes.Status403Forbidden);
            var pollDTO = new PollDTO(poll);
            return Json(pollDTO);
        }

        [HttpGet, Authorize]
        [ProducesResponseType(typeof(ICollection<PollDTO>), 200)]
        public IActionResult GetPolls()
        {
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            var polls = databaseContext.Polls.Where((poll) => poll.OwnerId == user!.Id).Select((poll) => new PollDTO(poll));
            return Json(polls.ToList());
        }

        [HttpPost, Authorize]
        [ProducesResponseType(typeof(PollDTO), 200)]
        public IActionResult Create([FromBody] CreatePollDTO pollCreateModel)
        {
            // Verify model field attributes
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            var poll = new Models.Poll.Poll
            {
                Title = pollCreateModel.Name,
                Description = pollCreateModel.Description,
                Owner = user,
                OwnerId = user!.Id,
                Type = pollCreateModel.Type,
                Access = pollCreateModel.Access
            };
            databaseContext.Polls.Add(poll);
            databaseContext.SaveChanges();
            var pollDTO = new PollDTO(poll);
            return Json(pollDTO);
        }

        [HttpDelete("{id}"), Authorize]
        public IActionResult Delete(int id)
        {
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            Models.Poll.Poll? poll = databaseContext.Polls.Find(id);
            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            if (poll.OwnerId != user!.Id)
                return StatusCode(StatusCodes.Status403Forbidden);
            databaseContext.Polls.Remove(poll);
            databaseContext.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpPut("{id}"), Authorize]
        public IActionResult Update(int id, [FromBody] UpdatePollDTO updatePollDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            Models.Poll.Poll? poll = databaseContext.Polls.Find(id);
            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            if (poll.OwnerId != user!.Id)
                return StatusCode(StatusCodes.Status403Forbidden);
            poll.Title = updatePollDTO.Title;
            poll.Description = updatePollDTO.Description;
            poll.Type = updatePollDTO.Type;
            databaseContext.Update(poll);
            databaseContext.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }


        [HttpGet("{pollId}/allowed-users"), Authorize]
        [ProducesResponseType(typeof(ICollection<AllowedUserDTO>), 200)]
        public IActionResult GetAllowedUsers(int pollId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            Models.Poll.Poll? poll = databaseContext.Polls
                .Where(p => p.Id == pollId)
                .Include(p => p.AllowedUsers)
                .First();

            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            if (poll.OwnerId != user!.Id)
                return StatusCode(StatusCodes.Status403Forbidden);
            return Json(poll.AllowedUsers!.Select(au => new AllowedUserDTO(au.User.Id, au.User.Nickname)));
        }

        [HttpDelete("{pollId}/allowed-users"), Authorize]
        public IActionResult DeleteAllowedUser(int pollId, [FromBody] AllowedUserDTO allowedUserDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            Models.Poll.Poll? poll = databaseContext.Polls
                .Where(p => p.Id == pollId)
                .Include(p => p.AllowedUsers)
                .First();

            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            if (poll.OwnerId != user!.Id)
                return StatusCode(StatusCodes.Status403Forbidden);
            poll.AllowedUsers!.Remove(poll.AllowedUsers.Where(au => au.UserId == allowedUserDTO.Id).First());
            databaseContext.Polls.Update(poll);
            databaseContext.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpPost("{pollId}/allowed-users"), Authorize]
        public IActionResult AddAllowedUser(int pollId, [FromBody] AddAllowedUserDTO addAllowedUserDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            Models.Poll.Poll? poll = databaseContext.Polls
                .Where(p => p.Id == pollId)
                .Include(p => p.AllowedUsers)
                .First();

            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            if (poll.OwnerId != user!.Id)
                return StatusCode(StatusCodes.Status403Forbidden);

            var addUser = databaseContext.Users.Where(u => u.Nickname == addAllowedUserDTO.Nickname).FirstOrDefault();
            if( addUser is null)
                return StatusCode(StatusCodes.Status200OK);

            var pollAllowedUser = new PollAllowedUsers(){ UserId = addUser.Id };
            poll.AllowedUsers!.Add(pollAllowedUser);
            databaseContext.Polls.Update(poll);
            databaseContext.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }

    }
}

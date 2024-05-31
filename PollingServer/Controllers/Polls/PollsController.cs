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
using PollingServer.Services.PollAccessService;
using PollingServer.Controllers.Question;
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

        public PollsController( DatabaseContext databaseContext, IPollAccessService pollAccessService, IUserFetchService userFetchService)
        {
            this.databaseContext = databaseContext;
            this.pollAccessService = pollAccessService;
            this.userFetchService = userFetchService;
        }


        [HttpGet]
        [Route("{pollId}")]
        [ProducesResponseType(typeof(PollDTO), 200)]
        public IActionResult GetPollDescription( int pollId )
        {
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            Models.Poll.Poll? poll = databaseContext.Polls.Find(pollId);
            if (poll is null)
                return StatusCode(StatusCodes.Status404NotFound);
            // Ensure that user has access to this poll
            if( pollAccessService.IsUserHasAccessToPoll(poll, HttpContext) is not true )
                return StatusCode(StatusCodes.Status403Forbidden);
            var pollDTO = new PollDTO(poll);
            return Json(pollDTO);
        }


        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ICollection<PollDTO>), 200)]
        public IActionResult GetPolls()
        {
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            var polls = databaseContext.Polls.Where((poll) => poll.OwnerId == user!.Id).Select((poll) => new PollDTO(poll));
            return Json(polls.ToList());
        }

        [HttpPost]
        [Authorize]
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

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            Models.User.User? user = userFetchService.GetUserFromContext(HttpContext);
            Models.Poll.Poll? poll = databaseContext.Polls.Find(id);
            if (poll is null )
                return StatusCode(StatusCodes.Status404NotFound);
            if (poll.OwnerId != user!.Id)
                return StatusCode(StatusCodes.Status403Forbidden);
            databaseContext.Polls.Remove(poll);
            databaseContext.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using PollingServer.Models;
using PollingServer.Services.UserFetchService;
using System.Security.Claims;

namespace PollingServer.Services.PollAccessService
{
    public class PollAccessService: IPollAccessService
    {
        public DatabaseContext databaseContext { get; protected set; }
        public IUserFetchService userFetchService { get; protected set; }

        public PollAccessService(DatabaseContext databaseContext, IUserFetchService userFetchService) 
        {
            this.databaseContext = databaseContext;
            this.userFetchService = userFetchService;
        }

        public bool IsUserHasAccessToPoll( Models.Poll.Poll poll, HttpContext httpContext)
        {
            Models.User.User? user = userFetchService.GetUserFromContext(httpContext);

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

using Microsoft.AspNetCore.Http;
using PollingServer.Models;
using PollingServer.Models.User;
using System.Security.Claims;

namespace PollingServer.Services.UserFetchService
{
    public class UserFetchService: IUserFetchService 
    {
        public DatabaseContext databaseContext { get; protected set; }

        public UserFetchService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public User? GetUserFromContext(HttpContext httpContext)
        {
            Models.User.User? user = null;
            var userId = httpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if (userId is not null)
                return databaseContext.Users.FirstOrDefault((user) => user.Id == Convert.ToInt32(userId));
            else
                return null;
        }
    }
}

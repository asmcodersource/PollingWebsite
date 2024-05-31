using PollingServer.Models.User;

namespace PollingServer.Services.UserFetchService
{
    public interface IUserFetchService
    {
        public User? GetUserFromContext(HttpContext httpContext);
    }
}

namespace PollingServer.Services.PollAccessService
{
    public interface IPollAccessService
    {

        public bool IsUserHasAccessToPoll(Models.Poll.Poll poll, HttpContext httpContext);
    }
}

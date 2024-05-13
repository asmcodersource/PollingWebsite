namespace PollingServer.Controllers
{
    public static class BadResponseFactory
    {
        public static object CreateErrorResponse(string type, string title, string[] errors, int code = 400)
        {
            return new
            {
                type = type,
                title = title,
                errors = new { error = errors },
                code = code
            };
        }
    }
}

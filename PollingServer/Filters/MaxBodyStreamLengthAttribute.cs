using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PollingServer.Filters
{
    public class MaxBodyStreamLengthAttribute : Attribute, IEndpointFilter
    {
        private readonly int maxBodyLength;

        public MaxBodyStreamLengthAttribute(int maxBodyLength) 
        {
            this.maxBodyLength = maxBodyLength;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            if (context.HttpContext.Request.ContentLength.HasValue && context.HttpContext.Request.ContentLength > maxBodyLength)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status413PayloadTooLarge;
                await context.HttpContext.Response.WriteAsync("Request body is too large.");
                return null;
            }
            else
            {
                return await next(context);
            }
        }
    }
}

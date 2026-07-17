using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APIS_WEB_DB.Security
{
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string APIKEY = "biblioteca123";

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("X-API-Key", out var apiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (apiKey != APIKEY)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}

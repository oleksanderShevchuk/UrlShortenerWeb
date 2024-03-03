using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortenerWeb.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                // Extract validation errors from ModelState
                var errors = context.ModelState
                    .Where(e => e.Value.Errors.Any())
                    .Select(e => new {
                        Property = e.Key,
                        Errors = e.Value.Errors.Select(error => error.ErrorMessage)
                    });

                // Return the validation errors as part of the response
                context.Result = new BadRequestObjectResult(errors);
                return;
            }
            await next();
        }
    }
}

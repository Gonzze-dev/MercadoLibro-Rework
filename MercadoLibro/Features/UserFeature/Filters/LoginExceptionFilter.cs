using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MercadoLibro.Features.UserFeature.Filters
{

    [AttributeUsage(AttributeTargets.Method)]
    public class LoginExceptionFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            context.Result = exception switch
            {
                ArgumentNullException or BadHttpRequestException =>
                    new BadRequestObjectResult(exception.Message),

                UnauthorizedAccessException =>
                    new ObjectResult(exception.Message) { StatusCode = 401 },

                KeyNotFoundException =>
                    new NotFoundObjectResult(exception.Message),

                _ => new ObjectResult("An unexpected error occurred.") { StatusCode = 500 }
            };

            context.ExceptionHandled = true;
        }
    }
}

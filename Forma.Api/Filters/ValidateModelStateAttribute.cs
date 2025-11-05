using Forma.Api.Extensions;
using Forma.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Forma.Api.Filters
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.ToApiErrors();
                var errorResponse = new ApiErrorResponse { Errors = errors };

                context.Result = new BadRequestObjectResult(errorResponse);
            }
        }
    }
}
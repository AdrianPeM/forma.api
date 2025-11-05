using Forma.Api.Constants;
using Forma.Api.Responses;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected IActionResult ErrorResponse(string field, string code, Func<object, IActionResult>? responseMethod = null)
    {
        List<ApiError> errors = [
            new() {
                Field = field.ToLower(),
                Code = ErrorCodes.ForField(field, code)
            }
        ];

        var errorResponse = new ApiErrorResponse
        {
            Errors = errors
        };

        return responseMethod != null ? responseMethod(errorResponse) : BadRequest(errorResponse);
    }

    protected IActionResult ErrorResponse(List<ApiError> errors, Func<object, IActionResult>? responseMethod = null)
    {
        var errorResponse = new ApiErrorResponse { Errors = errors };
        return responseMethod != null ? responseMethod(errorResponse) : BadRequest(errorResponse);
    }
}

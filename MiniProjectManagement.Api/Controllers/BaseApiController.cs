using Microsoft.AspNetCore.Mvc;
using MiniProjectManagement.Api.Helpers;

namespace MiniProjectManagement.Api.Controllers;

public abstract class BaseApiController : ControllerBase
{
    protected ActionResult HandleServiceError<T>(ServiceResult<T> result)
    {
        return result.ErrorType switch
        {
            ServiceErrorType.NotFound => NotFound(result.ErrorMessage),
            ServiceErrorType.BadRequest => BadRequest(result.ErrorMessage),
            ServiceErrorType.Validation => BadRequest(result.ErrorMessage),
            ServiceErrorType.Conflict => Conflict(result.ErrorMessage),
            _ => StatusCode(500, "Unexpected error occurred.")
        };
    }
}
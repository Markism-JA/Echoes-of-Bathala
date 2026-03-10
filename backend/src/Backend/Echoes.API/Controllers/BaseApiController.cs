using Echoes.Shared.Network.Common;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Echoes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        var response = new ErrorResponse();

        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            foreach (var error in errors)
            {
                response.Errors.Add(error.Code, error.Description);
            }
            response.Code = "ValidationFailure";
            response.Description = "One or more validation errors occurred.";

            return BadRequest(response);
        }

        var firstError = errors[0];
        response.Code = firstError.Code;
        response.Description = firstError.Description;

        var statusCode = firstError.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError,
        };

        return StatusCode(statusCode, response);
    }
}

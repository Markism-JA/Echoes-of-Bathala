using GameBackend.Core.Interfaces.Security;
using GameBackend.Shared.DTOs.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameBackend.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto request, CancellationToken ct)
    {
        var result = await authService.RegisterAsync(request, ct);

        return result.Match(authResponse => Ok(authResponse), errors => MapErrorsToProblem(errors));
    }

    private IActionResult MapErrorsToProblem(List<ErrorOr.Error> errors)
    {
        if (errors.Count == 0)
            return Problem();

        if (errors.All(error => error.Type == ErrorOr.ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        var firstError = errors[0];

        var statusCode = firstError.Type switch
        {
            ErrorOr.ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorOr.ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorOr.ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Problem(
            statusCode: statusCode,
            title: firstError.Code,
            detail: firstError.Description
        );
    }

    private ActionResult ValidationProblem(List<ErrorOr.Error> errors)
    {
        var modelStateDictionary = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();

        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(error.Code, error.Description);
        }

        return ValidationProblem(modelStateDictionary);
    }
}

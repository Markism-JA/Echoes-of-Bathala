using GameBackend.Core.Interfaces.Security;
using GameBackend.Shared.DTOs.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameBackend.API.Controllers;

[Route("api/auth")]
public class AuthController(IAuthService authService) : BaseApiController
{
    /// <summary>
    /// Registers a new player in the Echoes of Bathala universe.
    /// </summary>
    /// <response code="200">Returns the new user details and initial auth tokens.</response>
    /// <response code="400">If the request is malformed or validation fails.</response>
    /// <response code="409">If the username or email is already taken.</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequestDto request,
        CancellationToken ct
    )
    {
        var result = await authService.RegisterAsync(request, ct);

        // Uses the Problem() method inherited from BaseApiController
        return result.Match(authResponse => Ok(authResponse), errors => Problem(errors));
    }

    /// <summary>
    /// Authenticates a player and initiates a new game session.
    /// </summary>
    /// <response code="200">Returns the access and refresh tokens.</response>
    /// <response code="401">If the email or password is incorrect.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken ct)
    {
        var result = await authService.LoginAsync(request, ct);

        return result.Match(authResponse => Ok(authResponse), errors => Problem(errors));
    }
}

using Echoes.Application.Auth.RegisterEmail;
using Echoes.Shared.Network.DTOs.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Echoes.API.Controllers
{
    [Route("api/auth")]
    public class AuthController(ISender sender) : BaseApiController
    {
        [HttpPost("register/email")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var command = new RegisterEmailCommand(
                request.Username,
                request.Email,
                request.Password
            );
            var result = await sender.Send(command);

            return result.Match(Ok, errors => Problem(errors));
        }
    }
}

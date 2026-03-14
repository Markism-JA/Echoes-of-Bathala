using Echoes.Shared.Network.Auth;
using ErrorOr;
using MediatR;

namespace Echoes.Application.Auth.RegisterEmail
{
    /// <summary>
    /// Represents a request to register a new user account using an email and password.
    /// </summary>
    /// <param name="Username">The unique username requested by the player.</param>
    /// <param name="Email">The email address used for identity and communication.</param>
    /// <param name="Password">The raw password string provided by the player.</param>
    /// <remarks>
    /// This command is handled by the corresponding handler which enforces
    /// registration policies and interacts with the identity service.
    /// </remarks>
    public record RegisterEmailCommand(string Username, string Email, string Password)
        : IRequest<ErrorOr<AuthResponseDto>>;
}

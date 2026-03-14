using Echoes.Shared.Network.Auth;
using ErrorOr;
using MediatR;

namespace Echoes.Application.Auth.RegisterEmail
{
    public record RegisterEmailCommand(string Username, string Email, string Password)
        : IRequest<ErrorOr<AuthResponseDto>>;
}

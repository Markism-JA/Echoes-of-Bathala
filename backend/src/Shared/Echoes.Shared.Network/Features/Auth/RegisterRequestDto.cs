namespace Echoes.Shared.Network.Features.Auth;

public record RegisterRequestDto(
    string Username,
    string Email,
    string Password
);

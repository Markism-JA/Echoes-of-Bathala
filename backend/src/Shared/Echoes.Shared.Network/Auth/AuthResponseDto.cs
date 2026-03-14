using System;

namespace Echoes.Shared.Network.Auth
{
    /// <summary>
    /// An immutable record representing the result of a successful authentication.
    /// </summary>
    public record AuthResponseDto(
        string AccessToken,
        string RefreshToken,
        DateTime AccessTokenExpiration,
        DateTime RefreshTokenExpiration,
        UserResponseDto User
    );
}

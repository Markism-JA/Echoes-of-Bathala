using System;

namespace GameBackend.Shared.DTOs.Identity
{
    public record AuthResponseDto(
        string AccessToken,
        string RefreshToken,
        DateTime Expiration,
        UserResponseDto User
    );
}

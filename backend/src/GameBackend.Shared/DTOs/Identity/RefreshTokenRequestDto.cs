using System.ComponentModel.DataAnnotations;

namespace GameBackend.Shared.DTOs.Identity
{
    public record RefreshTokenRequestDto(
        [Required] string AccessToken,
        [Required] string RefreshToken
    );
}

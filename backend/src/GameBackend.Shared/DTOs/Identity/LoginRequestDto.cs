using System.ComponentModel.DataAnnotations;

namespace GameBackend.Shared.DTOs.Identity
{
    public record LoginRequestDto(
        [Required] [EmailAddress] string Email,
        [Required] string Password
    );
}

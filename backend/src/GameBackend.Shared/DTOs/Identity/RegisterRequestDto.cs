using System.ComponentModel.DataAnnotations;

namespace GameBackend.Shared.DTOs.Identity
{
    public record RegisterRequestDto(
        [Required]
        [StringLength(20, MinimumLength = 3)]
        [RegularExpression(
            @"^[a-zA-Z0-9_]+$",
            ErrorMessage = "Only letters, numbers, and underscores are allowed."
        )]
            string Username,
        [Required] [EmailAddress] string Email,
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
            string Password,
        [Required]
        [property: Compare("Password", ErrorMessage = "Password do not match.")]
            string ConfirmPassword
    );
}

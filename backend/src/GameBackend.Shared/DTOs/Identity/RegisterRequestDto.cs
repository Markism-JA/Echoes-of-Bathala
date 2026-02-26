using System.ComponentModel.DataAnnotations;

namespace GameBackend.Shared.DTOs.Identity;

public record RegisterRequestDto
{
    [Required]
    [StringLength(20, MinimumLength = 3)]
    [RegularExpression(
        @"^[a-zA-Z0-9_]+$",
        ErrorMessage = "Only letters, numbers, and underscores are allowed."
    )]
    public string Username { get; init; } = default!;

    [Required]
    [EmailAddress]
    public string Email { get; init; } = default!;

    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    public string Password { get; init; } = default!;

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; init; } = default!;
}

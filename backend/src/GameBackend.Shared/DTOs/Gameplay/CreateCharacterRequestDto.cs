using System.ComponentModel.DataAnnotations;
using GameBackend.Shared.Enums;

namespace GameBackend.Shared.DTOs.Gameplay;

public record CreateCharacterRequestDto(
    [Required]
    [StringLength(16, MinimumLength = 3)]
    [RegularExpression(
        @"^[a-zA-Z0-9]*$",
        ErrorMessage = "Name must be alphanumeric with no spaces."
    )]
        string Name,
    [Required] CharacterClass Class
);

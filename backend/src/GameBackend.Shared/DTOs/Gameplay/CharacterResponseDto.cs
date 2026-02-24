using System;
using GameBackend.Shared.Enums;

namespace GameBackend.Shared.DTOs.Gameplay;

public record CharacterResponseDto(
    Guid Id,
    Guid UserId,
    string Name,
    CharacterClass Class,
    int Level,
    long Experience,
    int Gold,
    CharacterStatsDto Stats,
    DateTime CreatedAt
);

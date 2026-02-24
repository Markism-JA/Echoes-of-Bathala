namespace GameBackend.Shared.DTOs.Gameplay;

public record CharacterStatsDto(
    int MaxHealth,
    int MaxMana,
    int Strength,
    int Dexterity,
    int Intelligence
);

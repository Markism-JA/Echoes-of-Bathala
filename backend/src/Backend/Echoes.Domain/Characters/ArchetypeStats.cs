namespace Echoes.Domain.Characters;

/// <summary>
/// Configuration DTO representing the growth parameters and stat modifiers
/// associated with a specific character class.
/// </summary>
/// <remarks>
/// This is used by the <c>ArchetypeRegistry</c> to deserialize static data
/// from external stores (JSON, DB) into the server's in-memory lookup table.
/// </remarks>
public record ArchetypeStats(
    AnitoClass ClassType,
    float HealthGrowthPerLevel,
    float ManaGrowthPerLevel,
    float BaseDamageMultiplier,
    float MovementSpeedModifier,
    float AbilityCooldownReduction
);

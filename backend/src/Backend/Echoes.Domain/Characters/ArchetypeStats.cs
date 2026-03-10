using Echoes.Domain.Characters.Persistence;

namespace Echoes.Domain.Characters;

/// <summary>
/// Immutable DTO used for runtime game simulation.
/// <para>Derived from <see cref="ArchetypeEntity"/> via <see cref="ArchetypeEntity.ToStats"/>.</para>
/// </summary>
public record ArchetypeStats(
    AnitoClass ClassType,
    /// <summary> The flat amount of Health added to the player's MaxHealth upon leveling up. </summary>
    float HealthGrowthPerLevel,
    /// <summary> The flat amount of Mana added to the player's MaxMana upon leveling up. </summary>
    float ManaGrowthPerLevel,
    /// <summary>
    /// Scalar multiplier applied to raw damage before defense mitigation.
    /// e.g., 1.1f equals +10% bonus damage.
    /// </summary>
    float BaseDamageMultiplier,
    /// <summary>
    /// Modifier applied to the base movement speed.
    /// e.g., 1.05f equals +5% speed.
    /// </summary>
    float MovementSpeedModifier,
    /// <summary>
    /// Percentage reduction applied to ability cooldown calculations.
    /// e.g., 0.15f equals 15% reduction.
    /// </summary>
    float AbilityCooldownReduction
);

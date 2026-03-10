using Echoes.Domain.Common;

namespace Echoes.Domain.Characters.Persistence;

/// <summary>
/// Database aggregate representing persistent class growth templates.
/// <para>For the runtime simulation representation, see <see cref="ArchetypeStats"/>.</para>
/// </summary>
public class ArchetypeEntity : IAggregateRoot
{
    public AnitoClass ClassType { get; set; }
    public float HealthGrowthPerLevel { get; set; }
    public float ManaGrowthPerLevel { get; set; }
    public float BaseDamageMultiplier { get; set; }
    public float MovementSpeedModifier { get; set; }
    public float AbilityCooldownReduction { get; set; }

    /// <summary>
    /// Projects this database entity into a read-only <see cref="ArchetypeStats"/> snapshot.
    /// <para>Used by <c>ArchetypeRegistry</c> to seed the game simulation memory.</para>
    /// </summary>
    public ArchetypeStats ToStats() =>
        new(
            ClassType,
            HealthGrowthPerLevel,
            ManaGrowthPerLevel,
            BaseDamageMultiplier,
            MovementSpeedModifier,
            AbilityCooldownReduction
        );
}

namespace Echoes.Domain.Characters.Services;

/// <summary>
/// Provides access to static character class definitions (e.g., base HP,
/// growth rates, and skill templates).
/// </summary>
/// <remarks>
///Use this to
/// retrieve the 'blueprints' used to initialize character components
/// during spawning or level-up events.
/// </remarks>
public interface IArchetypeRegistry
{
    /// <summary>
    /// Loads character class definitions into memory from the data source.
    /// Must be called during server bootstrap.
    /// </summary>
    public Task InitializeAsync();

    /// <summary>
    /// Retrieves the static statistical template for a specific character class.
    /// </summary>
    /// <param name="classType">The enum representing the character class.</param>
    /// <returns>The <see cref="ArchetypeStats"/> containing baseline stats.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the class type is not defined in the registry.</exception>
    public ArchetypeStats GetStats(AnitoClass classType);
}

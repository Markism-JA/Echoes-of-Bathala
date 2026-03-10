namespace Echoes.Domain.Characters.Components;

/// <summary>
/// Marker tag identifying an entity as a client-controlled player.
/// </summary>
public struct PlayerTag { }

/// <summary>
/// Marker tag identifying an entity as a server-controlled NPC.
/// </summary>
public struct NpcTag { }

/// <summary>
/// Triggers a level-up logic check in the Simulation System.
/// </summary>
public struct LevelUpIntent { }

/// <summary>
/// Maps the entity to a persistent record in the database.
/// </summary>
public struct DatabaseLink
{
    public Guid CharacterId;
}

/// <summary>
/// Stores the display name of the entity.
/// </summary>
public struct Name
{
    public string Value;
}

/// <summary>
/// Tracks the network peer ID for communication synchronization.
/// </summary>
public struct NetworkPeerId
{
    public Guid PeerId;
}

/// <summary>
/// Defines the archetype-specific class for the character.
/// </summary>
public struct CharacterClassComponent
{
    public AnitoClass Value;
}

/// <summary>
/// Manages experience tracking and level requirements.
/// </summary>
public struct Progression
{
    public ushort Level; // 2 bytes
    public ushort SkillPoints; // 2 bytes
    public uint TitleId; // 4 bytes
    public long CurrentExperience; // 8 bytes
    public long ExperienceToNextLevel; // 8 bytes
} // Total size: 24 bytes

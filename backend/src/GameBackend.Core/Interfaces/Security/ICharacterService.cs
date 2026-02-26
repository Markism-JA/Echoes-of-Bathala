using GameBackend.Shared.DTOs.Gameplay;

namespace GameBackend.Core.Interfaces.Security;

public interface ICharacterService
{
    /// <summary>
    /// Creates a new character tied to the specific user.
    /// </summary>
    public Task<CharacterResponseDto> CreateCharacterAsync(
        Guid userId,
        CreateCharacterRequestDto request,
        CancellationToken ct = default
    );

    /// <summary>
    /// Fetches all non-deleted characters belonging to a user for the Character Selection screen.
    /// </summary>
    public Task<IEnumerable<CharacterResponseDto>> GetUserCharactersAsync(
        Guid userId,
        CancellationToken ct = default
    );

    /// <summary>
    /// A lightweight check for the UI to show a green checkmark if a name is free.
    /// </summary>
    public Task<bool> IsNameAvailableAsync(string name, CancellationToken ct = default);

    /// <summary>
    /// Soft-deletes a character.
    /// </summary>
    public Task DeleteCharacterAsync(Guid userId, Guid characterId, CancellationToken ct = default);
}

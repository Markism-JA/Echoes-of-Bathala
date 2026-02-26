namespace GameBackend.Core.Interfaces.Security;

/// <summary>
/// Defines domain rules for player identities, ensuring usernames are
/// respectful, lore-compliant, and resistant to impersonation/spoofing.
/// </summary>
public interface IUsernamePolicy
{
    /// <summary>
    /// Performs a security and Lore-compliance check on a proposed username.
    /// Detects profanity, identifies "reserved" game terms (like NPC or Admin names),
    /// and ensures the name fits the game's naming conventions.
    /// </summary>
    /// <param name="username">The raw username input provided by the user.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>
    /// A <see cref="UsernameValidationResult"/> indicating if the name is permitted
    /// for use in the game world.
    /// </returns>
    public Task<UsernameValidationResult> IsAllowedAsync(
        string username,
        CancellationToken ct = default
    );

    /// <summary>
    /// Normalizes a username to its "canonical" form to prevent identity spoofing
    /// via homoglyphs (e.g., '0' for 'O') or character variations.
    /// </summary>
    /// <param name="username">The username to standardize.</param>
    /// <returns>A flattened, lowercase string representing the base unique identity.</returns>
    /// <example>
    /// B@thala_123 -> bathala123
    /// </example>
    /// <remarks>
    /// This canonical version should be used for database uniqueness checks
    /// to prevent two users from having visually similar names.
    /// </remarks>
    public string Normalize(string username);
}

/// <summary>
/// Represents the outcome of a username validation attempt.
/// </summary>
/// <param name="IsValid">True if the username satisfies all Lore and security rules.</param>
/// <param name="ErrorMessage">The localized reason for rejection if <paramref name="IsValid"/> is false.</param>
public record UsernameValidationResult(bool IsValid, string ErrorMessage = "");

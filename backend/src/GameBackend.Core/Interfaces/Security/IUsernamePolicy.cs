namespace GameBackend.Core.Interfaces.Security;

public interface IUsernamePolicy
{
    /// <summary>
    /// Performs a security and lore-compliance check on a username.
    /// Uses Machine Learning to detect profanity and a local registry to protect reserved game terms.
    /// </summary>
    /// <param name="username">The raw username input from the user.</param>
    /// <param name="ct">Cancellation token for async operations.</param>
    /// <returns>True if the username is safe and available for use; otherwise, false.</returns>
    public Task<bool> IsAllowedAsync(string username, CancellationToken ct = default);

    /// <summary>
    /// Normalizes a username to its "canonical" form to prevent identity spoofing
    /// and bypasses using homoglyphs (e.g., 0 for o) or symbols (e.g., underscores).
    /// </summary>
    /// <param name="username">The username to standardize.</param>
    /// <returns>A flattened, lowercase string representing the unique identity.</returns>
    /// <example>B@thala_123 -> bathala123</example>
    public string Normalize(string username);
}

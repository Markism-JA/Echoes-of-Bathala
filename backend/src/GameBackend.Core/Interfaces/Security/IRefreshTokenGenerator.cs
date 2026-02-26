namespace GameBackend.Core.Interfaces.Security;

/// <summary>
/// Defines the contract for creating high-entropy, opaque strings used
/// as Refresh Tokens in the authentication lifecycle.
/// </summary>
public interface IRefreshTokenGenerator
{
    /// <summary>
    /// Generates a cryptographically secure, random string to represent a new refresh session.
    /// </summary>
    /// <returns>A unique, non-deterministic string (typically Base64 or Hex encoded).</returns>
    /// <remarks>
    /// Unlike JWTs, these tokens should be "opaque," meaning they carry no
    /// identity data themselves and must be validated against a data store.
    /// </remarks>
    public string GenerateToken();
}

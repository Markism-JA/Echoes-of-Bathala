using GameBackend.Core.Entities;

namespace GameBackend.Core.Interfaces.Security;

/// <summary>
/// Provides high-level functionality for generating JSON Web Tokens (JWT)
/// used to authorize requests in the GameBackend system.
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Generates a cryptographically signed JWT for a specific user containing
    /// essential identity claims (e.g., UserId, Username, Email).
    /// </summary>
    /// <param name="user">The user entity whose information will be embedded in the token claims.</param>
    /// <param name="now">
    /// The reference timestamp used as the 'IssuedAt' (iat) value to ensure
    /// synchronization with the database session records.
    /// </param>
    /// <returns>
    /// A tuple containing:
    /// <list type="bullet">
    /// <item><description><c>Token</c>: The encoded and signed JWT string.</description></item>
    /// <item><description><c>Expiration</c>: The exact UTC timestamp when this token becomes invalid.</description></item>
    /// </list>
    /// </returns>
    /// <remarks>
    /// The expiration duration is determined by the system's <c>JwtSettings</c> configuration.
    /// </remarks>
    public (string Token, DateTime Expiration) GenerateToken(User user, DateTime now);
}

using Echoes.Domain.Users.Persistence;

namespace Echoes.Application.Auth.Abstractions;

/// <summary>
/// Defines a contract for generating JSON Web Tokens (JWT) that encapsulate
/// both user identity and authorization context.
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Generates a signed JWT containing authenticated user claims and access rights.
    /// </summary>
    /// <param name="user">The user entity for whom the token is being generated.</param>
    /// <param name="utcNow">The current UTC time to serve as the baseline for token issuance and expiration.</param>
    /// <returns>
    /// A tuple containing:
    /// <list type="bullet">
    /// <item><description><c>Token</c>: The encoded and signed JWT string.</description></item>
    /// <item><description><c>Expiration</c>: The exact UTC timestamp when this token becomes invalid.</description></item>
    /// </list>
    /// </returns>
    /// <remarks>
    /// The generated token is expected to contain:
    /// <list type="number">
    /// <item><description><b>Identity:</b> sub, email, unique_name.</description></item>
    /// <item><description><b>Security:</b> jti (for uniqueness) and iat (Issued At) for temporal validation.</description></item>
    /// </list>
    /// </remarks>
    public (string Token, DateTime Expiration) GenerateToken(UserEntity user, DateTime utcNow);
}

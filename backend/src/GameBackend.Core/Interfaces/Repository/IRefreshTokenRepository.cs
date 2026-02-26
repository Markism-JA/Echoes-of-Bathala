using GameBackend.Core.Entities;

namespace GameBackend.Core.Interfaces.Repository;

/// <summary>
/// Repository for managing the lifecycle of Refresh Tokens,
/// facilitating session persistence and rotation.
/// </summary>
public interface IRefreshTokenRepository : IRepository<RefreshToken, Guid>
{
    /// <summary>
    /// Retrieves a specific refresh token by its unique string value.
    /// </summary>
    /// <param name="token">The cryptographically secure token string.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The <see cref="RefreshToken"/> if found; otherwise, null.</returns>
    public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default);

    /// <summary>
    /// Invalidates all active tokens for a specific user.
    /// Primarily used for security breaches, password changes, or global logouts.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="ct">The cancellation token.</param>
    public Task RevokeAllForUserAsync(Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Invalidates a single refresh token by marking it as revoked.
    /// Typically called during an explicit user logout.
    /// </summary>
    /// <param name="token">The token string to revoke.</param>
    /// <param name="ct">The cancellation token.</param>
    public Task RevokeTokenAsync(string token, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a collection of tokens for a user that are neither revoked nor used.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A list of active <see cref="RefreshToken"/> entities.</returns>
    public Task<List<RefreshToken>> GetActiveTokensForUserAsync(
        Guid userId,
        CancellationToken ct = default
    );

    /// <summary>
    /// Enforces a session cap by revoking the oldest active tokens until the count
    /// falls below the specified <paramref name="maxSessions"/>.
    /// </summary>
    /// <param name="userId">The user whose sessions are being capped.</param>
    /// <param name="maxSessions">The maximum number of concurrent active sessions allowed.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <remarks>
    /// This follows a First-In-First-Out (FIFO) logic to maintain the "Golden Middle" session strategy.
    /// </remarks>
    public Task RevokeExcessTokensAsync(
        Guid userId,
        int maxSessions,
        CancellationToken ct = default
    );
}

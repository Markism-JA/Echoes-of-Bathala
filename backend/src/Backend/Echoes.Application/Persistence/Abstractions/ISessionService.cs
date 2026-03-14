using Echoes.Domain.Auth;

namespace Echoes.Application.Persistence.Abstractions;

/// <summary>
/// Defines the contract for managing the lifecycle and security of user authentication sessions.
/// </summary>
/// <remarks>
/// This service is responsible for the stateful management of <see cref="RefreshToken"/> instances.
/// Implementations should handle session persistence, concurrency limits (multi-device management),
/// and validation logic to ensure tokens remain secure and revocable.
/// </remarks>
public interface ISessionService
{
    /// <summary>
    /// Registers a new session for a user and persists the associated metadata.
    /// </summary>
    /// <param name="token">The domain model representing the refresh token and user context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// <b>Implementer Note:</b> This method should ideally enforce business rules such as
    /// "Maximum Concurrent Sessions" by evicting the oldest session if a user exceeds the limit.
    /// </remarks>
    public Task CreateSessionAsync(RefreshToken token);

    /// <summary>
    /// Explicitly terminates an active session, rendering the associated token unusable.
    /// </summary>
    /// <param name="token">The token to be invalidated.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This is typically called during a "Logout" or when a specific device is remotely revoked.
    /// </remarks>
    public Task RevokeSessionAsync(RefreshToken token);

    /// <summary>
    /// Verifies whether a session is still active and has not been revoked or expired.
    /// </summary>
    /// <param name="token">The token to validate against the session store.</param>
    /// <returns><c>true</c> if the session exists and is valid; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// This check should be performed during the "Token Exchange" (refreshing an Access Token)
    /// to ensure the user's session hasn't been killed by the system or a manual logout.
    /// </remarks>
    public Task<bool> IsSessionValidAsync(RefreshToken token);
}

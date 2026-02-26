using ErrorOr;
using GameBackend.Shared.DTOs.Identity;

namespace GameBackend.Core.Interfaces.Security;

/// <summary>
/// Provides high-level authentication and identity management services,
/// coordinating between domain policies and persistence.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new player account after validating against domain policies
    /// and provides an initial authenticated session.
    /// </summary>
    /// <param name="request">The registration details including username, email, and password.</param>
    /// <param name="ct">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// An <see cref="ErrorOr{AuthResponseDto}"/> containing the generated tokens and user info
    /// if successful; otherwise, a collection of validation or conflict errors.
    /// </returns>
    public Task<ErrorOr<AuthResponseDto>> RegisterAsync(
        RegisterRequestDto request,
        CancellationToken ct = default
    );

    /// <summary>
    /// Authenticates a user based on their credentials and establishes a new session,
    /// while enforcing concurrent session limits.
    /// </summary>
    /// <param name="request">The login credentials (email and password).</param>
    /// <param name="ct">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// An <see cref="ErrorOr{AuthResponseDto}"/> containing fresh tokens if credentials match;
    /// otherwise, a generic authentication error to prevent account enumeration.
    /// </returns>
    public Task<ErrorOr<AuthResponseDto>> LoginAsync(
        LoginRequestDto request,
        CancellationToken ct = default
    );

    /// <summary>
    /// Rotates an existing session by exchanging a valid Refresh Token for a new set of tokens.
    /// </summary>
    /// <param name="request">The request containing the current Refresh Token to be rotated.</param>
    /// <param name="ct">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A new pair of Access and Refresh tokens. The provided token is marked as used
    /// to prevent replay attacks (Token Rotation).
    /// </returns>
    /// <remarks>
    /// If the token is expired, revoked, or already used, an error is returned
    /// requiring the user to perform a full login.
    /// </remarks>
    public Task<ErrorOr<AuthResponseDto>> RefreshTokenAsync(
        RefreshTokenRequestDto request,
        CancellationToken ct = default
    );

    /* /// <summary>
    /// Explicitly terminates a session by revoking the provided Refresh Token.
    /// </summary>
    /// <param name="refreshToken">The token to be invalidated.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A success result if the token was found and invalidated.</returns>
    Task<ErrorOr<Deleted>> LogoutAsync(
        string refreshToken,
        CancellationToken ct = default
    );
    */
}

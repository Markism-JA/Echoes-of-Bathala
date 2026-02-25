using ErrorOr;
using GameBackend.Shared.DTOs.Identity;

namespace GameBackend.Core.Interfaces.Security
{
    public interface IAuthService
    {
        /// <summary>
        /// Creates a new user account and returns the initial auth tokens.
        /// </summary>
        public Task<ErrorOr<AuthResponseDto>> RegisterAsync(
            RegisterRequestDto request,
            CancellationToken ct = default
        );

        /// <summary>
        /// Validates credentials and returns auth tokens.
        /// </summary>
        public Task<ErrorOr<AuthResponseDto>> LoginAsync(
            LoginRequestDto request,
            CancellationToken ct = default
        );

        /// <summary>
        /// Trades an expired JWT and a valid Refresh Token for a fresh set.
        /// </summary>
        public Task<ErrorOr<AuthResponseDto>> RefreshTokenAsync(
            RefreshTokenRequestDto request,
            CancellationToken ct = default
        );
    }
}

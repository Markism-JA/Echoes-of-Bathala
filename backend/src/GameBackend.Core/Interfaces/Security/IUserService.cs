using GameBackend.Shared.DTOs.Identity;

namespace GameBackend.Core.Interfaces.Security
{
    public interface IUserService
    {
        /// <summary>
        /// Fetches the public or private profile data for a specific user.
        /// </summary>
        public Task<UserResponseDto> GetProfileAsync(Guid userId, CancellationToken ct = default);

        /// <summary>
        /// Updates user settings, like changing an email or display name.
        /// </summary>
        // Task UpdateProfileAsync(Guid userId, UpdateProfileRequestDto request, CancellationToken ct = default);

        /// <summary>
        /// Handles password changes for an already logged-in user.
        /// </summary>
        // Task ChangePasswordAsync(Guid userId, ChangePasswordRequestDto request, CancellationToken ct = default);
    }
}

using ErrorOr;
using GameBackend.Core.Interfaces.Repository;
using GameBackend.Core.Interfaces.Security;
using GameBackend.Shared.DTOs.Identity;
using GameBackend.Shared.Errors;

namespace GameBackend.Core.Services
{
    public class AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUsernamePolicy usernamePolicy
    ) : IAuthService
    {
        public Task<ErrorOr<AuthResponseDto>> LoginAsync(
            LoginRequestDto request,
            CancellationToken ct = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<ErrorOr<AuthResponseDto>> RefreshTokenAsync(
            RefreshTokenRequestDto request,
            CancellationToken ct = default
        )
        {
            throw new NotImplementedException();
        }

        public async Task<ErrorOr<AuthResponseDto>> RegisterAsync(
            RegisterRequestDto request,
            CancellationToken ct = default
        )
        {
            if (!await usernamePolicy.IsAllowedAsync(request.Username, ct))
                return GameErrors.Auth.ProfaneUsername;

            var normalizedUsername = usernamePolicy.Normalize(request.Username);

            var emailCheckTask = userRepository.IsEmailTakenAsync(request.Email, ct);
            var usernameCheckTask = userRepository.IsUserNameTakenAsync(normalizedUsername, ct);

            await Task.WhenAll(emailCheckTask, usernameCheckTask);

            if (await emailCheckTask)
                return GameErrors.Auth.EmailTaken;

            if (await usernameCheckTask)
                return GameErrors.Auth.UsernameTaken;

            var hashedPassword = passwordHasher.HashPassword(request.Password);

            throw new NotImplementedException();
        }
    }
}

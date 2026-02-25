using GameBackend.Core.Interfaces.Repository;
using GameBackend.Core.Interfaces.Security;
using GameBackend.Shared.DTOs.Identity;

namespace GameBackend.Core.Services
{
    public class AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUsernamePolicy usernamePolicy
    ) : IAuthService
    {
        public Task<AuthResponseDto> LoginAsync(
            LoginRequestDto request,
            CancellationToken ct = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponseDto> RefreshTokenAsync(
            RefreshTokenRequestDto request,
            CancellationToken ct = default
        )
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResponseDto> RegisterAsync(
            RegisterRequestDto request,
            CancellationToken ct = default
        )
        {
            if (!await usernamePolicy.IsAllowedAsync(request.Username, ct))
                throw new InvalidOperationException(
                    "Username contains forbidden words or is reserved."
                );

            var normalizedUsername = usernamePolicy.Normalize(request.Username);

            var emailCheckTask = userRepository.IsEmailTakenAsync(request.Email, ct);
            var usernameCheckTask = userRepository.IsUserNameTakenAsync(normalizedUsername, ct);

            await Task.WhenAll(emailCheckTask, usernameCheckTask);

            if (await emailCheckTask)
                throw new InvalidOperationException("Email is already taken.");

            if (await usernameCheckTask)
                throw new InvalidOperationException("Username is already taken.");

            var hashedPassword = passwordHasher.HashPassword(request.Password);

            throw new NotImplementedException();
        }
    }
}

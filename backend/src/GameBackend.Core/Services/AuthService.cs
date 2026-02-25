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
        IUsernamePolicy usernamePolicy,
        IEmailPolicy emailPolicy
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
            var usernamePolicyResult = await usernamePolicy.IsAllowedAsync(request.Username, ct);
            if (!usernamePolicyResult.IsValid)
            {
                return usernamePolicyResult.ErrorMessage switch
                {
                    "Username is required." => GameErrors.Auth.UsernameRequired,
                    "This username is reserved for system use." => GameErrors.Auth.UsernameReserved,
                    "Username contains forbidden language." => GameErrors.Auth.UsernameProfane,

                    _ => Error.Validation(
                        code: GameErrors.Auth.UsernameInvalid.Code,
                        description: usernamePolicyResult.ErrorMessage
                            ?? GameErrors.Auth.UsernameInvalid.Description
                    ),
                };
            }

            var emailPolicyResult = await emailPolicy.ValidateAsync(request.Email, ct);
            if (!emailPolicyResult.IsValid)
            {
                return emailPolicyResult.ErrorMessage switch
                {
                    "Email is required." => GameErrors.Auth.EmailRequired,
                    "Disposable email addresses are not allowed." => GameErrors
                        .Auth
                        .EmailDisposable,
                    "Invalid email format." => GameErrors.Auth.EmailInvalid,
                    _ => Error.Validation(
                        code: GameErrors.Auth.EmailInvalid.Code,
                        description: emailPolicyResult.ErrorMessage
                            ?? GameErrors.Auth.EmailInvalid.Description
                    ),
                };
            }

            var normalizedUsername = usernamePolicy.Normalize(request.Username);
            var normalizedEmail = emailPolicy.Normalize(request.Email);

            var emailCheckTask = userRepository.IsEmailTakenAsync(normalizedEmail, ct);
            var usernameCheckTask = userRepository.IsUserNameTakenAsync(normalizedUsername, ct);

            await Task.WhenAll(emailCheckTask, usernameCheckTask);

            if (await emailCheckTask)
                return GameErrors.Auth.EmailTaken;

            if (await usernameCheckTask)
                return GameErrors.Auth.UsernameTaken;

            // 5. Success Flow
            var hashedPassword = passwordHasher.HashPassword(request.Password);

            // TODO: Map to User Entity and Persist
            // var user = User.Create(request.Username, normalizedUsername, request.Email, normalizedEmail, hashedPassword);
            // await userRepository.AddAsync(user, ct);

            throw new NotImplementedException("Next step: User persistence and JWT generation.");
        }
    }
}

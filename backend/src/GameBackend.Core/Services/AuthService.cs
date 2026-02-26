using ErrorOr;
using GameBackend.Core.Entities;
using GameBackend.Core.Interfaces.Persistence;
using GameBackend.Core.Interfaces.Repository;
using GameBackend.Core.Interfaces.Security;
using GameBackend.Core.Interfaces.Services;
using GameBackend.Shared.DTOs.Identity;
using GameBackend.Shared.Errors;

namespace GameBackend.Core.Services
{
    public class AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUsernamePolicy usernamePolicy,
        IEmailPolicy emailPolicy,
        IPasswordPolicy passwordPolicy,
        IUnitOfWork unitOfWork,
        IJwtTokenGenerator jwtTokenGenerator,
        IDateTimeProvider dateTimeProvider,
        IRefreshTokenRepository refreshTokenRepository,
        IRefreshTokenGenerator refreshTokenGenerator
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
            var validationResult = await ValidateRegistrationPolicies(request, ct);
            if (validationResult.IsError)
                return validationResult.Errors;

            var normalizedUsername = usernamePolicy.Normalize(request.Username);
            var normalizedEmail = emailPolicy.Normalize(request.Email);

            var availabilityResult = await CheckAvailabilityAsync(
                normalizedUsername,
                normalizedEmail,
                ct
            );
            if (availabilityResult.IsError)
                return availabilityResult.Errors;

            var user = User.Create(
                request.Username,
                request.Email,
                passwordHasher.HashPassword(null!, request.Password),
                normalizedUsername,
                normalizedEmail,
                dateTimeProvider.UtcNow
            );

            await userRepository.AddAsync(user, ct);

            var refreshTokenString = refreshTokenGenerator.GenerateToken();
            var refreshToken = RefreshToken.Create(
                refreshTokenString,
                user.Id,
                dateTimeProvider.UtcNow.AddDays(7),
                dateTimeProvider.UtcNow
            );

            await refreshTokenRepository.AddAsync(refreshToken, ct);

            await unitOfWork.SaveChangesAsync(ct);

            var (accessToken, expiration) = jwtTokenGenerator.GenerateToken(user);

            var userDto = new UserResponseDto(user.Id, user.UserName!, user.Email!, user.CreatedAt);

            return new AuthResponseDto(accessToken, refreshTokenString, expiration, userDto);
        }

        private async Task<ErrorOr<Success>> ValidateRegistrationPolicies(
            RegisterRequestDto request,
            CancellationToken ct
        )
        {
            var userResult = await usernamePolicy.IsAllowedAsync(request.Username, ct);
            if (!userResult.IsValid)
                return MapUsernameError(userResult.ErrorMessage);

            var emailResult = await emailPolicy.ValidateAsync(request.Email, ct);
            if (!emailResult.IsValid)
                return MapEmailError(emailResult.ErrorMessage);

            var passResult = passwordPolicy.Validate(
                request.Password,
                request.Username,
                request.Email
            );
            if (!passResult.IsValid)
                return MapPasswordError(passResult.ErrorMessage);

            return Result.Success;
        }

        private Error MapUsernameError(string? message) =>
            message switch
            {
                "Username is required." => GameErrors.Auth.UsernameRequired,
                "This username is reserved for system use." => GameErrors.Auth.UsernameReserved,
                "Username contains forbidden language." => GameErrors.Auth.UsernameProfane,
                _ => Error.Validation(
                    GameErrors.Auth.UsernameInvalid.Code,
                    message ?? GameErrors.Auth.UsernameInvalid.Description
                ),
            };

        private Error MapEmailError(string? message) =>
            message switch
            {
                "Email is required." => GameErrors.Auth.EmailRequired,
                "Disposable email addresses are not allowed." => GameErrors.Auth.EmailDisposable,
                "Invalid email format." => GameErrors.Auth.EmailInvalid,
                _ => Error.Validation(
                    GameErrors.Auth.EmailInvalid.Code,
                    message ?? GameErrors.Auth.EmailInvalid.Description
                ),
            };

        private Error MapPasswordError(string? message) =>
            message switch
            {
                "Password is required." => GameErrors.Auth.PasswordRequired,
                "Password must be at least 8 characters long." => GameErrors.Auth.PasswordTooShort,
                _ => Error.Validation(
                    GameErrors.Auth.PasswordTooWeak.Code,
                    message ?? GameErrors.Auth.PasswordTooWeak.Description
                ),
            };

        private async Task<ErrorOr<Success>> CheckAvailabilityAsync(
            string normName,
            string normEmail,
            CancellationToken ct
        )
        {
            var emailTask = userRepository.IsEmailTakenAsync(normEmail, ct);
            var userTask = userRepository.IsUserNameTakenAsync(normName, ct);

            await Task.WhenAll(emailTask, userTask);

            if (await emailTask)
                return GameErrors.Auth.EmailTaken;
            if (await userTask)
                return GameErrors.Auth.UsernameTaken;

            return Result.Success;
        }
    }
}

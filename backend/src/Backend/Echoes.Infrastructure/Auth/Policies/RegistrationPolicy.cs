using Echoes.Application.Auth;
using Echoes.Application.Auth.Policies;
using Echoes.Application.Auth.RegisterEmail;
using Echoes.Domain.Auth;
using Echoes.Domain.Users.Persistence;
using Echoes.Shared.Abstraction.Security;
using Echoes.Shared.Network.Common;
using ErrorOr;

namespace Echoes.Infrastructure.Auth.Policies
{
    public class RegistrationPolicy(
        IUserNamePolicy userNamePolicy,
        IEmailPolicy emailPolicy,
        IPasswordPolicy passwordPolicy,
        IUserRepository userRepository
    ) : IRegistrationPolicy
    {
        public async Task<ErrorOr<RegistrationDetails>> IsAllowedAsync(
            RegisterEmailCommand command,
            CancellationToken ct
        )
        {
            var userResult = await userNamePolicy.IsAllowedAsync(command.Username, ct);
            if (!userResult.IsValid)
                return MapUsernameError(userResult);

            var emailResult = await emailPolicy.IsAllowedAsync(command.Email, ct);
            if (!emailResult.IsValid)
                return MapEmailError(emailResult);

            var passResult = await passwordPolicy.IsAllowedAsync(
                command.Password,
                command.Username,
                command.Email
            );
            if (!passResult.IsValid)
                return MapPasswordError(passResult);

            var normalizedUsername = userNamePolicy.Normalize(command.Username);
            var normalizedEmail = emailPolicy.Normalize(command.Email);

            if (await userRepository.IsUserNameTakenAsync(normalizedUsername, ct))
                return Errors.Username.Taken;

            if (await userRepository.IsEmailTakenAsync(normalizedEmail, ct))
                return Errors.Email.Taken;

            return new RegistrationDetails(normalizedUsername, normalizedEmail);
        }

        private Error MapUsernameError(ValidationResult result) =>
            result.ErrorCode switch
            {
                AuthErrorCodes.Username.Required => Errors.Username.Required,
                AuthErrorCodes.Username.Reserved => Errors.Username.Reserved,
                AuthErrorCodes.Username.Profane => Errors.Username.Profane,
                _ => throw new InvalidOperationException(
                    $"New error code '{result.ErrorCode}' was returned by UsernamePolicy but not mapped in RegistrationPolicy."
                ),
            };

        private Error MapEmailError(ValidationResult result) =>
            result.ErrorCode switch
            {
                AuthErrorCodes.Email.Required => Errors.Email.Required,
                AuthErrorCodes.Email.Disposable => Errors.Email.Disposable,
                AuthErrorCodes.Email.Invalid => Errors.Email.Invalid,
                _ => throw new InvalidOperationException(
                    $"New error code '{result.ErrorCode}' was returned by EmailPolicy but not mapped in RegistrationPolicy."
                ),
            };

        private Error MapPasswordError(ValidationResult result) =>
            result.ErrorCode switch
            {
                AuthErrorCodes.Password.Required => Errors.Password.Required,
                AuthErrorCodes.Password.TooShort => Errors.Password.TooShort,
                AuthErrorCodes.Password.TooWeak => Error.Validation(
                    code: Errors.Password.TooWeak.Code,
                    description: result.ErrorMessage
                ),
                _ => throw new InvalidOperationException(
                    $"New error code '{result.ErrorCode}' was returned by PasswordPolicy but not mapped in RegistrationPolicy."
                ),
            };
    }
}

using Echoes.Application.Auth.RegisterEmail;
using Echoes.Application.Common;
using Echoes.Application.Common.Identity;
using Echoes.Domain.Repository;
using Echoes.Shared.Abstraction.Security;
using Echoes.Shared.Network.Constants;
using ErrorOr;

namespace Echoes.Infrastructure.Security
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
                return Errors.Auth.Username.Taken;

            if (await userRepository.IsEmailTakenAsync(normalizedEmail, ct))
                return Errors.Auth.Email.Taken;

            return new RegistrationDetails(normalizedUsername, normalizedEmail);
        }

        private Error MapUsernameError(ValidationResult result) =>
            result.ErrorCode switch
            {
                AuthErrorCodes.Username.Required => Errors.Auth.Username.Required,
                AuthErrorCodes.Username.Reserved => Errors.Auth.Username.Reserved,
                AuthErrorCodes.Username.Profane => Errors.Auth.Username.Profane,
                _ => throw new InvalidOperationException(
                    $"New error code '{result.ErrorCode}' was returned by UsernamePolicy but not mapped in RegistrationPolicy."
                ),
            };

        private Error MapEmailError(ValidationResult result) =>
            result.ErrorCode switch
            {
                AuthErrorCodes.Email.Required => Errors.Auth.Email.Required,
                AuthErrorCodes.Email.Disposable => Errors.Auth.Email.Disposable,
                AuthErrorCodes.Email.Invalid => Errors.Auth.Email.Invalid,
                _ => throw new InvalidOperationException(
                    $"New error code '{result.ErrorCode}' was returned by EmailPolicy but not mapped in RegistrationPolicy."
                ),
            };

        private Error MapPasswordError(ValidationResult result) =>
            result.ErrorCode switch
            {
                AuthErrorCodes.Password.Required => Errors.Auth.Password.Required,
                AuthErrorCodes.Password.TooShort => Errors.Auth.Password.TooShort,
                AuthErrorCodes.Password.TooWeak => Error.Validation(
                    code: Errors.Auth.Password.TooWeak.Code,
                    description: result.ErrorMessage
                ),
                _ => throw new InvalidOperationException(
                    $"New error code '{result.ErrorCode}' was returned by PasswordPolicy but not mapped in RegistrationPolicy."
                ),
            };
    }
}

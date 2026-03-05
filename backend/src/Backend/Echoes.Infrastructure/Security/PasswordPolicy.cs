using Echoes.Shared.Abstraction.Security;
using Echoes.Shared.Network.Constants;
using Zxcvbn;

namespace Echoes.Infrastructure.Security
{
    public class PasswordPolicy : IPasswordPolicy
    {
        public Task<ValidationResult> IsAllowedAsync(
            string password,
            string? username = null,
            string? email = null
        )
        {
            var result = Core.EvaluatePassword(password, new[] { username, email });

            if (result.Score < 3)
            {
                var warning = result.Feedback.Warning ?? "Password is too weak.";

                var suggestions =
                    result.Feedback.Suggestions != null
                        ? string.Join(" ", result.Feedback.Suggestions)
                        : string.Empty;

                var errorMessage = $"{warning} {suggestions}".Trim();

                return Task.FromResult(
                    new ValidationResult(false, AuthErrorCodes.Password.TooWeak, errorMessage)
                );
            }

            return Task.FromResult(new ValidationResult(true));
        }
    }
}

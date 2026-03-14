using Echoes.Shared.Abstraction.Security;
using Echoes.Shared.Network.Common;
using Zxcvbn;

namespace Echoes.Infrastructure.Auth.Policies
{
    /// <summary>
    /// Implements realistic password strength estimation using the Zxcvbn algorithm.
    /// </summary>
    /// <remarks>
    /// This policy moves beyond simple character-set requirements by detecting common
    /// patterns, dictionary words, and sequential characters. It specifically checks for
    /// the presence of the user's identifiers (email/username) inside the password
    /// to prevent easily guessable credentials.
    /// </remarks>
    public class PasswordPolicy : IPasswordPolicy
    {
        /// <inheritdoc />
        /// <remarks>
        /// Requires a Zxcvbn score of at least 3 (Strong) out of 4 (Very Strong).
        /// Scores 0-2 (Very Weak to Weak) are rejected with localized feedback suggestions.
        /// </remarks>
        public Task<ValidationResult> IsAllowedAsync(
            string password,
            string? username = null,
            string? email = null
        )
        {
            var result = Core.EvaluatePassword(password, [username, email]);

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

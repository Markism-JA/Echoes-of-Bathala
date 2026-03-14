using DotnetBadWordDetector;
using Echoes.Shared.Abstraction.Security;
using Echoes.Shared.Network.Common;

namespace Echoes.Infrastructure.Auth.Policies
{
    /// <summary>
    /// Implements username validation using a combination of a reserved keyword list
    /// and an external profanity detection service.
    /// </summary>
    /// <remarks>
    /// This implementation is designed to defeat "evasion" tactics (like replacing 'S' with '5')
    /// by normalizing the input before performing security checks.
    /// </remarks>
    public class UserNamePolicy(ProfanityDetector profanityDetector) : IUserNamePolicy
    {
        /// NOTE: In prod this should ideally connect to a db table.
        private static readonly HashSet<string> _reservedNames = new(
            StringComparer.OrdinalIgnoreCase
        )
        {
            "Admin",
            "System",
            "Support",
            "Bathala",
            "GameMaster",
            "Moderator",
        };

        /// <inheritdoc />
        /// <remarks>
        /// Checks for empty inputs, system-reserved names (Admin, GameMaster, etc.),
        /// and forbidden language via the <see cref="ProfanityDetector"/>.
        /// </remarks>
        public Task<ValidationResult> IsAllowedAsync(
            string username,
            CancellationToken ct = default
        )
        {
            ct.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(username))
                return Task.FromResult(
                    new ValidationResult(
                        false,
                        AuthErrorCodes.Username.Required,
                        "Username is required."
                    )
                );

            if (IsReserved(username))
                return Task.FromResult(
                    new ValidationResult(
                        false,
                        AuthErrorCodes.Username.Reserved,
                        "This username is reserved for system use."
                    )
                );

            if (profanityDetector.IsProfane(username))
                return Task.FromResult(
                    new ValidationResult(
                        false,
                        AuthErrorCodes.Username.Profane,
                        "Username contains forbidden language."
                    )
                );

            return Task.FromResult(new ValidationResult(true));
        }

        /// <summary>
        /// Aggressively normalizes strings to their phonetic/visual equivalents.
        /// Maps 1->I, 0->O, 3->E, etc., and removes all punctuation.
        /// </summary>
        public string Normalize(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return string.Empty;

            return username
                .ToUpperInvariant()
                .Replace("@", "a")
                .Replace("1", "i")
                .Replace("0", "o")
                .Replace("3", "e")
                .Replace("5", "s")
                .Replace("7", "t")
                .Replace("_", "")
                .Replace(".", "");
        }

        private bool IsReserved(string input)
        {
            string normalized = Normalize(input);

            foreach (var reserved in _reservedNames)
            {
                if (normalized.Contains(reserved, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}

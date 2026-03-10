using DotnetBadWordDetector;
using Echoes.Shared.Abstraction.Security;
using Echoes.Shared.Network.Common;

namespace Echoes.Infrastructure.Auth.Policies
{
    public class UserNamePolicy(ProfanityDetector profanityDetector) : IUserNamePolicy
    {
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

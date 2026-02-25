using DotnetBadWordDetector;
using GameBackend.Core.Interfaces.Security;

namespace GameBackend.Infra.Security
{
    public class UsernamePolicy : IUsernamePolicy
    {
        private readonly ProfanityDetector _detector;
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

        public UsernamePolicy()
        {
            _detector = new ProfanityDetector();
        }

        public async Task<bool> IsAllowedAsync(string username, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            if (IsReserved(username))
                return false;

            return !_detector.IsProfane(username);
        }

        public string Normalize(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return string.Empty;

            return username
                .ToLowerInvariant()
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

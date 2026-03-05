using System.Net.Mail;
using Echoes.Shared.Abstraction.Security;
using Echoes.Shared.Network.Constants;

namespace Echoes.Infrastructure.Security
{
    public class EmailPolicy : IEmailPolicy
    {
        private static readonly HashSet<string> _disposableDomains = new(
            StringComparer.OrdinalIgnoreCase
        )
        {
            "10minutemail.com",
            "tempmail.com",
            "mailinator.com",
            "guerrillamail.com",
            "yopmail.com",
        };

        public Task<ValidationResult> IsAllowedAsync(string email, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            if (string.IsNullOrWhiteSpace(email))
                return Task.FromResult(
                    new ValidationResult(false, AuthErrorCodes.Email.Required, "Email is required.")
                );
            var addr = new MailAddress(email);
            var domain = addr.Host;

            if (_disposableDomains.Contains(domain))
            {
                return Task.FromResult(
                    new ValidationResult(
                        false,
                        AuthErrorCodes.Email.Disposable,
                        "Disposable email addresses are not allowed."
                    )
                );
            }
            return Task.FromResult(new ValidationResult(true));
        }

        public string Normalize(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return string.Empty;

            var parts = email.ToUpperInvariant().Trim().Split('@');
            if (parts.Length != 2)
                return email.ToUpperInvariant();

            var user = parts[0];
            var domain = parts[1];

            if (domain == "gmail.com" || domain == "googlemail.com")
            {
                user = user.Split('+')[0].Replace(".", "");
            }

            return $"{user}@{domain}";
        }
    }
}

using System.Net.Mail;
using Echoes.Shared.Abstraction.Security;
using Echoes.Shared.Network.Common;

namespace Echoes.Infrastructure.Auth.Policies
{
    /// <summary>
    /// Enforces business-level rules for email addresses, including anti-spam measures
    /// and canonical normalization.
    /// </summary>
    /// <remarks>
    /// This policy handles security concerns such as blocking disposable "burn" email
    /// services and preventing "plus-addressing" exploits (specifically for Gmail)
    /// to ensure unique user identities.
    /// </remarks>
    public class EmailPolicy : IEmailPolicy
    {
        // NOTE: In prod this should query a db table or an external service that contains a flag for disposable domains.
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

        /// <summary>
        /// Validates an email address against a blacklist of known disposable providers.
        /// </summary>
        /// <param name="email">The email address to evaluate.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating success or a specific anti-spam error.</returns>
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

        /// <summary>
        /// Transforms an email into a canonical format to prevent duplicate accounts.
        /// </summary>
        /// <remarks>
        /// For Gmail/Googlemail, it removes dots and ignores "plus-suffixing"
        /// (e.g., 'john.doe+game@gmail.com' becomes 'JOHNDOE@GMAIL.COM').
        /// </remarks>
        /// <param name="email">The raw email string.</param>
        /// <returns>The normalized, uppercase canonical email.</returns>
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

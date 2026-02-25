using System.Net.Mail;
using GameBackend.Core.Interfaces.Security;

namespace GameBackend.Infra.Security;

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

    public Task<EmailValidationResult> ValidateAsync(string email, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        if (string.IsNullOrWhiteSpace(email))
            return Task.FromResult(new EmailValidationResult(false, "Email is required."));

        try
        {
            var addr = new MailAddress(email);
            var domain = addr.Host;

            if (_disposableDomains.Contains(domain))
            {
                return Task.FromResult(
                    new EmailValidationResult(false, "Disposable email addresses are not allowed.")
                );
            }

            return Task.FromResult(new EmailValidationResult(true));
        }
        catch (FormatException)
        {
            return Task.FromResult(new EmailValidationResult(false, "Invalid email format."));
        }
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

namespace GameBackend.Core.Interfaces.Security;

public interface IEmailPolicy
{
    /// <summary>
    /// Performs comprehensive validation of an email address.
    /// Includes format, domain blocklisting, and disposable email detection.
    /// </summary>
    public Task<EmailValidationResult> ValidateAsync(string email, CancellationToken ct = default);

    /// <summary>
    /// Normalizes an email to its "canonical" form to prevent duplicate accounts
    /// via Gmail "plus-addressing" or "dot" tricks.
    /// </summary>
    /// <example>l.a.k.a.n+game@gmail.com -> lakan@gmail.com</example>
    public string Normalize(string email);
}

public record EmailValidationResult(bool IsValid, string ErrorMessage = "");

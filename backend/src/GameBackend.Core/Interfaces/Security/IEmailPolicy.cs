namespace GameBackend.Core.Interfaces.Security;

/// <summary>
/// Defines the domain rules for email addresses, ensuring data integrity
/// and preventing account duplication through various email aliasing techniques.
/// </summary>
public interface IEmailPolicy
{
    /// <summary>
    /// Performs comprehensive validation of an email address.
    /// Checks format, domain blocklisting, and detects disposable (temporary) email providers.
    /// </summary>
    /// <param name="email">The raw email address string provided by the user.</param>
    /// <param name="ct">A token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="EmailValidationResult"/> indicating success or a specific failure reason.</returns>
    public Task<EmailValidationResult> ValidateAsync(string email, CancellationToken ct = default);

    /// <summary>
    /// Normalizes an email to its "canonical" form to prevent duplicate accounts
    /// via Gmail "plus-addressing" or "dot" tricks.
    /// </summary>
    /// <param name="email">The email address to normalize.</param>
    /// <returns>A uppercased, stripped version of the email used for database lookups.</returns>
    /// <example>
    /// l.a.k.a.n+game@gmail.com becomes lakan@gmail.com.
    /// </example>
    /// <remarks>
    /// This should be used before any "IsEmailTaken" check or "GetByEmail" query
    /// to ensure a single identity per physical email inbox.
    /// </remarks>
    public string Normalize(string email);
}

/// <summary>
/// Represents the result of an email validation operation.
/// </summary>
/// <param name="IsValid">True if the email meets all domain requirements.</param>
/// <param name="ErrorMessage">The localized error message if the email is invalid.</param>
public record EmailValidationResult(bool IsValid, string ErrorMessage = "");

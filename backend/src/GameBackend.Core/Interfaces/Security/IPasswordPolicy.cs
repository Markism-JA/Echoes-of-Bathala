namespace GameBackend.Core.Interfaces.Security;

/// <summary>
/// Defines the domain rules for password strength, complexity, and security
/// to ensure player accounts are protected against common attacks.
/// </summary>
public interface IPasswordPolicy
{
    /// <summary>
    /// Evaluates a plain-text password against the system's security requirements.
    /// </summary>
    /// <param name="password">The raw, plain-text password to be validated.</param>
    /// <param name="username">Optional. The player's username to ensure the password doesn't contain it.</param>
    /// <param name="email">Optional. The player's email to ensure the password doesn't contain it.</param>
    /// <returns>A <see cref="PasswordValidationResult"/> indicating if the password meets all criteria.</returns>
    /// <remarks>
    /// Typical validations include minimum length, character variety (uppercase, numbers, symbols),
    /// and checking against a list of common/leaked passwords.
    /// </remarks>
    public PasswordValidationResult Validate(
        string password,
        string? username = null,
        string? email = null
    );
}

/// <summary>
/// Represents the result of a password validation check.
/// </summary>
/// <param name="IsValid">True if the password satisfies all policy requirements.</param>
/// <param name="ErrorMessage">The localized description of the policy violation if <paramref name="IsValid"/> is false.</param>
public record PasswordValidationResult(bool IsValid, string ErrorMessage = "");

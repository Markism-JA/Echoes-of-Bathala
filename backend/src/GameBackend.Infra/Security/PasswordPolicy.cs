using GameBackend.Core.Interfaces.Security;

namespace GameBackend.Infra.Security;

public class PasswordPolicy : IPasswordPolicy
{
    public PasswordValidationResult Validate(
        string password,
        string? username = null,
        string? email = null
    )
    {
        if (string.IsNullOrWhiteSpace(password))
            return new PasswordValidationResult(false, "Password is required.");

        if (password.Length < 8)
            return new PasswordValidationResult(
                false,
                "Password must be at least 8 characters long."
            );

        var result = Zxcvbn.Core.EvaluatePassword(password, new[] { username, email });

        if (result.Score < 3)
        {
            return new PasswordValidationResult(false, "Password is too weak or too common.");
        }

        return new PasswordValidationResult(true);
    }
}

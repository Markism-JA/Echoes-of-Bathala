namespace GameBackend.Core.Interfaces.Security
{
    public interface IPasswordPolicy
    {
        public PasswordValidationResult Validate(
            string password,
            string? username = null,
            string? email = null
        );
    }

    public record PasswordValidationResult(bool IsValid, string ErrorMessage = "");
}

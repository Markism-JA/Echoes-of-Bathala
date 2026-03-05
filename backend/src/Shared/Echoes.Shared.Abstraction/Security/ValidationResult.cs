namespace Echoes.Shared.Abstraction.Security
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;

        public ValidationResult(bool isValid, string? errorCode = null, string? errorMessage = null)
        {
            IsValid = isValid;
            ErrorCode = errorCode ?? string.Empty;
            ErrorMessage = errorMessage ?? string.Empty;
        }
    }
}

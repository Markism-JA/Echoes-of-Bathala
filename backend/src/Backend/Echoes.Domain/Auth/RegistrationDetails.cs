namespace Echoes.Domain.Auth
{
    /// <summary>
    /// Contains the normalized identity details for a user that has passed registration validation.
    /// </summary>
    /// <param name="NormalizedUsername">The username after normalization (e.g., casing/trimming) rules are applied.</param>
    /// <param name="NormalizedEmail">The email address after normalization rules are applied.</param>
    public record RegistrationDetails(string NormalizedUsername, string NormalizedEmail);
}

namespace Echoes.Application.Auth.Models
{
    /// <summary>
    /// Represents the configuration settings for JWT authentication and session management.
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// The name of the configuration section in appsettings.json.
        /// </summary>
        public const string SectionName = "JwtSettings";

        /// <summary>
        /// Gets the secret key used to sign the JWT.
        /// Should be a long, high-entropy string.
        /// </summary>
        public string Secret { get; init; } = null!;

        /// <summary>
        /// Gets the number of minutes until an access token expires.
        /// </summary>
        public int ExpiryMinutes { get; init; }

        /// <summary>
        /// Gets the number of days until a refresh token expires.
        /// </summary>
        public int RefreshTokenExpiryDays { get; init; }

        /// <summary>
        /// Gets the 'iss' (issuer) claim value identifying the provider that issued the JWT.
        /// </summary>
        public string Issuer { get; init; } = null!;

        /// <summary>
        /// Gets the 'aud' (audience) claim value identifying the recipients that the JWT is intended for.
        /// </summary>
        public string Audience { get; init; } = null!;

        /// <summary>
        /// Gets the maximum number of concurrent active sessions allowed for a single user account.
        /// </summary>
        public int MaxCurrentSessions { get; init; }
    }
}

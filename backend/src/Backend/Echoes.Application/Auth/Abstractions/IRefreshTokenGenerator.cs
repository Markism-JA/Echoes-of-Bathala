namespace Echoes.Application.Auth.Abstractions
{
    /// <summary>
    /// Defines a contract for generating cryptographically secure refresh tokens.
    /// </summary>
    public interface IRefreshTokenGenerator
    {
        /// <summary>
        /// Generates a high-entropy, random string to be used as a refresh token.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> representing 64 bytes of cryptographically
        /// secure random data, encoded in Base64.
        /// </returns>
        public string GenerateToken();
    }
}

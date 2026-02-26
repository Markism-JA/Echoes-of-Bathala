using System.Security.Claims;

namespace GameBackend.Core.Interfaces.Security
{
    public interface IJwtTokenValidator
    {
        /// <summary>
        /// Validates a JWT string and returns the ClaimsPrincipal if successful.
        /// Returns null if the token is invalid or expired.
        /// </summary>
        public ClaimsPrincipal? ValidateToken(string token);
    }
}

using GameBackend.Core.Entities;

namespace GameBackend.Core.Interfaces.Security
{
    public interface IJwtTokenGenerator
    {
        /// <summary>
        /// Generates a JWT for a specific user.
        /// Returns the token string and the expiration timestamp.
        /// </summary>
        public (string Token, DateTime Expiration) GenerateToken(User user);
    }
}

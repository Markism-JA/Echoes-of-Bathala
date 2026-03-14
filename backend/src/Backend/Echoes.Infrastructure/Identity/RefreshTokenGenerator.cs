using System.Security.Cryptography;
using Echoes.Application.Auth.Abstractions;

namespace Echoes.Infrastructure.Identity
{
    /// <summary>
    /// Provides a concrete implementation of <see cref="IRefreshTokenGenerator"/>
    /// using the system's underlying random number generator.
    /// </summary>
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        /// <inheritdoc />
        /// <remarks>
        /// This implementation utilizes <see cref="RandomNumberGenerator.GetBytes(int)"/>
        /// to ensure tokens are non-deterministic and resistant to brute-force attacks.
        /// </remarks>
        public string GenerateToken()
        {
            // Generates 512 bits (64 bytes) of entropy
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}

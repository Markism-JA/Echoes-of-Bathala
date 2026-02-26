using System.Security.Cryptography;
using GameBackend.Core.Interfaces.Security;

namespace GameBackend.Infra.Security
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        public string GenerateToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}

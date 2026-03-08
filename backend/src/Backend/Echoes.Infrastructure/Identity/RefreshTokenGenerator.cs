using System.Security.Cryptography;
using Echoes.Application.Auth.Abstractions;

namespace Echoes.Infrastructure.Identity
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        public string GenerateToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}

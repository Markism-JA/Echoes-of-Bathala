using System.Security.Cryptography;
using System.Text;

namespace Echoes.Infrastructure.Security
{
    public class TokenHasher
    {
        public static string ComputeHash(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));

            byte[] tokenBytes = Encoding.UTF8.GetBytes(token);

            byte[] hashBytes = SHA256.HashData(tokenBytes);

            return Convert.ToHexString(hashBytes);
        }
    }
}

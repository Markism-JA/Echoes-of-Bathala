using GameBackend.Core.Entities;

namespace GameBackend.Core.Interfaces.Security
{
    public interface IPasswordHasher
    {
        /// <summary>
        /// Returns a secure, salted cryptographic hash of the plain-text password.
        /// </summary>
        public string HashPassword(User user, string password);

        /// <summary>
        /// Compares a plain-text password against a stored hash.
        /// </summary>
        public bool VerifyPassword(User user, string password, string hashedPassword);
    }
}

using GameBackend.Core.Interfaces;
using GameBackend.Shared.Enums;
using Microsoft.AspNetCore.Identity;

namespace GameBackend.Core.Entities
{
    public class User : IdentityUser<Guid>, IEntity<Guid>, IAuditable, ISoftDelete
    {
        public UserStatus Status { get; set; } = UserStatus.Unverified;
        public string? LinkedWalletAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ICollection<PlayerCharacter> Characters { get; set; } =
            new HashSet<PlayerCharacter>();

        public static User Create(
            string username,
            string email,
            string passwordHash,
            string normalizedUsername,
            string normalizedEmail,
            DateTime createdAt
        )
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required", nameof(username));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required", nameof(email));
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash cannot be empty", nameof(passwordHash));
            if (string.IsNullOrWhiteSpace(normalizedUsername))
                throw new ArgumentException(
                    "Normalized Username is required",
                    nameof(normalizedUsername)
                );
            if (string.IsNullOrWhiteSpace(normalizedEmail))
                throw new ArgumentException(
                    "Normalized Email is required",
                    nameof(normalizedEmail)
                );
            if (createdAt == default)
                throw new ArgumentException(
                    "A valid creation date must be provided.",
                    nameof(createdAt)
                );

            return new User
            {
                Id = Guid.NewGuid(),
                UserName = username,
                Email = email,
                PasswordHash = passwordHash,
                NormalizedUserName = normalizedUsername,
                NormalizedEmail = normalizedEmail,
                Status = UserStatus.Unverified,
                CreatedAt = createdAt,
            };
        }

        public void LinkWallet(string walletAddress)
        {
            if (string.IsNullOrWhiteSpace(walletAddress))
                throw new ArgumentException("Wallet address cannot be empty");

            LinkedWalletAddress = walletAddress.ToUpperInvariant();
        }

        public void SetPasswordHash(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException("Password hash cannot be empty");

            PasswordHash = hash;
        }

        public void UpdateNormalizedFields(string normalizedUserName, string normalizedEmail)
        {
            if (string.IsNullOrWhiteSpace(normalizedUserName))
                throw new ArgumentException("Normalized Username is required");
            if (string.IsNullOrWhiteSpace(normalizedEmail))
                throw new ArgumentException("Normalized Email is required");

            NormalizedUserName = normalizedUserName;
            NormalizedEmail = normalizedEmail;
        }
    }
}

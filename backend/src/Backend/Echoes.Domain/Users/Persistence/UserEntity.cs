using Echoes.Domain.Common;

namespace Echoes.Domain.Users.Persistence
{
    public class UserEntity : BaseEntity, ISoftDelete, IAggregateRoot
    {
        public string UserName { get; private set; } = null!;
        public string NormalizedUserName { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string NormalizedEmail { get; private set; } = null!;
        public UserStatus Status { get; private set; } = UserStatus.Unverified;
        public string? LinkedWalletAddress { get; private set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        private UserEntity() { }

        public static UserEntity Create(
            Guid id,
            string username,
            string normalizedUserName,
            string email,
            string normalizedEmail,
            DateTime? createdAt = null
        )
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required", nameof(username));
            if (string.IsNullOrWhiteSpace(normalizedUserName))
                throw new ArgumentException(
                    "Normalized Username is required",
                    nameof(normalizedUserName)
                );
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required", nameof(email));
            if (string.IsNullOrWhiteSpace(normalizedEmail))
                throw new ArgumentException(
                    "Normalized Email is required",
                    nameof(normalizedEmail)
                );

            return new UserEntity
            {
                Id = id,
                UserName = username,
                NormalizedUserName = normalizedUserName,
                Email = email,
                NormalizedEmail = normalizedEmail,
                Status = UserStatus.Unverified,
                CreatedAt = createdAt ?? DateTime.UtcNow,
            };
        }

        public void Delete(DateTime? deletedAt = null)
        {
            IsDeleted = true;
            DeletedAt = deletedAt ?? DateTime.UtcNow;
        }
    }
}

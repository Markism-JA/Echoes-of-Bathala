namespace Echoes.Domain.Auth
{
    public class RefreshToken
    {
        private RefreshToken() { }

        public Guid Id { get; set; }
        public string Token { get; private set; } = null!;
        public Guid UserId { get; private set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiryDate { get; private set; }
        public bool IsRevoked { get; private set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
        public bool IsActive => !IsRevoked && !IsExpired;

        public static RefreshToken Create(
            string token,
            Guid userId,
            DateTime expiryDate,
            DateTime createdAt
        )
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token is required.", nameof(token));
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId is required.", nameof(userId));

            return new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = token,
                UserId = userId,
                ExpiryDate = expiryDate,
                CreatedAt = createdAt,
                IsRevoked = false,
            };
        }

        public static RefreshToken Restore(
            Guid id,
            string token,
            Guid userId,
            DateTime expiryDate,
            bool isRevoked,
            DateTime createdAt
        )
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Cannot restore RefreshToken without an ID.");

            return new RefreshToken
            {
                Id = id,
                Token = token,
                UserId = userId,
                ExpiryDate = expiryDate,
                IsRevoked = isRevoked,
                CreatedAt = createdAt,
            };
        }

        public void Revoke()
        {
            IsRevoked = true;
        }
    }
}

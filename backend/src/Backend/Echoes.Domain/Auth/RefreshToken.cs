namespace Echoes.Domain.Auth
{
    /// <summary>
    /// Represents a long-lived credential used to acquire new access tokens.
    /// </summary>
    /// <remarks>
    /// <b>Storage Note:</b> This entity is persisted in the <b>Redis Buffer Instance</b>
    /// via <c>ISessionService</c> to ensure high-performance authentication checks
    /// and to support rapid revocation across distributed services.
    /// </remarks>
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

        /// <summary>
        /// Reconstructs an existing Refresh Token from the persistence layer (Redis).
        /// </summary>
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

        /// <summary>
        /// Permanently invalidates this session.
        /// Use this to trigger revocation logic across the infrastructure
        /// without losing the session's metadata.
        /// </summary>
        public void Revoke()
        {
            IsRevoked = true;
        }
    }
}

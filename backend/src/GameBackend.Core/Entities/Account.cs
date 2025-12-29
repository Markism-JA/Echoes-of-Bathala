using GameBackend.Shared.Enums;

namespace GameBackend.Core.Entities
{
    public class Account
    {
        // Note: Init allows for this property to be accessible only during creation.
        public required string AccountId { get; init; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string HashedPassword { get; set; }
        public AccountStatus Status { get; set; } = AccountStatus.Unverified;
        public string? LinkedWalletAddress { get; set; }
        public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.Now;
    }
}

using GameBackend.Shared.Enums;

namespace GameBackend.Core.Entities
{
    public class User
    {
        public required Guid Id { get; init; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string HashedPassword { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Unverified;
        public string? LinkedWalletAddress { get; set; }
        public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.Now;
    }
}

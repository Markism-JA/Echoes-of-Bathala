using GameBackend.Core.Interfaces;
using GameBackend.Shared.Enums;
using Microsoft.AspNetCore.Identity;

namespace GameBackend.Core.Entities
{
    public class User : IdentityUser<Guid>, IEntity<Guid>, IAuditable, ISoftDelete
    {
        public required string HashedPassword { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Unverified;
        public string? LinkedWalletAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ICollection<PlayerCharacter> Characters { get; set; }
    }
}

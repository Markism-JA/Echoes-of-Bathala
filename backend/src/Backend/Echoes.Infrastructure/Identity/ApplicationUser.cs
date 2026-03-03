using Echoes.Domain.Common;
using Echoes.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Echoes.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>, ISoftDelete
    {
        public virtual User User { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public async Task DeleteAsync(
            UserManager<ApplicationUser> userManager,
            DateTime? deletedAt = null
        )
        {
            IsDeleted = true;
            DeletedAt = deletedAt ?? DateTime.UtcNow;

            await userManager.SetLockoutEnabledAsync(this, true);
            await userManager.SetLockoutEndDateAsync(this, DateTimeOffset.MaxValue);
        }
    }
}

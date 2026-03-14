using Echoes.Domain.Common;
using Echoes.Domain.Users.Persistence;
using Microsoft.AspNetCore.Identity;

namespace Echoes.Infrastructure.Identity
{
    /// <summary>
    /// Extends the default Identity User to bridge authentication data with domain-specific user profiles.
    /// Implements <see cref="ISoftDelete"/> to ensure accounts can be deactivated without immediate data loss.
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>, ISoftDelete
    {
        /// <summary>
        /// Navigation property to the domain-specific <see cref="UserEntity"/>.
        /// This links the "Account" (credentials) to the "Player" (game data).
        /// </summary>
        public virtual UserEntity User { get; set; } = null!;

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Performs a soft-delete by flagging the record and indefinitely locking the account
        /// via the <see cref="UserManager{TUser}"/>.
        /// </summary>
        /// <param name="userManager">The ASP.NET Identity manager responsible for account state.</param>
        /// <param name="deletedAt">Optional timestamp; defaults to <see cref="DateTime.UtcNow"/>.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteAsync(
            UserManager<ApplicationUser> userManager,
            DateTime? deletedAt = null
        )
        {
            IsDeleted = true;
            DeletedAt = deletedAt ?? DateTime.UtcNow;

            // Secure the account by enabling lockout and setting it to the maximum possible date.
            await userManager.SetLockoutEnabledAsync(this, true);
            await userManager.SetLockoutEndDateAsync(this, DateTimeOffset.MaxValue);
        }
    }
}

using Echoes.Domain.Repository;
using Echoes.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Echoes.Infrastructure.Persistence.Postgresql.Repositories
{
    public class UserRepository(GameDbContext context) : Repository<User>(context), IUserRepository
    {
        public async Task<User?> GetByEmailAsync(
            string normalizedEmail,
            CancellationToken cancellationToken = default
        )
        {
            return await DbSet.FirstOrDefaultAsync(
                a => a.NormalizedEmail == normalizedEmail,
                cancellationToken
            );
        }

        public async Task<User?> GetByUserNameAsync(
            string normalizedUsername,
            CancellationToken cancellationToken = default
        )
        {
            return await DbSet.FirstOrDefaultAsync(
                a => a.NormalizedUserName == normalizedUsername,
                cancellationToken
            );
        }

        public async Task<User?> GetByWalletAddressAsync(
            string normalizedWalletAddress,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrWhiteSpace(normalizedWalletAddress))
                return null;

            return await DbSet.FirstOrDefaultAsync(
                a => a.LinkedWalletAddress == normalizedWalletAddress,
                cancellationToken
            );
        }

        public async Task<bool> IsEmailTakenAsync(
            string normalizedEmail,
            CancellationToken cancellationToken = default
        )
        {
            return await DbSet.AnyAsync(
                a => a.NormalizedEmail == normalizedEmail,
                cancellationToken
            );
        }

        public async Task<bool> IsUserNameTakenAsync(
            string normalizedUsername,
            CancellationToken cancellationToken = default
        )
        {
            return await DbSet.AnyAsync(
                a => a.NormalizedUserName == normalizedUsername,
                cancellationToken
            );
        }

        public async Task<bool> IsWalletLinkedAsync(
            string normalizedWalletAddress,
            CancellationToken cancellationToken = default
        )
        {
            return await DbSet.AnyAsync(
                a => a.LinkedWalletAddress == normalizedWalletAddress,
                cancellationToken
            );
        }
    }
}

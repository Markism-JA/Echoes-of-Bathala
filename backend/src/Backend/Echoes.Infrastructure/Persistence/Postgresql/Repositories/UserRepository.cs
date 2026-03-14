using Echoes.Domain.Users.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Echoes.Infrastructure.Persistence.Postgresql.Repositories
{
    public class UserRepository(GameDbContext context) : Repository<UserEntity>(context), IUserRepository
    {
        public async Task<UserEntity?> GetByEmailAsync(
            string normalizedEmail,
            CancellationToken cancellationToken = default
        )
        {
            return await DbSet.FirstOrDefaultAsync(
                a => a.NormalizedEmail == normalizedEmail,
                cancellationToken
            );
        }

        public async Task<UserEntity?> GetByUserNameAsync(
            string normalizedUsername,
            CancellationToken cancellationToken = default
        )
        {
            return await DbSet.FirstOrDefaultAsync(
                a => a.NormalizedUserName == normalizedUsername,
                cancellationToken
            );
        }

        public async Task<UserEntity?> GetByWalletAddressAsync(
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

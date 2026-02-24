using GameBackend.Core.Entities;
using GameBackend.Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace GameBackend.Infra.Persistence.Repositories
{
    public class UserRepository(GameDbContext context)
        : Repository<User, Guid>(context),
            IUserRepository
    {
        public async Task<User?> GetByEmailAsync(
            string email,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            return await _dbSet.FirstOrDefaultAsync(
                a => a.Email != null && a.Email.ToLower() == email.ToLower(),
                cancellationToken
            );
        }

        public async Task<User?> GetByUserNameAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;
            return await _dbSet.FirstOrDefaultAsync(
                a => a.UserName != null && a.UserName.ToLower() == username.ToLower(),
                cancellationToken
            );
        }

        public async Task<User?> GetByWalletAddressAsync(
            string walletAddress,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrWhiteSpace(walletAddress))
                return null;
            return await _dbSet.FirstOrDefaultAsync(
                a =>
                    a.LinkedWalletAddress != null
                    && a.LinkedWalletAddress.ToLower() == walletAddress.ToLower(),
                cancellationToken
            );
        }

        public async Task<bool> IsEmailTakenAsync(
            string email,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet.AnyAsync(a => a.Email == email, cancellationToken);
        }

        public async Task<bool> IsUserNameTakenAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet.AnyAsync(a => a.UserName == username, cancellationToken);
        }

        public async Task<bool> IsWalletLinkedAsync(
            string walletAddress,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet.AnyAsync(
                a => a.LinkedWalletAddress == walletAddress,
                cancellationToken
            );
        }
    }
}

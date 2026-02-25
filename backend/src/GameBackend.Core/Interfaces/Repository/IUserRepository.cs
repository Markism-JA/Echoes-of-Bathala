using GameBackend.Core.Entities;

namespace GameBackend.Core.Interfaces.Repository
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        public Task<User?> GetByEmailAsync(
            string normalizedEmail,
            CancellationToken cancellationToken = default
        );
        public Task<User?> GetByUserNameAsync(
            string normalizedUsername,
            CancellationToken cancellationToken = default
        );
        public Task<User?> GetByWalletAddressAsync(
            string walletAddress,
            CancellationToken cancellationToken = default
        );

        public Task<bool> IsEmailTakenAsync(
            string normalizedEmail,
            CancellationToken cancellationToken = default
        );
        public Task<bool> IsUserNameTakenAsync(
            string normalizedUsername,
            CancellationToken cancellationToken = default
        );
        public Task<bool> IsWalletLinkedAsync(
            string walletAddress,
            CancellationToken cancellationToken = default
        );
    }
}

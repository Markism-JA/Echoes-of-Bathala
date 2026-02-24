using GameBackend.Core.Entities;

namespace GameBackend.Core.Interfaces.Repository
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<User?> GetByUserNameAsync(
            string username,
            CancellationToken cancellationToken = default
        );
        Task<User?> GetByWalletAddressAsync(
            string walletAddress,
            CancellationToken cancellationToken = default
        );

        Task<bool> IsEmailTakenAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> IsUserNameTakenAsync(
            string username,
            CancellationToken cancellationToken = default
        );
        Task<bool> IsWalletLinkedAsync(
            string walletAddress,
            CancellationToken cancellationToken = default
        );
    }
}

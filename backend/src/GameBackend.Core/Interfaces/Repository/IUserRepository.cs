using GameBackend.Core.Entities;

namespace GameBackend.Core.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid accountId);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUserNameAsync(string username);
        Task<User?> GetByWalletAddressAsync(string walletAddress);

        Task<bool> IsEmailTakenAsync(string email);
        Task<bool> IsUserNameTakenAsync(string username);
        Task<bool> IsWalletLinkedAsync(string walletAddress);

        Task CreateUserAsync(User account);

        Task UpdateUserAsync(User account);

        Task DeleteUserAsync(User account);
    }
}

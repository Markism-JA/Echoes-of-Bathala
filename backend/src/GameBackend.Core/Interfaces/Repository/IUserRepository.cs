using GameBackend.Core.Entities;

namespace GameBackend.Core.Interfaces.Repository
{
    public interface IUserRepository
    {
        // -- Reads
        Task<User?> GetByIdAsync(Guid accountId);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUserNameAsync(string username);
        Task<User?> GetByWalletAddressAsync(string walletAddress);

        // Fast Checks
        Task<bool> IsEmailTakenAsync(string email);
        Task<bool> IsUserNameTakenAsync(string username);
        Task<bool> IsWalletLinkedAsync(string walletAddress);

        // -- Create
        Task CreateUserAsync(User account);

        // -- Update
        Task UpdateUserAsync(User account);

        // -- Delete
        Task DeleteUserAsync(User account);
    }
}

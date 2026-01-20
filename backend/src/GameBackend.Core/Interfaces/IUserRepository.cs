using GameBackend.Core.Entities;

namespace GameBackend.Core.Interfaces
{
    public interface IUserRepository
    {
        // -- Reads
        Task<User?> GetByIdAsync(Guid accountId);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByWalletAddressAsync(string walletAddress);

        // Fast Checks
        Task<bool> IsEmailTakenAsync(string email);
        Task<bool> IsUsernameTakenAsync(string username);
        Task<bool> IsWalletLinkedAsync(string walletAddress);

        // -- Create
        Task CreateUserAsync(User account);

        // -- Update
        Task UpdateUserAsync(User account);

        // -- Delete
        Task DeleteUserAsync(User account);
    }
}

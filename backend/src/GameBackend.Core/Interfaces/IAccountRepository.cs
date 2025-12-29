using GameBackend.Core.Entities;

namespace GameBackend.Core.Interfaces
{
    public interface IAccountRepository
    {
        // -- Reads
        Task<Account?> GetByIdAsync(Guid accountId);
        Task<Account?> GetByEmailAsync(string email);
        Task<Account?> GetByUsernameAsync(string username);
        Task<Account?> GetByWalletAddressAsync(string walletAddress);

        // Fast Checks
        Task<bool> IsEmailTakenAsync(string email);
        Task<bool> IsUsernameTakenAsync(string username);
        Task<bool> IsWalletLinkedAsync(string walletAddress);

        // -- Create
        Task CreateAccountAsync(Account account);

        // -- Update
        Task UpdateAccountAsync(Account account);

        // -- Delete
        Task DeleteAccountAsync(Account account);
    }
}

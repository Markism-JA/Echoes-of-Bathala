using GameBackend.Core.Entities;
using GameBackend.Core.Interfaces;
using GameBackend.Infra.Persistence;

namespace GameBackend.Infra
{
    public class AccountRepository(GameDbContext context) : IAccountRepository
    {
        private readonly GameDbContext _context = context;

        public Task CreateAccountAsync(Account account)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAccountAsync(Account account)
        {
            throw new NotImplementedException();
        }

        public Task<Account?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Account?> GetByIdAsync(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task<Account?> GetByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<Account?> GetByWalletAddressAsync(string walletAddress)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsEmailTakenAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUsernameTakenAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsWalletLinkedAsync(string walletAddress)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAccountAsync(Account account)
        {
            throw new NotImplementedException();
        }
    }
}

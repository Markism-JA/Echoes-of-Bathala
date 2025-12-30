using GameBackend.Core.Entities;
using GameBackend.Core.Interfaces;
using GameBackend.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GameBackend.Infra
{
    public class AccountRepository(GameDbContext context) : IAccountRepository
    {
        private readonly GameDbContext _context = context;

        public async Task CreateAccountAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(Account account)
        {
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
        }

        public async Task<Account?> GetByEmailAsync(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<Account?> GetByIdAsync(Guid accountId)
        {
            return await _context.Accounts.FindAsync(accountId);
        }

        public async Task<Account?> GetByUsernameAsync(string username)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Username == username);
        }

        public async Task<Account?> GetByWalletAddressAsync(string walletAddress)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a =>
                a.LinkedWalletAddress == walletAddress
            );
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            return await _context.Accounts.AnyAsync(a => a.Email == email);
        }

        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            return await _context.Accounts.AnyAsync(a => a.Username == username);
        }

        public async Task<bool> IsWalletLinkedAsync(string walletAddress)
        {
            return await _context.Accounts.AnyAsync(a => a.LinkedWalletAddress == walletAddress);
        }

        public async Task UpdateAccountAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }
    }
}

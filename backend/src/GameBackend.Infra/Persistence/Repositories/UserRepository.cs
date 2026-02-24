using GameBackend.Core.Entities;
using GameBackend.Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace GameBackend.Infra.Persistence.Repositories
{
    public class UserRepository(GameDbContext context) : IUserRepository
    {
        private readonly GameDbContext _context = context;

        public async Task CreateUserAsync(User account)
        {
            await _context.Users.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User account)
        {
            _context.Users.Remove(account);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid accountId)
        {
            return await _context.Users.FindAsync(accountId);
        }

        public async Task<User?> GetByUserNameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(a => a.UserName == username);
        }

        public Task<User?> GetByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetByWalletAddressAsync(string walletAddress)
        {
            return await _context.Users.FirstOrDefaultAsync(a =>
                a.LinkedWalletAddress == walletAddress
            );
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            return await _context.Users.AnyAsync(a => a.Email == email);
        }

        public async Task<bool> IsUserNameTakenAsync(string username)
        {
            return await _context.Users.AnyAsync(a => a.UserName == username);
        }

        public Task<bool> IsUsernameTakenAsync(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsWalletLinkedAsync(string walletAddress)
        {
            return await _context.Users.AnyAsync(a => a.LinkedWalletAddress == walletAddress);
        }

        public async Task UpdateUserAsync(User account)
        {
            _context.Users.Update(account);
            await _context.SaveChangesAsync();
        }
    }
}

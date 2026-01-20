using GameBackend.Core.Entities;
using GameBackend.Infra;
using GameBackend.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GameBackend.Tests.GameBackend.IntegrationTests.Infra
{
    public class UserRepositoryTests
    {
        private static DbContextOptions<GameDbContext> GetOptions(string dbName)
        {
            return new DbContextOptionsBuilder<GameDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        private static User CreateTestUser(string suffix = "1")
        {
            return new User
            {
                UserId = Guid.NewGuid(),
                Username = $"User{suffix}",
                Email = $"user{suffix}@example.com",
                HashedPassword = $"Hash{suffix}",
                LinkedWalletAddress = $"0xWallet{suffix}",
            };
        }

        [Fact]
        public async Task CreateUserAsync_AddsUserToDatabase()
        {
            var options = GetOptions("CreateUser_Db");
            var newUser = CreateTestUser();

            // Act
            using (var context = new GameDbContext(options))
            {
                var repository = new UserRepository(context);
                await repository.CreateUserAsync(newUser);
            }

            // Assert
            using (var context = new GameDbContext(options))
            {
                var accountInDb = await context.Users.FirstOrDefaultAsync();
                Assert.NotNull(accountInDb);
                Assert.Equal(newUser.Email, accountInDb.Email);
            }
        }

        [Fact]
        public async Task DeleteUserAsync_RemovesUserFromDatabase()
        {
            var options = GetOptions("DeleteUser_Db");
            var account = CreateTestUser();

            // Arrange: Seed
            using (var context = new GameDbContext(options))
            {
                context.Users.Add(account);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new GameDbContext(options))
            {
                var repository = new UserRepository(context);

                var accToDelete = await context.Users.FirstAsync();
                await repository.DeleteUserAsync(accToDelete);
            }

            // Assert
            using (var context = new GameDbContext(options))
            {
                Assert.Empty(context.Users);
            }
        }

        [Fact]
        public async Task GetByEmailAsync_ReturnsCorrectUser()
        {
            var options = GetOptions("GetByEmail_Db");
            var account = CreateTestUser();

            using (var context = new GameDbContext(options))
            {
                context.Users.Add(account);
                await context.SaveChangesAsync();
            }

            using (var context = new GameDbContext(options))
            {
                var repository = new UserRepository(context);
                var result = await repository.GetByEmailAsync(account.Email);

                Assert.NotNull(result);
                Assert.Equal(account.Username, result.Username);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectUser()
        {
            var options = GetOptions("GetById_Db");
            var account = CreateTestUser();

            using (var context = new GameDbContext(options))
            {
                context.Users.Add(account);
                await context.SaveChangesAsync();
            }

            using (var context = new GameDbContext(options))
            {
                var repository = new UserRepository(context);
                var result = await repository.GetByIdAsync(account.UserId);

                Assert.NotNull(result);
                Assert.Equal(account.Email, result.Email);
            }
        }

        [Fact]
        public async Task GetByUsernameAsync_ReturnsCorrectUser()
        {
            var options = GetOptions("GetByUsername_Db");
            var account = CreateTestUser();

            using (var context = new GameDbContext(options))
            {
                context.Users.Add(account);
                await context.SaveChangesAsync();
            }

            using (var context = new GameDbContext(options))
            {
                var repository = new UserRepository(context);
                var result = await repository.GetByUsernameAsync(account.Username);

                Assert.NotNull(result);
                Assert.Equal(account.UserId, result.UserId);
            }
        }

        [Fact]
        public async Task GetByWalletAddressAsync_ReturnsCorrectUser()
        {
            var options = GetOptions("GetByWallet_Db");
            var account = CreateTestUser();

            using (var context = new GameDbContext(options))
            {
                context.Users.Add(account);
                await context.SaveChangesAsync();
            }

            using (var context = new GameDbContext(options))
            {
                var repository = new UserRepository(context);
                var result = await repository.GetByWalletAddressAsync(account.LinkedWalletAddress!);

                Assert.NotNull(result);
                Assert.Equal(account.Username, result.Username);
            }
        }

        [Fact]
        public async Task IsEmailTakenAsync_ReturnsTrue_WhenEmailExists()
        {
            var options = GetOptions("IsEmailTaken_Db");
            var account = CreateTestUser();

            using (var context = new GameDbContext(options))
            {
                context.Users.Add(account);
                await context.SaveChangesAsync();
            }

            using (var context = new GameDbContext(options))
            {
                var repository = new UserRepository(context);

                // Check existing
                var exists = await repository.IsEmailTakenAsync(account.Email);
                Assert.True(exists);

                // Check non-existing
                var doesNotExist = await repository.IsEmailTakenAsync("fake@nothing.com");
                Assert.False(doesNotExist);
            }
        }

        [Fact]
        public async Task IsUsernameTakenAsync_ReturnsTrue_WhenUsernameExists()
        {
            var options = GetOptions("IsUsernameTaken_Db");
            var account = CreateTestUser();

            using (var context = new GameDbContext(options))
            {
                context.Users.Add(account);
                await context.SaveChangesAsync();
            }

            using (var context = new GameDbContext(options))
            {
                var repository = new UserRepository(context);

                Assert.True(await repository.IsUsernameTakenAsync(account.Username));
                Assert.False(await repository.IsUsernameTakenAsync("NonExistentUser"));
            }
        }

        [Fact]
        public async Task IsWalletLinkedAsync_ReturnsTrue_WhenWalletExists()
        {
            var options = GetOptions("IsWalletLinked_Db");
            var account = CreateTestUser();

            using (var context = new GameDbContext(options))
            {
                context.Users.Add(account);
                await context.SaveChangesAsync();
            }

            using (var context = new GameDbContext(options))
            {
                var repository = new UserRepository(context);

                Assert.True(await repository.IsWalletLinkedAsync(account.LinkedWalletAddress!));
                Assert.False(await repository.IsWalletLinkedAsync("0x999999999"));
            }
        }

        [Fact]
        public async Task UpdateUserAsync_PersistsChanges()
        {
            var options = GetOptions("UpdateUser_Db");
            var account = CreateTestUser();

            // Arrange
            using (var context = new GameDbContext(options))
            {
                context.Users.Add(account);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new GameDbContext(options))
            {
                var repository = new UserRepository(context);

                // Fetch, Modify, Update
                var fetchedUser = await repository.GetByIdAsync(account.UserId);
                fetchedUser!.Username = "UpdatedUser";

                await repository.UpdateUserAsync(fetchedUser);
            }

            // Assert
            using (var context = new GameDbContext(options))
            {
                var updatedUser = await context.Users.FindAsync(account.UserId);
                Assert.Equal("UpdatedUser", updatedUser!.Username);
            }
        }
    }
}

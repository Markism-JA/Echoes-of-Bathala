using FluentAssertions;
using GameBackend.Core.Entities;

namespace GameBackend.Infra.Tests.Repositories;

public class UserRepositoryTests(DatabaseFixture fixture) : BaseIntegrationTest(fixture)
{
    [Fact]
    public async Task IsEmailTakenAsync_ShouldReturnTrue_WhenEmailExists()
    {
        var testEmail = $"{GetShortUnique("test")}@example.com";
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = GetShortUnique("player"),
            Email = testEmail,
            HashedPassword = "Pass123",
        };

        await UserRepository.AddAsync(user);
        await Context.SaveChangesAsync();

        var result = await UserRepository.IsEmailTakenAsync(testEmail);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task AddAsync_ShouldThrowException_WhenUserNameIsNotUnique()
    {
        var duplicateUserName = GetShortUnique("dup");

        var user1 = new User
        {
            Id = Guid.NewGuid(),
            UserName = duplicateUserName,
            Email = $"{GetShortUnique("e1")}@example.com",
            HashedPassword = "Pass123",
        };

        var user2 = new User
        {
            Id = Guid.NewGuid(),
            UserName = duplicateUserName,
            Email = $"{GetShortUnique("e2")}@example.com",
            HashedPassword = "Pass123",
        };

        await UserRepository.AddAsync(user1);
        await Context.SaveChangesAsync();

        await UserRepository.AddAsync(user2);

        await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateException>(() =>
            Context.SaveChangesAsync()
        );
    }
}

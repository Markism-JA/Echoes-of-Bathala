using FluentAssertions;
using GameBackend.Core.Entities;
using GameBackend.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace GameBackend.Infra.Tests.Repositories;

public class UserRepositoryTests(DatabaseFixture fixture) : BaseIntegrationTest(fixture)
{
    private User CreateValidTestUser(string? walletAddress = null)
    {
        var rawName = GetShortUnique("usr");
        var rawEmail = $"{GetShortUnique("email")}@example.com";

        return new User
        {
            Id = Guid.NewGuid(),
            UserName = rawName,
            NormalizedUserName = rawName.ToUpperInvariant(),
            Email = rawEmail,
            NormalizedEmail = rawEmail.ToUpperInvariant(),
            PasswordHash = "Pass123",
            LinkedWalletAddress = walletAddress?.ToLowerInvariant(),
        };
    }

    #region Category 1: Configuration & Constraints (Postgres Schema Verification)

    [Fact]
    public async Task AddAsync_ShouldThrowException_WhenUserNameIsNotUnique()
    {
        var duplicateName = GetShortUnique("dup");

        var user1 = CreateValidTestUser();
        user1.UserName = duplicateName;
        user1.NormalizedUserName = duplicateName.ToUpperInvariant();

        var user2 = CreateValidTestUser();
        user2.UserName = duplicateName;
        user2.NormalizedUserName = duplicateName.ToUpperInvariant();

        await UserRepository.AddAsync(user1);
        await Context.SaveChangesAsync();

        await UserRepository.AddAsync(user2);

        await Assert.ThrowsAsync<DbUpdateException>(() => Context.SaveChangesAsync());
    }

    [Fact]
    public async Task AddAsync_ShouldThrowException_WhenNormalizedEmailIsNotUnique()
    {
        var duplicateEmail = $"{GetShortUnique("dup")}@example.com";
        var normalizedEmail = duplicateEmail.ToUpperInvariant();

        var user1 = CreateValidTestUser();
        user1.Email = duplicateEmail;
        user1.NormalizedEmail = normalizedEmail;

        var user2 = CreateValidTestUser();
        user2.Email = duplicateEmail;
        user2.NormalizedEmail = normalizedEmail;

        await UserRepository.AddAsync(user1);
        await Context.SaveChangesAsync();

        await UserRepository.AddAsync(user2);

        await Assert.ThrowsAsync<DbUpdateException>(() => Context.SaveChangesAsync());
    }

    [Fact]
    public async Task AddAsync_ShouldThrowException_WhenLinkedWalletAddressIsNotUnique()
    {
        var duplicateWallet = $"0x{GetShortUnique("wallet")}";

        var user1 = CreateValidTestUser(duplicateWallet);
        var user2 = CreateValidTestUser(duplicateWallet);

        await UserRepository.AddAsync(user1);
        await Context.SaveChangesAsync();

        await UserRepository.AddAsync(user2);

        await Assert.ThrowsAsync<DbUpdateException>(() => Context.SaveChangesAsync());
    }

    [Fact]
    public async Task AddAsync_ShouldAllowMultipleUsers_WhenLinkedWalletAddressIsNull()
    {
        var user1 = CreateValidTestUser(walletAddress: null);
        var user2 = CreateValidTestUser(walletAddress: null);

        await UserRepository.AddAsync(user1);
        await UserRepository.AddAsync(user2);

        var exception = await Record.ExceptionAsync(() => Context.SaveChangesAsync());
        exception.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldApplyDefaultValues_WhenUserIsCreated()
    {
        var user = CreateValidTestUser();
        user.Status = default;
        user.CreatedAt = default;

        await UserRepository.AddAsync(user);
        await Context.SaveChangesAsync();

        var dbUser = await Context.Users.FindAsync(user.Id);

        dbUser.Should().NotBeNull();
        dbUser!.Status.Should().Be(UserStatus.Unverified);

        dbUser.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    #endregion

    #region Category 2: Repository Queries Logic

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUser_WhenNormalizedEmailMatches()
    {
        var user = CreateValidTestUser();
        await UserRepository.AddAsync(user);
        await Context.SaveChangesAsync();

        var result = await UserRepository.GetByEmailAsync(user.NormalizedEmail!);

        result.Should().NotBeNull();
        result!.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        var result = await UserRepository.GetByEmailAsync("FAKE@EXAMPLE.COM");
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByUserNameAsync_ShouldReturnUser_WhenNormalizedUserNameMatches()
    {
        var user = CreateValidTestUser();
        await UserRepository.AddAsync(user);
        await Context.SaveChangesAsync();

        var result = await UserRepository.GetByUserNameAsync(user.NormalizedUserName!);

        result.Should().NotBeNull();
        result!.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task GetByWalletAddressAsync_ShouldReturnUser_WhenNormalizedWalletMatches()
    {
        var wallet = "0xabc123456";
        var user = CreateValidTestUser(wallet);

        await UserRepository.AddAsync(user);
        await Context.SaveChangesAsync();

        var result = await UserRepository.GetByWalletAddressAsync(wallet);

        result.Should().NotBeNull();
        result!.Id.Should().Be(user.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task GetByWalletAddressAsync_ShouldReturnNull_WhenInputIsNullOrWhitespace(
        string invalidWallet
    )
    {
        var result = await UserRepository.GetByWalletAddressAsync(invalidWallet);
        result.Should().BeNull();
    }

    [Fact]
    public async Task IsEmailTakenAsync_ShouldReturnTrue_WhenEmailExists()
    {
        var user = CreateValidTestUser();
        await UserRepository.AddAsync(user);
        await Context.SaveChangesAsync();

        var result = await UserRepository.IsEmailTakenAsync(user.NormalizedEmail!);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsEmailTakenAsync_ShouldReturnFalse_WhenEmailIsAvailable()
    {
        var result = await UserRepository.IsEmailTakenAsync("AVAILABLE@EXAMPLE.COM");
        result.Should().BeFalse();
    }

    [Fact]
    public async Task IsWalletLinkedAsync_ShouldReturnTrue_WhenWalletExists()
    {
        var wallet = "0x111222333";
        var user = CreateValidTestUser(wallet);

        await UserRepository.AddAsync(user);
        await Context.SaveChangesAsync();

        var result = await UserRepository.IsWalletLinkedAsync(wallet);

        result.Should().BeTrue();
    }

    #endregion
}

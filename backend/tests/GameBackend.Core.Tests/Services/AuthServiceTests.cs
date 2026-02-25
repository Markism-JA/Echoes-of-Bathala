using FluentAssertions;
using GameBackend.Core.Interfaces.Repository;
using GameBackend.Core.Interfaces.Security;
using GameBackend.Core.Services;
using GameBackend.Shared.DTOs.Identity;
using NSubstitute;

namespace GameBackend.Core.Tests.Services
{
    public class AuthServiceTest
    {
        private readonly IUserRepository _userRepositoryMock;
        private readonly IPasswordHasher _passwordHasherMock;
        private readonly IUsernamePolicy _usernamePolicyMock;
        private readonly AuthService _sut;

        public AuthServiceTest()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _passwordHasherMock = Substitute.For<IPasswordHasher>();
            _usernamePolicyMock = Substitute.For<IUsernamePolicy>();

            _sut = new AuthService(_userRepositoryMock, _passwordHasherMock, _usernamePolicyMock);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowException_WhenEmailIsTaken()
        {
            var request = new RegisterRequestDto(
                "NewPlayer",
                "testPlayer@example.com",
                "Password123",
                "Password123"
            );

            _usernamePolicyMock
                .IsAllowedAsync(request.Username, Arg.Any<CancellationToken>())
                .Returns(true);

            _userRepositoryMock
                .IsEmailTakenAsync(request.Email, Arg.Any<CancellationToken>())
                .Returns(true);

            var action = async () => await _sut.RegisterAsync(request);

            await action
                .Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Email is already taken.");

            await _userRepositoryMock.DidNotReceiveWithAnyArgs().AddAsync(default!);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowException_WhenUsernameIsTaken()
        {
            var request = new RegisterRequestDto(
                "NewPlayer",
                "testPlayer@example.com",
                "Password123",
                "Password123"
            );

            _usernamePolicyMock
                .IsAllowedAsync(request.Username, Arg.Any<CancellationToken>())
                .Returns(true);

            _userRepositoryMock
                .IsUserNameTakenAsync(request.Username, Arg.Any<CancellationToken>())
                .Returns(true);

            var action = async () => await _sut.RegisterAsync(request);

            await action
                .Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Username is already taken.");

            await _userRepositoryMock.DidNotReceiveWithAnyArgs().AddAsync(default!);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowException_WhenUsernameIsProfaneOrReserved()
        {
            var request = new RegisterRequestDto(
                "NewPlayer",
                "testPlayer@example.com",
                "Password123",
                "Password123"
            );

            _usernamePolicyMock.IsAllowedAsync(request.Username).Returns(false);

            var action = async () => await _sut.RegisterAsync(request);

            await action
                .Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Username contains forbidden words or is reserved.");

            await _userRepositoryMock.DidNotReceiveWithAnyArgs().IsUserNameTakenAsync(default!);
        }
    }
}

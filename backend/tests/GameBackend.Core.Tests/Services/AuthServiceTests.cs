using FluentAssertions;
using GameBackend.Core.Interfaces.Repository;
using GameBackend.Core.Interfaces.Security;
using GameBackend.Core.Services;
using GameBackend.Shared.DTOs.Identity;
using GameBackend.Shared.Errors;
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
        public async Task RegisterAsync_ShouldReturnConflict_WhenEmailIsTaken()
        {
            var request = CreateDefaultRequest();

            _usernamePolicyMock
                .IsAllowedAsync(request.Username, Arg.Any<CancellationToken>())
                .Returns(true);

            _userRepositoryMock
                .IsEmailTakenAsync(request.Email, Arg.Any<CancellationToken>())
                .Returns(true);

            var result = await _sut.RegisterAsync(request);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(GameErrors.Auth.EmailTaken);

            await _userRepositoryMock.DidNotReceiveWithAnyArgs().AddAsync(default!);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnConflict_WhenUsernameIsTaken()
        {
            var request = CreateDefaultRequest();
            var normalized = "newplayer";

            _usernamePolicyMock
                .IsAllowedAsync(request.Username, Arg.Any<CancellationToken>())
                .Returns(true);
            _usernamePolicyMock.Normalize(request.Username).Returns(normalized);

            _userRepositoryMock
                .IsEmailTakenAsync(request.Email, Arg.Any<CancellationToken>())
                .Returns(false);
            _userRepositoryMock
                .IsUserNameTakenAsync(normalized, Arg.Any<CancellationToken>())
                .Returns(true);

            var result = await _sut.RegisterAsync(request);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(GameErrors.Auth.UsernameTaken);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnValidationError_WhenUsernameIsProfaneOrReserved()
        {
            var request = CreateDefaultRequest();

            _usernamePolicyMock
                .IsAllowedAsync(request.Username, Arg.Any<CancellationToken>())
                .Returns(false);

            var result = await _sut.RegisterAsync(request);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(GameErrors.Auth.ProfaneUsername);

            await _userRepositoryMock
                .DidNotReceiveWithAnyArgs()
                .IsEmailTakenAsync(default!, default);
        }

        private static RegisterRequestDto CreateDefaultRequest() =>
            new("NewPlayer", "testPlayer@example.com", "Password123", "Password123");
    }
}

using ErrorOr;
using FluentAssertions;
using GameBackend.Core.Entities;
using GameBackend.Core.Interfaces.Persistence;
using GameBackend.Core.Interfaces.Repository;
using GameBackend.Core.Interfaces.Security;
using GameBackend.Core.Interfaces.Services;
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
        private readonly IEmailPolicy _emailPolicyMock;
        private readonly IPasswordPolicy _passwordPolicyMock;
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IJwtTokenGenerator _jwtTokenGeneratorMock;
        private readonly IDateTimeProvider _dateTimeProviderMock;
        private readonly IRefreshTokenRepository _refreshTokenRepositoryMock;
        private readonly IRefreshTokenGenerator _refreshTokenGeneratorMock;
        private readonly AuthService _sut;

        public AuthServiceTest()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _passwordHasherMock = Substitute.For<IPasswordHasher>();
            _usernamePolicyMock = Substitute.For<IUsernamePolicy>();
            _emailPolicyMock = Substitute.For<IEmailPolicy>();
            _passwordPolicyMock = Substitute.For<IPasswordPolicy>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _jwtTokenGeneratorMock = Substitute.For<IJwtTokenGenerator>();
            _dateTimeProviderMock = Substitute.For<IDateTimeProvider>();
            _refreshTokenRepositoryMock = Substitute.For<IRefreshTokenRepository>();
            _refreshTokenGeneratorMock = Substitute.For<IRefreshTokenGenerator>();

            _sut = new AuthService(
                _userRepositoryMock,
                _passwordHasherMock,
                _usernamePolicyMock,
                _emailPolicyMock,
                _passwordPolicyMock,
                _unitOfWorkMock,
                _jwtTokenGeneratorMock,
                _dateTimeProviderMock,
                _refreshTokenRepositoryMock,
                _refreshTokenGeneratorMock
            );
        }

        [Theory]
        [InlineData("Username is required.", "Auth.Username.Required")]
        [InlineData("This username is reserved for system use.", "Auth.Username.Reserved")]
        [InlineData("Username contains forbidden language.", "Auth.Username.Profane")]
        [InlineData("Some random unknown error.", "Auth.Username.Invalid")]
        public async Task RegisterAsync_ShouldReturnValidationError_WhenUsernamePolicyFails(
            string policyMessage,
            string expectedErrorCode
        )
        {
            var request = CreateDefaultRequest();

            _usernamePolicyMock
                .IsAllowedAsync(request.Username, Arg.Any<CancellationToken>())
                .Returns(new UsernameValidationResult(false, policyMessage));

            var result = await _sut.RegisterAsync(request);

            result.IsError.Should().BeTrue();
            result.FirstError.Code.Should().Be(expectedErrorCode);
            result.FirstError.Type.Should().Be(ErrorType.Validation);

            await _emailPolicyMock.DidNotReceiveWithAnyArgs().ValidateAsync(default!, default);
            await _userRepositoryMock
                .DidNotReceiveWithAnyArgs()
                .IsEmailTakenAsync(default!, default);
        }

        [Theory]
        [InlineData("Email is required.", "Auth.Email.Required")]
        [InlineData("Disposable email addresses are not allowed.", "Auth.Email.Disposable")]
        [InlineData("Invalid email format.", "Auth.Email.Invalid")]
        [InlineData("Unexpected email error.", "Auth.Email.Invalid")]
        public async Task RegisterAsync_ShouldReturnValidationError_WhenEmailPolicyFails(
            string policyMessage,
            string expectedErrorCode
        )
        {
            var request = CreateDefaultRequest();

            _usernamePolicyMock
                .IsAllowedAsync(request.Username, Arg.Any<CancellationToken>())
                .Returns(new UsernameValidationResult(true));

            _emailPolicyMock
                .ValidateAsync(request.Email, Arg.Any<CancellationToken>())
                .Returns(new EmailValidationResult(false, policyMessage));

            var result = await _sut.RegisterAsync(request);

            result.IsError.Should().BeTrue();
            result.FirstError.Code.Should().Be(expectedErrorCode);
            result.FirstError.Type.Should().Be(ErrorType.Validation);

            await _userRepositoryMock
                .DidNotReceiveWithAnyArgs()
                .IsEmailTakenAsync(default!, default);
        }

        [Theory]
        [InlineData("Password is required.", "Auth.Password.Required")]
        [InlineData("Password must be at least 8 characters long.", "Auth.Password.TooShort")]
        [InlineData("This is a top 100 common password.", "Auth.Password.TooWeak")]
        [InlineData("Unexpected security flaw.", "Auth.Password.TooWeak")]
        public async Task RegisterAsync_ShouldReturnValidationError_WhenPasswordPolicyFails(
            string policyMessage,
            string expectedErrorCode
        )
        {
            var request = CreateDefaultRequest();

            _usernamePolicyMock
                .IsAllowedAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(new UsernameValidationResult(true));
            _emailPolicyMock
                .ValidateAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(new EmailValidationResult(true));

            _passwordPolicyMock
                .Validate(request.Password, request.Username, request.Email)
                .Returns(new PasswordValidationResult(false, policyMessage));

            var result = await _sut.RegisterAsync(request);

            result.IsError.Should().BeTrue();
            result.FirstError.Code.Should().Be(expectedErrorCode);

            await _userRepositoryMock
                .DidNotReceiveWithAnyArgs()
                .IsEmailTakenAsync(default!, default);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnConflict_WhenEmailIsTaken()
        {
            var request = CreateDefaultRequest();
            SetupHappyPathPolicies(request);

            _userRepositoryMock
                .IsEmailTakenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
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
            SetupHappyPathPolicies(request);

            _userRepositoryMock
                .IsEmailTakenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(false);

            _userRepositoryMock
                .IsUserNameTakenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(true);

            var result = await _sut.RegisterAsync(request);

            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(GameErrors.Auth.UsernameTaken);
        }

        [Fact]
        public async Task RegisterAsync_ShouldProceedToSuccessFlow_WhenAllChecksPass()
        {
            var request = CreateDefaultRequest();
            SetupHappyPathPolicies(request);

            _userRepositoryMock
                .IsEmailTakenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(false);
            _userRepositoryMock
                .IsUserNameTakenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(false);

            var expectedHash = "secure_hashed_password_123";
            _passwordHasherMock
                .HashPassword(Arg.Any<User>(), Arg.Any<string>())
                .Returns(expectedHash);

            var fixedTime = new DateTime(2026, 2, 26, 12, 0, 0, DateTimeKind.Utc);
            _dateTimeProviderMock.UtcNow.Returns(fixedTime);

            var expectedRefreshToken = "mock_refresh_token_abc_123";
            _refreshTokenGeneratorMock.GenerateToken().Returns(expectedRefreshToken);

            var expectedAccessToken = "mock_jwt_token";
            var expectedExpiry = fixedTime.AddMinutes(60);
            _jwtTokenGeneratorMock
                .GenerateToken(Arg.Any<User>())
                .Returns((expectedAccessToken, expectedExpiry));

            var result = await _sut.RegisterAsync(request);

            result.IsError.Should().BeFalse();
            result.Value.AccessToken.Should().Be(expectedAccessToken);
            result.Value.RefreshToken.Should().Be(expectedRefreshToken);
            result.Value.User.Username.Should().Be(request.Username);
        }

        [Fact]
        public async Task RegisterAsync_ShouldNotCheckEmail_WhenUsernameIsInvalid()
        {
            var request = CreateDefaultRequest();
            _usernamePolicyMock
                .IsAllowedAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(new UsernameValidationResult(false, "Username is required."));

            await _sut.RegisterAsync(request);

            await _emailPolicyMock.DidNotReceiveWithAnyArgs().ValidateAsync(default!, default);
            _passwordPolicyMock.DidNotReceiveWithAnyArgs().Validate(default!, default!, default!);
        }

        [Fact]
        public async Task RegisterAsync_ShouldPersistUserAndRefreshToken_WhenAllChecksPass()
        {
            var request = CreateDefaultRequest();
            SetupHappyPathPolicies(request);

            _userRepositoryMock
                .IsEmailTakenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(false);
            _userRepositoryMock
                .IsUserNameTakenAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(false);

            var expectedHash = "secure_hashed_password_123";
            _passwordHasherMock
                .HashPassword(Arg.Any<User>(), Arg.Is(request.Password))
                .Returns(expectedHash);

            var fixedTime = new DateTime(2026, 2, 26, 12, 0, 0, DateTimeKind.Utc);
            _dateTimeProviderMock.UtcNow.Returns(fixedTime);

            var expectedRefreshToken = "mock_refresh_token_123";
            _refreshTokenGeneratorMock.GenerateToken().Returns(expectedRefreshToken);

            _jwtTokenGeneratorMock
                .GenerateToken(Arg.Any<User>())
                .Returns(("token", fixedTime.AddMinutes(60)));

            var result = await _sut.RegisterAsync(request);

            result.IsError.Should().BeFalse();

            await _userRepositoryMock
                .Received(1)
                .AddAsync(
                    Arg.Is<User>(u => u.PasswordHash == expectedHash),
                    Arg.Any<CancellationToken>()
                );

            await _refreshTokenRepositoryMock
                .Received(1)
                .AddAsync(
                    Arg.Is<RefreshToken>(rt => rt.Token == expectedRefreshToken),
                    Arg.Any<CancellationToken>()
                );

            await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        private static RegisterRequestDto CreateDefaultRequest() =>
            new("NewPlayer", "testPlayer@example.com", "Password123", "Password123");

        private void SetupHappyPathPolicies(RegisterRequestDto request)
        {
            _usernamePolicyMock
                .IsAllowedAsync(request.Username, Arg.Any<CancellationToken>())
                .Returns(new UsernameValidationResult(true));

            _usernamePolicyMock
                .Normalize(request.Username)
                .Returns(request.Username.ToUpperInvariant());

            _emailPolicyMock
                .ValidateAsync(request.Email, Arg.Any<CancellationToken>())
                .Returns(new EmailValidationResult(true));

            _emailPolicyMock.Normalize(request.Email).Returns(request.Email.ToUpperInvariant());

            _passwordPolicyMock
                .Validate(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(new PasswordValidationResult(true));
        }
    }
}

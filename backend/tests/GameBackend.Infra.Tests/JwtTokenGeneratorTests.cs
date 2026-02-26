using FluentAssertions;
using GameBackend.Core.Entities;
using GameBackend.Core.Interfaces.Services;
using GameBackend.Infra.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using NSubstitute;

namespace GameBackend.Infra.Tests;

public class JwtTokenGeneratorTests
{
    private readonly IDateTimeProvider _dateTimeProviderMock;
    private readonly IOptions<JwtSettings> _jwtOptions;
    private readonly JwtTokenGenerator _sut;
    private const string _testSecret = "super-secret-key-at-least-32-chars-long";

    public JwtTokenGeneratorTests()
    {
        _dateTimeProviderMock = Substitute.For<IDateTimeProvider>();

        var settings = new JwtSettings
        {
            Secret = _testSecret,
            ExpiryMinutes = 60,
            Issuer = "TestIssuer",
            Audience = "TestAudience",
        };

        _jwtOptions = Options.Create(settings);
        _sut = new JwtTokenGenerator(_jwtOptions, _dateTimeProviderMock);
    }

    [Fact]
    public void GenerateToken_ShouldReadSettingsCorrectly()
    {
        var fixedTime = new DateTime(2026, 2, 26, 12, 0, 0, DateTimeKind.Utc);
        _dateTimeProviderMock.UtcNow.Returns(fixedTime);

        var user = User.Create(
            "Lakan",
            "lakan@bathala.ph",
            "hashed_pass",
            "LAKAN",
            "LAKAN@BATHALA.PH",
            fixedTime
        );

        var (token, expiration) = _sut.GenerateToken(user);

        expiration.Should().Be(fixedTime.AddMinutes(60));
        token.Should().NotBeNullOrEmpty();

        var handler = new JsonWebTokenHandler();
        var jwt = handler.ReadJsonWebToken(token);

        jwt.Issuer.Should().Be("TestIssuer");
        jwt.Audiences.Should().Contain("TestAudience");
        jwt.Subject.Should().Be(user.Id.ToString());

        jwt.ValidTo.Should().BeCloseTo(expiration, precision: TimeSpan.FromSeconds(1));
    }
}

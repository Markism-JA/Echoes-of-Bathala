using FluentAssertions;
using GameBackend.Core.Common.Authentication;
using GameBackend.Core.Entities;
using GameBackend.Infra.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;

namespace GameBackend.Infra.Tests;

public class JwtTokenGeneratorTests
{
    private readonly IOptions<JwtSettings> _jwtOptions;
    private readonly JwtTokenGenerator _sut;
    private const string _testSecret = "super-secret-key-at-least-32-chars-long";

    public JwtTokenGeneratorTests()
    {
        var settings = new JwtSettings
        {
            Secret = _testSecret,
            ExpiryMinutes = 60,
            Issuer = "TestIssuer",
            Audience = "TestAudience",
        };

        _jwtOptions = Options.Create(settings);
        _sut = new JwtTokenGenerator(_jwtOptions);
    }

    [Fact]
    public void GenerateToken_ShouldReadSettingsCorrectly()
    {
        var fixedTime = new DateTime(2026, 2, 26, 12, 0, 0, DateTimeKind.Utc);

        var user = User.Create(
            "Lakan",
            "lakan@bathala.ph",
            "hashed_pass",
            "LAKAN",
            "LAKAN@BATHALA.PH",
            fixedTime
        );

        var (token, expiration) = _sut.GenerateToken(user, fixedTime);

        expiration.Should().Be(fixedTime.AddMinutes(60));
        token.Should().NotBeNullOrEmpty();

        var handler = new JsonWebTokenHandler();
        var jwt = handler.ReadJsonWebToken(token);

        jwt.Issuer.Should().Be("TestIssuer");
        jwt.Audiences.Should().Contain("TestAudience");
        jwt.Subject.Should().Be(user.Id.ToString());

        jwt.ValidTo.Should().BeCloseTo(expiration, precision: TimeSpan.FromSeconds(1));

        jwt.GetClaim(JwtRegisteredClaimNames.Name).Value.Should().Be("Lakan");
    }
}

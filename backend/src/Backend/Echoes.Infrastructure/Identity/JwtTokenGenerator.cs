using System.Text;
using Echoes.Application.Auth.Abstractions;
using Echoes.Application.Auth.Models;
using Echoes.Domain.Users.Persistence;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Echoes.Infrastructure.Identity;

/// <summary>
/// Implementation of <see cref="IJwtTokenGenerator"/> that issues signed HMAC-SHA256 tokens.
/// </summary>
public class JwtTokenGenerator(IOptions<JwtSettings> jwtOptions) : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    /// <inheritdoc />
    public (string Token, DateTime Expiration) GenerateToken(UserEntity user, DateTime now)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new Dictionary<string, object>
        {
            [JwtRegisteredClaimNames.Sub] = user.Id.ToString(),
            [JwtRegisteredClaimNames.Name] = user.UserName,
            [JwtRegisteredClaimNames.Email] = user.Email,
            [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString(),

            // TODO: Implement authorization claims.
            // TODO: Implement iat claim.
        };

        var expiration = now.AddMinutes(_jwtSettings.ExpiryMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Claims = claims,
            Expires = expiration,
            SigningCredentials = credentials,
        };

        var tokenHandler = new JsonWebTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return (token, expiration);
    }
}

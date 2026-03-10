using Echoes.Domain.Users.Persistence;

namespace Echoes.Application.Auth.Abstractions;

public interface IJwtTokenGenerator
{
    public (string Token, DateTime Expiration) GenerateToken(UserEntity user, DateTime utcNow);
}

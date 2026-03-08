using Echoes.Domain.Users;

namespace Echoes.Application.Auth.Abstractions;

public interface IJwtTokenGenerator
{
    public (string Token, DateTime Expiration) GenerateToken(User user, DateTime utcNow);
}

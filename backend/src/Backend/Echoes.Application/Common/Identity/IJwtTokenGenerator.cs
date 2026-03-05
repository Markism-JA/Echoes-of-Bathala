using Echoes.Domain.Users;

namespace Echoes.Application.Common.Identity;

public interface IJwtTokenGenerator
{
    public (string Token, DateTime Expiration) GenerateToken(User user, DateTime utcNow);
}

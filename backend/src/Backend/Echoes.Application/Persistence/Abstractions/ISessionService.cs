using Echoes.Domain.Auth;

namespace Echoes.Application.Persistence.Abstractions;

public interface ISessionService
{
    public Task CreateSessionAsync(RefreshToken token);
    public Task RevokeSessionAsync(RefreshToken token);
    public Task<bool> IsSessionValidAsync(RefreshToken token);
}

using Echoes.Domain;

namespace Echoes.Application.Common.Persistence;

public interface ISessionService
{
    public Task CreateSessionAsync(RefreshToken token);
    public Task RevokeSessionAsync(RefreshToken token);
    public Task<bool> IsSessionValidAsync(RefreshToken token);
}

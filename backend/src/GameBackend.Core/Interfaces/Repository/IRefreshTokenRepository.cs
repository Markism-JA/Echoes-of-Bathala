using GameBackend.Core.Entities;

namespace GameBackend.Core.Interfaces.Repository;

public interface IRefreshTokenRepository : IRepository<RefreshToken, Guid>
{
    public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default);
    public Task RevokeAllForUserAsync(Guid userId, CancellationToken ct = default);
}

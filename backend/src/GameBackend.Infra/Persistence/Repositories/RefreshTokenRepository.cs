using GameBackend.Core.Entities;
using GameBackend.Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace GameBackend.Infra.Persistence.Repositories;

public class RefreshTokenRepository(GameDbContext context)
    : Repository<RefreshToken, Guid>(context),
        IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default)
    {
        return await _dbSet.FirstOrDefaultAsync(rt => rt.Token == token, ct);
    }

    public async Task RevokeAllForUserAsync(Guid userId, CancellationToken ct = default)
    {
        var activeTokens = await _dbSet
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync(ct);

        foreach (var token in activeTokens)
        {
            token.Revoke();
        }
    }

    public async Task RevokeExcessTokensAsync(Guid userId, int maxSessions, CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        var oldTokens = await _dbSet
            .Where(t => t.UserId == userId && !t.IsRevoked && t.ExpiryDate > now)
            .OrderByDescending(t => t.CreatedAt)
            .Skip(maxSessions - 1)
            .ToListAsync(ct);

        foreach (var token in oldTokens)
        {
            token.Revoke();
        }
    }

    public Task RevokeTokenAsync(string token, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    Task<List<RefreshToken>> IRefreshTokenRepository.GetActiveTokensForUserAsync(
        Guid id,
        CancellationToken ct
    )
    {
        throw new NotImplementedException();
    }
}

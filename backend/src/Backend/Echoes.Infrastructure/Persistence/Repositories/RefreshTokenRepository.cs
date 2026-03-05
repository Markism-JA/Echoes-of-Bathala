using Echoes.Domain;
using Echoes.Domain.Repository;

namespace Echoes.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository(GameDbContext context)
        : Repository<RefreshToken>(context),
            IRefreshTokenRepository { }
}

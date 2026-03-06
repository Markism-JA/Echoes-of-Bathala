using Echoes.Domain;
using Echoes.Domain.Repository;

namespace Echoes.Infrastructure.Persistence.Postgresql.Repositories
{
    public class RefreshTokenRepository(GameDbContext context)
        : Repository<RefreshToken>(context),
            IRefreshTokenRepository { }
}

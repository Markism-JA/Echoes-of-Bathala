using Echoes.Application.Common.Persistence;

namespace Echoes.Infrastructure.Persistence.Postgresql
{
    public class UnitOfWork(GameDbContext context) : IUnitOfWork
    {
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

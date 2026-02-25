using GameBackend.Core.Interfaces.Persistence;

namespace GameBackend.Infra.Persistence
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

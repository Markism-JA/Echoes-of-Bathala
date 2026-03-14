using Echoes.Application.Persistence.Abstractions;

namespace Echoes.Infrastructure.Persistence.Postgresql
{
    /// <inheritdoc cref="IUnitOfWork" />
    public class UnitOfWork(GameDbContext context) : IUnitOfWork
    {
        /// <inheritdoc/>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}

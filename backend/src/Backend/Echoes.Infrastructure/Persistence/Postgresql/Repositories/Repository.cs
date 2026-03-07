using Echoes.Domain.Common;
using Echoes.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Echoes.Infrastructure.Persistence.Postgresql.Repositories
{
    public class Repository<T>(GameDbContext context) : IRepository<T>
        where T : class, IAggregateRoot
    {
        protected readonly GameDbContext Context = context;
        protected readonly DbSet<T> DbSet = context.Set<T>();

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await DbSet.FindAsync([id], ct);
        }

        public async Task AddAsync(T entity, CancellationToken ct = default)
        {
            await DbSet.AddAsync(entity, ct);
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }

        public void Remove(T entity)
        {
            if (entity is ISoftDelete softDeletable)
            {
                softDeletable.IsDeleted = true;
                softDeletable.DeletedAt = DateTime.UtcNow;
                Context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                DbSet.Remove(entity);
            }
        }
    }
}

using Echoes.Domain.Common;
using Echoes.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Echoes.Infrastructure.Persistence.Repositories
{
    public class Repository<T>(GameDbContext context) : IRepository<T>
        where T : class, IAggregateRoot
    {
        protected readonly GameDbContext _context = context;
        protected readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbSet.FindAsync([id], ct);
        }

        public async Task AddAsync(T entity, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entity, ct);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove(T entity)
        {
            if (entity is ISoftDelete softDeletable)
            {
                softDeletable.IsDeleted = true;
                softDeletable.DeletedAt = DateTime.UtcNow;
                _context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                _dbSet.Remove(entity);
            }
        }
    }
}

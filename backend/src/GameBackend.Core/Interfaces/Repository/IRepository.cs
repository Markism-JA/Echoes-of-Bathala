using System.Linq.Expressions;

namespace GameBackend.Core.Interfaces.Repository
{
    public interface IRepository<T, TId>
        where T : class
    {
        public Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
        public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default
        );

        public Task AddAsync(T entity, CancellationToken cancellationToken = default);
        public Task AddRangeAsync(
            IEnumerable<T> entities,
            CancellationToken cancellationToken = default
        );

        public void Remove(T entity);
        public void RemoveRange(IEnumerable<T> entities);

        public void Update(T entity);
    }
}

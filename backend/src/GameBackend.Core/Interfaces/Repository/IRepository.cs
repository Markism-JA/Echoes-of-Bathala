using System.Linq.Expressions;

namespace GameBackend.Core.Interfaces.Repository
{
    public interface IRepository<T, TId>
        where T : class
    {
        Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default
        );

        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        void Update(T entity);
    }
}

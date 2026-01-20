using System.Linq.Expressions;

namespace GameBackend.Core.Interfaces.Repository
{
    public interface IRepository<T>
        where T : class
    {
        // 1. Reading Data
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default
        );

        // 2. Modifying Data
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        // Note: Update and Remove are often synchronous because
        // they usually just mark the state in the ChangeTracker (if using EF Core)
        // However, keeping them async allows for flexibility in other implementations.
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        void Update(T entity);
    }
}

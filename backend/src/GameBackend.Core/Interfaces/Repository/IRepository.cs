using System.Linq.Expressions;

namespace GameBackend.Core.Interfaces.Repository;

/// <summary>
/// Defines the generic contract for a repository, providing basic CRUD and querying operations
/// for domain entities within the GameBackend system.
/// </summary>
/// <typeparam name="T">The type of the domain entity. Must be a reference class.</typeparam>
/// <typeparam name="TId">The type of the primary key for the entity (e.g., Guid or int).</typeparam>
public interface IRepository<T, TId>
    where T : class
{
    /// <summary>
    /// Retrieves a single entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    public Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all entities of type <typeparamref name="T"/> from the data store.
    /// </summary>
    /// <remarks>
    /// Use with caution for large tables (like Game Logs); consider pagination for production.
    /// </remarks>
    public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds entities based on a provided LINQ expression predicate.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A collection of entities that satisfy the condition.</returns>
    public Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Begins tracking the specified entity to be inserted into the database upon the next SaveChanges.
    /// </summary>
    public Task AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins tracking a collection of entities to be inserted into the database.
    /// </summary>
    public Task AddRangeAsync(
        IEnumerable<T> entities,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Marks an existing entity for deletion in the unit of work.
    /// </summary>
    /// <remarks>
    /// This method is synchronous as it only modifies the state in the change tracker.
    /// </remarks>
    public void Remove(T entity);

    /// <summary>
    /// Marks a collection of entities for deletion.
    /// </summary>
    public void RemoveRange(IEnumerable<T> entities);

    /// <summary>
    /// Marks an entity as modified in the change tracker.
    /// </summary>
    /// <remarks>
    /// Most EF Core implementations do not require this if the entity was
    /// tracked during retrieval, but it is included for explicit updates.
    /// </remarks>
    public void Update(T entity);
}

namespace GameBackend.Core.Interfaces.Persistence;

/// <summary>
/// Maintains a list of objects affected by a business transaction and coordinates
/// the writing out of changes and the resolution of concurrency problems.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Atomically persists all changes made within the current transaction scope to the database.
    /// </summary>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous save operation.
    /// The task result contains the number of state entries written to the database.
    /// </returns>
    /// <remarks>
    /// In the context of GameBackend, this ensures that operations like User Registration
    /// and Refresh Token generation either succeed completely or fail without partial data.
    /// </remarks>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

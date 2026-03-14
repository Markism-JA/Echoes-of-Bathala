namespace Echoes.Application.Persistence.Abstractions;

/// <summary>
/// Coordinates the persistence of multiple changes as a single atomic transaction.
/// This prevents data inconsistency by ensuring that either all changes succeed, or none are applied.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Commits all staged changes from various repositories to the underlying data store in a single batch.
    /// </summary>
    /// <param name="cancellationToken">Used to abort the operation if the request is cancelled.</param>
    /// <returns>
    /// A task containing the number of rows/entities affected.
    /// Use this value to verify that the expected number of changes occurred.
    /// </returns>
    /// <exception cref="DbUpdateException">Thrown if the database rejects the changes due to constraints or connectivity issues.</exception>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

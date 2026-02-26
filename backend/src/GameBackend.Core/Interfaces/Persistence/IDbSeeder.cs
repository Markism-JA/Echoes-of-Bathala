namespace GameBackend.Core.Interfaces.Persistence;

/// <summary>
/// Defines a contract for initializing the database with essential starting data.
/// </summary>
public interface IDbSeeder
{
    /// <summary>
    /// Executes the database seeding logic to populate initial state,
    /// administrative accounts, and game constants.
    /// </summary>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous seeding operation.</returns>
    /// <remarks>
    /// This method should be idempotent, meaning it can be called multiple times
    /// without creating duplicate records or causing errors if the data already exists.
    /// </remarks>
    public Task SeedAsync(CancellationToken ct = default);
}

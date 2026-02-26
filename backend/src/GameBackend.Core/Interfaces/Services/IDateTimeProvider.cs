namespace GameBackend.Core.Interfaces.Services;

/// <summary>
/// Provides an abstraction for the system clock to ensure consistency,
/// predictability, and testability across the application.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Gets the current date and time in Coordinated Universal Time (UTC).
    /// </summary>
    /// <value>
    /// The current UTC date and time.
    /// </value>
    /// <remarks>
    /// All time-based logic in GameBackend (e.g., Token Expiration, User Creation,
    /// and Event Logging) must use this property instead of <see cref="DateTime.UtcNow"/>
    /// to allow for time-travel testing and global synchronization.
    /// </remarks>
    public DateTime UtcNow { get; }
}

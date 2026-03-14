namespace Echoes.Application.Common.Services
{
    /// <summary>
    /// Provides an abstraction for the system clock to ensure testability and consistency.
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Gets the current date and time on this computer, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        public DateTime UtcNow { get; }
    }
}

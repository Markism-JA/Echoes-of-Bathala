namespace Echoes.Application.Persistence.Abstractions
{
    /// <summary>
    /// Defines a contract for a generic transient key-value storage service,
    /// used for general caching or temporary data buffering.
    /// </summary>
    public interface IBufferService
    {
        /// <summary>
        /// Asynchronously stores a value in the buffer with an optional expiration.
        /// </summary>
        /// <typeparam name="T">The type of the value to be stored.</typeparam>
        /// <param name="key">The unique identifier for the cached item.</param>
        /// <param name="value">The data object to store.</param>
        /// <param name="expiration">An optional <see cref="TimeSpan"/> representing the Time-To-Live (TTL).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);

        /// <summary>
        /// Asynchronously retrieves a value from the buffer.
        /// </summary>
        /// <typeparam name="T">The type to which the stored data should be deserialized.</typeparam>
        /// <param name="key">The unique identifier for the cached item.</param>
        /// <returns>
        /// A task that returns the value of type <typeparamref name="T"/> if found;
        /// otherwise, <see langword="null"/>.
        /// </returns>
        public Task<T?> GetAsync<T>(string key);

        /// <summary>
        /// Asynchronously removes a specific item from the buffer.
        /// </summary>
        /// <param name="key">The unique identifier for the item to remove.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task RemoveAsync(string key);
    }
}

namespace Echoes.Application.Persistence.Abstractions
{
    /// <summary>
    /// Defines a contract for a Publisher/Subscriber messaging pattern,
    /// enabling decoupled communication between different parts of the system.
    /// </summary>
    public interface IPubSubService
    {
        /// <summary>
        /// Asynchronously publishes a message to a specific channel.
        /// </summary>
        /// <typeparam name="T">The type of the message payload.</typeparam>
        /// <param name="channel">The name of the topic or channel to broadcast to.</param>
        /// <param name="message">The data payload to send to subscribers.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task PublishAsync<T>(string channel, T message);

        /// <summary>
        /// Asynchronously subscribes to a specific channel and defines a handler for incoming messages.
        /// </summary>
        /// <typeparam name="T">The type of the expected message payload.</typeparam>
        /// <param name="channel">The name of the topic or channel to listen to.</param>
        /// <param name="handler">An action to execute whenever a message is received.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SubscribeAsync<T>(string channel, Action<T> handler);
    }
}

namespace Echoes.Application.Persistence.Abstractions
{
    public interface IPubSubService
    {
        public Task PublishAsync<T>(string channel, T message);
        public Task SubscribeAsync<T>(string channel, Action<T> handler);
    }
}

namespace Echoes.Application.Persistence.Abstractions
{
    public interface IBufferService
    {
        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        public Task<T?> GetAsync<T>(string key);
        public Task RemoveAsync(string key);
    }
}

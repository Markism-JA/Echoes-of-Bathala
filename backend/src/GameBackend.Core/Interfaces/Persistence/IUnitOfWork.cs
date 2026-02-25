namespace GameBackend.Core.Interfaces.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

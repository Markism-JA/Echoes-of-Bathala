namespace Echoes.Application.Persistence.Abstractions;

public interface IUnitOfWork : IDisposable
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

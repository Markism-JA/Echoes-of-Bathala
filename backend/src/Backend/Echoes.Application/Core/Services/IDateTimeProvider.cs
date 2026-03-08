namespace Echoes.Application.Core.Services
{
    public interface IDateTimeProvider
    {
        public DateTime UtcNow { get; }
    }
}

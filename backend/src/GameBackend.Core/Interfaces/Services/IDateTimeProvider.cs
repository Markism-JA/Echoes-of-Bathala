namespace GameBackend.Core.Interfaces.Services
{
    public interface IDateTimeProvider
    {
        public DateTime UtcNow { get; }
    }
}

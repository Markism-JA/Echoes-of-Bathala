using GameBackend.Core.Interfaces.Services;

namespace GameBackend.Infra.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}

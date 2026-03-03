using Echoes.Domain.Common;

namespace Echoes.Domain
{
    public abstract class BaseEntity : IAuditable
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

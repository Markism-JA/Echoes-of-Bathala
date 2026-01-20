using GameBackend.Core.Interfaces;

namespace GameBackend.Core.Entities.Base
{
    public class BaseEntity : IEntity<int>, IAuditable
    {
        int IEntity<int>.Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

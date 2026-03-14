namespace Echoes.Domain.Common
{
    /// <summary>
    /// Provides a standardized base for domain entities, contains identification
    /// and audit tracking.
    /// </summary>
    public abstract class BaseEntity : IAuditable
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

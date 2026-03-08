namespace Echoes.Infrastructure.Persistence.Redis.Dto
{
    public class SessionDto
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = null!;
        public Guid UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

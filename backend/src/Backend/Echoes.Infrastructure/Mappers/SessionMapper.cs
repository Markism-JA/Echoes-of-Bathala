using Echoes.Domain;
using Echoes.Infrastructure.Persistence.Redis.Models;

namespace Echoes.Infrastructure.Mappers
{
    public static class SessionMapper
    {
        public static SessionDto ToDto(RefreshToken entity)
        {
            return new SessionDto
            {
                Id = entity.Id,
                Token = entity.Token,
                UserId = entity.UserId,
                ExpiryDate = entity.ExpiryDate,
                IsRevoked = entity.IsRevoked,
                CreatedAt = entity.CreatedAt,
            };
        }

        public static RefreshToken ToDomain(this SessionDto dto)
        {
            return RefreshToken.Restore(
                dto.Id,
                dto.Token,
                dto.UserId,
                dto.ExpiryDate,
                dto.IsRevoked,
                dto.CreatedAt
            );
        }
    }
}

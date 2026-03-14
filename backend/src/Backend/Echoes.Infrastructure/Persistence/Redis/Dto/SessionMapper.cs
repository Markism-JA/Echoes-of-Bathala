using Echoes.Domain;
using Echoes.Domain.Auth;

namespace Echoes.Infrastructure.Persistence.Redis.Dto
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

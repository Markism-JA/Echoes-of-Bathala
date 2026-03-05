using ErrorOr;

namespace Echoes.Application.Common.Identity
{
    public interface IIdentityService
    {
        public Task<ErrorOr<Guid>> RegisterUserAsync(
            string username,
            string email,
            string password,
            Guid userId,
            CancellationToken ct
        );
    }
}

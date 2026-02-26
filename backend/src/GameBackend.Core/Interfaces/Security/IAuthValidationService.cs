using ErrorOr;

namespace GameBackend.Core.Interfaces.Security
{
    public interface IAuthValidationService
    {
        public Task<ErrorOr<bool>> IsUsernameAvailableAsync(
            string username,
            CancellationToken ct = default
        );
        public Task<ErrorOr<bool>> IsEmailAvailableAsync(
            string email,
            CancellationToken ct = default
        );
    }
}

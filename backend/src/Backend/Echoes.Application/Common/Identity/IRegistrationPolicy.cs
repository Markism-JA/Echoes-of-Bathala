using Echoes.Application.Auth.RegisterEmail;
using ErrorOr;

namespace Echoes.Application.Common.Identity
{
    public interface IRegistrationPolicy
    {
        public Task<ErrorOr<RegistrationDetails>> IsAllowedAsync(
            RegisterEmailCommand command,
            CancellationToken ct
        );
    }
}

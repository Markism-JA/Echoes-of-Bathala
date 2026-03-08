using Echoes.Application.Auth.Models;
using Echoes.Application.Auth.RegisterEmail;
using ErrorOr;

namespace Echoes.Application.Auth.Policies
{
    public interface IRegistrationPolicy
    {
        public Task<ErrorOr<RegistrationDetails>> IsAllowedAsync(
            RegisterEmailCommand command,
            CancellationToken ct
        );
    }
}

using Echoes.Application.Auth.RegisterEmail;
using Echoes.Domain.Auth;
using ErrorOr;

namespace Echoes.Application.Auth.Policies;

/// <summary>
/// Defines the contract for validating whether a new user registration
/// request meets all business, security, and persistence requirements.
/// </summary>
public interface IRegistrationPolicy
{
    /// <summary>
    /// Evaluates the registration command against format, security, and uniqueness policies.
    /// </summary>
    /// <param name="command">The user-provided registration data.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>
    /// An <see cref="ErrorOr{T}"/> containing <see cref="RegistrationDetails"/> (normalized data)
    /// on success, or a collection of validation errors on failure.
    /// </returns>
    public Task<ErrorOr<RegistrationDetails>> IsAllowedAsync(
        RegisterEmailCommand command,
        CancellationToken ct
    );
}

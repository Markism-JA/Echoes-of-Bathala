using ErrorOr;

namespace Echoes.Application.Auth.Abstractions
{
    /// <summary>
    /// Defines user identity management operations for the application.
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// Asynchronously registers a new user within the identity system.
        /// </summary>
        /// <param name="username">The unique username for the new account.</param>
        /// <param name="email">The email address associated with the account.</param>
        /// <param name="password">The plain-text password to be hashed and stored.</param>
        /// <param name="userId">The unique identifier to assign to the user.</param>
        /// <param name="ct">The cancellation token to cancel the operation.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result contains an <see cref="ErrorOr{TValue}"/> which holds the
        /// created <see cref="Guid"/> on success, or a list of validation errors on failure.
        /// </returns>
        public Task<ErrorOr<Guid>> RegisterUserAsync(
            string username,
            string email,
            string password,
            Guid userId,
            CancellationToken ct
        );
    }
}

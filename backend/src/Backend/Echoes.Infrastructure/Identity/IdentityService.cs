using Echoes.Application.Auth.Abstractions;
using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace Echoes.Infrastructure.Identity
{
    /// <summary>
    /// An implementation of <see cref="IIdentityService"/> using ASP.NET Core Identity.
    /// </summary>
    /// <remarks>
    /// This service acts as a wrapper around the <see cref="UserManager{TUser}"/>
    /// to map Identity results into the application's <see cref="ErrorOr"/> format.
    /// </remarks>
    /// <param name="userManager">The ASP.NET Core Identity user manager.</param>
    public class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
    {
        /// <inheritdoc />
        /// <remarks>
        /// If the <see cref="UserManager{TUser}"/> returns errors (e.g., password too weak,
        /// duplicate email), they are converted into <see cref="Error.Validation"/> results.
        /// </remarks>
        public async Task<ErrorOr<Guid>> RegisterUserAsync(
            string username,
            string email,
            string password,
            Guid userId,
            CancellationToken ct
        )
        {
            var user = new ApplicationUser
            {
                Id = userId,
                UserName = username,
                Email = email,
            };

            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return result.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();
            }

            return user.Id;
        }
    }
}

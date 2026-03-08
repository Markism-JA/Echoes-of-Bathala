using Echoes.Application.Auth.Abstractions;
using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace Echoes.Infrastructure.Identity
{
    public class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
    {
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

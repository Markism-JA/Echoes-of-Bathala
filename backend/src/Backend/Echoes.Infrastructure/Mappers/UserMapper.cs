using Echoes.Domain.Users;
using Echoes.Infrastructure.Identity;

namespace Echoes.Infrastructure.Mappers
{
    public static class UserMapper
    {
        public static User ToDomain(ApplicationUser appUser)
        {
            return User.Create(
                appUser.UserName!,
                appUser.NormalizedUserName!,
                appUser.Email!,
                appUser.NormalizedEmail!
            );
        }

        public static void MapToApplicationUser(User domainUser, ApplicationUser appUser)
        {
            appUser.UserName = domainUser.UserName;
            appUser.NormalizedUserName = domainUser.NormalizedUserName;
            appUser.Email = domainUser.Email;
            appUser.NormalizedEmail = domainUser.NormalizedEmail;
        }
    }
}

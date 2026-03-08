using Echoes.Domain.Users;

namespace Echoes.Infrastructure.Identity
{
    public static class UserMapper
    {
        public static User ToDomain(ApplicationUser appUser)
        {
            return User.Create(
                appUser.Id,
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

using Echoes.Domain.Users;
using Echoes.Domain.Users.Persistence;

namespace Echoes.Infrastructure.Identity
{
    public static class UserMapper
    {
        public static UserEntity ToDomain(ApplicationUser appUser)
        {
            return UserEntity.Create(
                appUser.Id,
                appUser.UserName!,
                appUser.NormalizedUserName!,
                appUser.Email!,
                appUser.NormalizedEmail!
            );
        }

        public static void MapToApplicationUser(UserEntity domainUser, ApplicationUser appUser)
        {
            appUser.UserName = domainUser.UserName;
            appUser.NormalizedUserName = domainUser.NormalizedUserName;
            appUser.Email = domainUser.Email;
            appUser.NormalizedEmail = domainUser.NormalizedEmail;
        }
    }
}

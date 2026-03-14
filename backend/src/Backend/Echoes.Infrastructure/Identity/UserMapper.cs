using Echoes.Domain.Users.Persistence;

namespace Echoes.Infrastructure.Identity
{
    /// <summary>
    /// Provides utility methods to map between the Identity-specific <see cref="ApplicationUser"/>
    /// and the domain-centric <see cref="UserEntity"/>.
    /// </summary>
    /// <remarks>
    /// This mapper facilitates the separation of concerns by ensuring the Infrastructure layer
    /// can translate security-focused user models into business-focused domain entities.
    /// </remarks>
    public static class UserMapper
    {
        /// <summary>
        /// Converts an <see cref="ApplicationUser"/> into a new <see cref="UserEntity"/>.
        /// </summary>
        /// <param name="appUser">The infrastructure-level user model.</param>
        /// <returns>A validated <see cref="UserEntity"/> instance.</returns>
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

        /// <summary>
        /// Synchronizes an existing <see cref="ApplicationUser"/> with data from a <see cref="UserEntity"/>.
        /// </summary>
        /// <param name="domainUser">The source of truth from the Domain layer.</param>
        /// <param name="appUser">The target identity user to be updated.</param>
        public static void MapToApplicationUser(UserEntity domainUser, ApplicationUser appUser)
        {
            appUser.UserName = domainUser.UserName;
            appUser.NormalizedUserName = domainUser.NormalizedUserName;
            appUser.Email = domainUser.Email;
            appUser.NormalizedEmail = domainUser.NormalizedEmail;
        }
    }
}

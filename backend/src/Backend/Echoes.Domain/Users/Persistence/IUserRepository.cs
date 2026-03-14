using Echoes.Domain.Common;

namespace Echoes.Domain.Users.Persistence
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        /// <summary>
        /// Retrieves a user by their normalized email address.
        /// </summary>
        /// <param name="normalizedEmail">The email address after policy normalization (e.g., uppercase).</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The <see cref="User"/> if a match is found; otherwise, null.</returns>
        public Task<UserEntity?> GetByEmailAsync(
            string normalizedEmail,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Retrieves a user by their normalized username.
        /// </summary>
        /// <param name="normalizedUsername">The username after policy normalization.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The <see cref="User"/> if a match is found; otherwise, null.</returns>
        public Task<UserEntity?> GetByUserNameAsync(
            string normalizedUsername,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Retrieves a user associated with a specific cryptocurrency wallet address.
        /// </summary>
        /// <param name="walletAddress">The unique public wallet address string.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The <see cref="User"/> if a link exists; otherwise, null.</returns>
        public Task<UserEntity?> GetByWalletAddressAsync(
            string walletAddress,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Checks if an email address is already registered in the system.
        /// </summary>
        /// <param name="normalizedEmail">The email address to verify.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>True if the email is already in use; otherwise, false.</returns>
        public Task<bool> IsEmailTakenAsync(
            string normalizedEmail,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Checks if a username is already claimed by another player.
        /// </summary>
        /// <param name="normalizedUsername">The username to verify.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>True if the username is taken; otherwise, false.</returns>
        public Task<bool> IsUserNameTakenAsync(
            string normalizedUsername,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Checks if a wallet address has already been linked to an existing account.
        /// </summary>
        /// <param name="walletAddress">The public wallet address to verify.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>True if the wallet is already associated with a user; otherwise, false.</returns>
        public Task<bool> IsWalletLinkedAsync(
            string walletAddress,
            CancellationToken cancellationToken = default
        );
    }
}

using GameBackend.Core.Entities;

namespace GameBackend.Core.Interfaces.Security;

/// <summary>
/// Defines the contract for secure password storage and verification using
/// one-way cryptographic hashing algorithms.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Generates a secure, salted cryptographic hash of a plain-text password.
    /// </summary>
    /// <param name="user">The user entity associated with the password (useful for unique salting strategies).</param>
    /// <param name="password">The raw, plain-text password provided by the player.</param>
    /// <returns>A string containing the encoded hash, typically including the salt and algorithm metadata.</returns>
    /// <remarks>
    /// This method is one-way; the resulting string cannot be reversed to find the original password.
    /// </remarks>
    public string HashPassword(User user, string password);

    /// <summary>
    /// Compares a plain-text password against a stored hash to determine if they match.
    /// </summary>
    /// <param name="user">The user entity whose credentials are being verified.</param>
    /// <param name="password">The raw, plain-text password entered during a login attempt.</param>
    /// <param name="hashedPassword">The secure hash retrieved from the database.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
    /// <remarks>
    /// Implementations should utilize constant-time comparison algorithms to prevent
    /// timing attacks that could reveal information about the hash.
    /// </remarks>
    public bool VerifyPassword(User user, string password, string hashedPassword);
}

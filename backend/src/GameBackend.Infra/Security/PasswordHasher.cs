using GameBackend.Core.Entities;
using GameBackend.Core.Interfaces.Security;
using Microsoft.AspNetCore.Identity;

namespace GameBackend.Infra.Security;

public class PasswordHasher(IPasswordHasher<User> hasher) : IPasswordHasher
{
    public string HashPassword(User user, string password)
    {
        return hasher.HashPassword(user, password);
    }

    public bool VerifyPassword(User user, string hashedPassword, string providedPassword)
    {
        var result = hasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}

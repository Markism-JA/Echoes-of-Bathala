using GameBackend.Core.Entities;
using GameBackend.Core.Interfaces.Persistence;
using GameBackend.Core.Interfaces.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameBackend.Infra.Persistence;

public class DbSeeder(
    GameDbContext context,
    IPasswordHasher passwordHasher,
    ILogger<DbSeeder> logger
) : IDbSeeder
{
    public async Task SeedAsync(CancellationToken ct = default)
    {
        logger.LogInformation("Initializing database seeding process...");

        await context.Database.EnsureCreatedAsync(ct);

        await SeedAdminUsersAsync(ct);

        logger.LogInformation("Database seeding completed successfully.");
    }

    private async Task SeedAdminUsersAsync(CancellationToken ct)
    {
        var initialAdmins = new[]
        {
            (Username: "Admin_Ron", Email: "ron@bathala.com"),
            (Username: "Admin_Matthew", Email: "matthew@bathala.com"),
            (Username: "Admin_Mark", Email: "mark@bathala.com"),
            (Username: "Admin_Andrei", Email: "andrei@bathala.com"),
        };

        var addedCount = 0;
        var now = DateTime.UtcNow;

        foreach (var (username, email) in initialAdmins)
        {
            if (!await context.Users.AnyAsync(u => u.Email == email, ct))
            {
                var admin = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = username,
                    NormalizedUserName = username.ToUpperInvariant(),
                    Email = email,
                    NormalizedEmail = email.ToUpperInvariant(),
                    EmailConfirmed = true,
                    Status = Shared.Enums.UserStatus.Active,
                    IsDeleted = false,
                    CreatedAt = now,
                    UpdatedAt = now,

                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),

                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                };

                admin.PasswordHash = passwordHasher.HashPassword(admin, "DevAdmin2026!");

                await context.Users.AddAsync(admin, ct);
                addedCount++;
            }
        }

        if (addedCount > 0)
        {
            await context.SaveChangesAsync(ct);
            logger.LogInformation("Successfully seeded {Count} new Admin accounts.", addedCount);
        }
        else
        {
            logger.LogInformation("Admin accounts are already up to date. No new accounts seeded.");
        }
    }
}

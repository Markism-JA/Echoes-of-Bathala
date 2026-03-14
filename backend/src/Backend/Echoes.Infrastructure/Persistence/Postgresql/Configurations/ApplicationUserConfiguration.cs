using Echoes.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Echoes.Infrastructure.Persistence.Postgresql.Configurations
{
    /// <summary>
    /// Configures the database schema and constraints for the <see cref="ApplicationUser"/> entity.
    /// </summary>
    /// <remarks>
    /// This configuration applies PostgreSQL-specific optimizations, handles naming conventions,
    /// and ensures identity uniqueness while accounting for the global Soft Delete filter.
    /// </remarks>
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("application_user");
            builder.Property(u => u.UserName).HasMaxLength(32).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(254).IsRequired();

            // UNIQUE INDEXES:
            // These ensure that no two 'active' users can share the same Email or Username.
            // The .HasFilter ensures that if a user is soft-deleted, their Email/Username
            // becomes available for reuse by new accounts.

            builder
                .HasIndex(u => u.NormalizedEmail)
                .IsUnique()
                .HasFilter("\"IsDeleted\" = false");

            builder
                .HasIndex(u => u.NormalizedUserName)
                .IsUnique()
                .HasFilter("\"IsDeleted\" = false");
        }
    }
}

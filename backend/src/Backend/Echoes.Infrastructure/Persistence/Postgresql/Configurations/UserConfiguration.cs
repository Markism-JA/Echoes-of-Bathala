using Echoes.Domain;
using Echoes.Domain.Users.Persistence;
using Echoes.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Echoes.Infrastructure.Persistence.Postgresql.Configurations
{
    /// <summary>
    /// Configures the database schema for the <see cref="UserEntity"/> domain model.
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("users");

            // Sets up a 1:1 relationship where UserEntity uses the same Primary Key as ApplicationUser.
            // Cascade delete ensures that if the Identity account is removed, the domain profile follows.
            builder
                .HasOne<ApplicationUser>()
                .WithOne(a => a.User)
                .HasForeignKey<UserEntity>(u => u.Id);

            builder.Property(u => u.UserName).HasMaxLength(32).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(254).IsRequired();
            builder.Property(u => u.NormalizedUserName).HasMaxLength(32).IsRequired();
            builder.Property(u => u.NormalizedEmail).HasMaxLength(254).IsRequired();

            builder
                .Property(e => e.Status)
                .HasConversion<int>()
                .HasDefaultValue(UserStatus.Unverified);

            builder.Property(e => e.LinkedWalletAddress).HasMaxLength(42).IsRequired(false);

            builder
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("timezone('utc', now())")
                .IsRequired();

            builder.HasIndex(e => e.NormalizedEmail).IsUnique().HasFilter("\"IsDeleted\" = false");

            builder
                .HasIndex(e => e.NormalizedUserName)
                .IsUnique()
                .HasFilter("\"IsDeleted\" = false");

            builder
                .HasIndex(e => e.LinkedWalletAddress)
                .IsUnique()
                .HasFilter("\"IsDeleted\" = false AND \"LinkedWalletAddress\" IS NOT NULL");
        }
    }
}

using GameBackend.Core.Entities;
using GameBackend.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameBackend.Infra.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.Property(u => u.UserName).HasMaxLength(32).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(254).IsRequired();

            builder.Property(u => u.NormalizedUserName).HasMaxLength(32);
            builder.Property(u => u.NormalizedEmail).HasMaxLength(254);

            builder
                .Property(e => e.Status)
                .HasConversion<int>()
                .HasDefaultValue(UserStatus.Unverified);

            builder.Property(e => e.LinkedWalletAddress).HasMaxLength(42).IsRequired(false);

            builder
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("timezone('utc', now())")
                .IsRequired();

            builder
                .HasIndex(e => e.LinkedWalletAddress)
                .IsUnique()
                .HasFilter("\"LinkedWalletAddress\" IS NOT NULL");

            builder.HasIndex(e => e.NormalizedEmail).IsUnique();

            builder.HasIndex(e => e.NormalizedUserName).IsUnique();
        }
    }
}

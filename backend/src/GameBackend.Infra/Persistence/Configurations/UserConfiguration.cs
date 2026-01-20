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

            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.UserName).IsUnique();
            builder.Property(e => e.UserName).IsRequired().HasMaxLength(32);

            builder.HasIndex(e => e.Email).IsUnique();
            builder.Property(p => p.Email).IsRequired().HasMaxLength(254);

            builder
                .Property(e => e.Status)
                .HasConversion<int>()
                .HasDefaultValue(UserStatus.Unverified);

            builder.HasIndex(e => e.LinkedWalletAddress).IsUnique();
            builder.Property(e => e.LinkedWalletAddress).HasMaxLength(42).IsRequired(false);

            builder
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("now() at time zone 'utc'")
                .IsRequired();
        }
    }
}

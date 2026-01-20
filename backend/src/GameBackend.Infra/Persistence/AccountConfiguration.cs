using GameBackend.Core.Entities;
using GameBackend.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameBackend.Infra.Persistence
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasMaxLength(36).IsFixedLength();

            builder.HasIndex(e => e.Username).IsUnique();
            builder.Property(e => e.Username).IsRequired().HasMaxLength(32);

            builder.HasIndex(e => e.Email).IsUnique();
            builder.Property(p => p.Email).IsRequired().HasMaxLength(254);

            builder
                .Property(e => e.Status)
                .HasConversion<int>()
                .HasDefaultValue(AccountStatus.Unverified);

            builder
                .HasIndex(e => e.LinkedWalletAddress)
                .IsUnique()
                .HasFilter("\"LinkedWalletAddress\" IS NOT NULL");
            builder.Property(e => e.LinkedWalletAddress).HasMaxLength(42).IsRequired(false);

            builder.Property(e => e.CreatedAt).IsRequired();
        }
    }
}

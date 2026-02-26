using GameBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameBackend.Infra.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Token).IsRequired().HasMaxLength(200);

        builder.Property(rt => rt.UserId).IsRequired();

        builder.HasIndex(rt => rt.Token).IsUnique();

        builder.HasIndex(rt => rt.UserId);

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

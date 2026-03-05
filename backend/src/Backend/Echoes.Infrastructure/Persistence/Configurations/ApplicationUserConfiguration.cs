using Echoes.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Echoes.Infrastructure.Persistence.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("application_user");
            builder.Property(u => u.UserName).HasMaxLength(32).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(254).IsRequired();

            builder.HasIndex(u => u.NormalizedEmail).IsUnique().HasFilter("\"IsDeleted\" = false");

            builder
                .HasIndex(u => u.NormalizedUserName)
                .IsUnique()
                .HasFilter("\"IsDeleted\" = false");
        }
    }
}

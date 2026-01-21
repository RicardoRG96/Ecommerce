using Domain.Entities.Users;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Users
{
    internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Token)
                .HasMaxLength(200);

            builder.HasIndex(r => r.Token)
                .IsUnique();

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(r => r.UserId);
        }
    }
}

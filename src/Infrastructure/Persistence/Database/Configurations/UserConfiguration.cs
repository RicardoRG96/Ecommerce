using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(u => u.UserId);

            builder.HasIndex(u => u.Username)
                .IsUnique();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.UserId)
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Avatar)
                .HasMaxLength(250);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(u => u.DateOfBirth)
                .IsRequired(true);

            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20);

            // Relationships
            builder.HasMany(u => u.Addresses)
                .WithMany(a => a.Users)
                .UsingEntity<AddressUser>();
        }
    }
}

using Infrastructure.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Configurations
{
    public class UserModelConfiguration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UserModel> builder)
        {
           builder.HasKey(u => u.UserId);

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
                .IsRequired(false);

            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(u => u.UpdatedAt)
                .ValueGeneratedOnUpdate()
                .HasDefaultValueSql("NULL");

            // Relationships
            builder.HasMany(u => u.Addresses)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

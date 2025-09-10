using Infrastructure.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public class AddressModelConfiguration : IEntityTypeConfiguration<AddressModel>
    {
        public void Configure(EntityTypeBuilder<AddressModel> builder)
        {
            builder.HasKey(a => a.AddressId);

            builder.Property(a => a.AddressId)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(a => a.Number)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(a => a.Apartament)
                .HasMaxLength(50);

            builder.Property(a => a.Reference)
                .HasMaxLength(250);

            builder.Property(a => a.PostalCode)
                .HasMaxLength(20);

            builder.Property(a => a.IsDefault)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(a => a.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(a => a.UpdatedAt)
                .ValueGeneratedOnUpdate()
                .HasDefaultValueSql("NULL");

            // Relationships
            builder.HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Country)
                .WithMany(c => c.Addresses)
                .HasForeignKey(a => a.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Municipality)
                .WithMany(m => m.Addresses)
                .HasForeignKey(a => a.MunicipalityId)
                .OnDelete(DeleteBehavior.Restrict);

            // unique index to ensure a user can have only one default address
            builder.HasIndex(a => new { a.UserId, a.IsDefault })
                .IsUnique()
                .HasFilter("[IsDefault] = 1");
        }
    }
}

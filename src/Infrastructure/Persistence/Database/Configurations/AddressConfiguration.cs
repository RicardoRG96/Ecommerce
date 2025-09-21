using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");

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

            // Relationships
            builder.HasMany(a => a.Users)
                .WithMany(u => u.Addresses)
                .UsingEntity<AddressUser>();

            builder.HasOne(a => a.Country)
                .WithMany(c => c.Addresses)
                .HasForeignKey(a => a.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Municipality)
                .WithMany(m => m.Addresses)
                .HasForeignKey(a => a.MunicipalityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

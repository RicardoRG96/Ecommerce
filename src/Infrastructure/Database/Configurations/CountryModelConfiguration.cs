using Infrastructure.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public class CountryModelConfiguration : IEntityTypeConfiguration<CountryModel>
    {
        public void Configure(EntityTypeBuilder<CountryModel> builder)
        {
            builder.HasKey(c => c.CountryId);

            builder.Property(c => c.CountryId)
                .ValueGeneratedOnAdd();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.UpdatedAt)
                .ValueGeneratedOnUpdate()
                .HasDefaultValueSql("NULL");

            // Relationships
            builder.HasMany(c => c.Addresses)
                .WithOne(a => a.Country)
                .HasForeignKey(a => a.CountryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

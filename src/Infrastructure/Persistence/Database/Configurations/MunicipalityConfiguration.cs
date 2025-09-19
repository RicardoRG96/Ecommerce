using Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations
{
    public class MunicipalityConfiguration : IEntityTypeConfiguration<Municipality>
    {
        public void Configure(EntityTypeBuilder<Municipality> builder)
        {
            builder.ToTable("Municipality");

            builder.HasKey(m => m.MunicipalityId);

            builder.Property(m => m.MunicipalityId)
                .ValueGeneratedOnAdd();

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(120);

            // Relationships
            builder.HasMany(m => m.Addresses)
                .WithOne(a => a.Municipality)
                .HasForeignKey(a => a.MunicipalityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

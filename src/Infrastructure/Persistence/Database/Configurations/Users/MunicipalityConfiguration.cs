using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Users
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
                .HasForeignKey(m => m.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Region)
                .WithMany(r => r.Municipalities)
                .HasForeignKey(m => m.RegionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(m => m.Region).AutoInclude();
        }
    }
}

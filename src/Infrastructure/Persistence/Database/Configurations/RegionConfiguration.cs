using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations
{
    public class RegionConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.ToTable("Region");

            builder.HasKey(r => r.RegionId);

            builder.Property(r => r.RegionId)
                .ValueGeneratedOnAdd();

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(120);

            // Relationships
            builder.HasMany(r => r.Municipalities)
                .WithOne(m => m.Region)
                .HasForeignKey(m => m.RegionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

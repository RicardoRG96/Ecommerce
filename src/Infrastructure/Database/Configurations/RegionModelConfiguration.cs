using Infrastructure.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public class RegionModelConfiguration : IEntityTypeConfiguration<RegionModel>
    {
        public void Configure(EntityTypeBuilder<RegionModel> builder)
        {
            builder.HasKey(r => r.RegionId);

            builder.Property(r => r.RegionId)
                .ValueGeneratedOnAdd();

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(120);

            builder.Property(r => r.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(r => r.UpdatedAt)
                .ValueGeneratedOnUpdate()
                .HasDefaultValueSql("NULL");

            // Relationships
            builder.HasMany(r => r.Municipalities)
                .WithOne(m => m.Region)
                .HasForeignKey(m => m.RegionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

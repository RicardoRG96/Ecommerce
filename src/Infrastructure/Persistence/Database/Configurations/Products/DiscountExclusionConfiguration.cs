using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Products
{
    public class DiscountExclusionConfiguration : IEntityTypeConfiguration<DiscountExclusion>
    {
        public void Configure(EntityTypeBuilder<DiscountExclusion> builder)
        {
            builder.ToTable("DiscountExclusion");

            builder.HasKey(de => new { de.DiscountId, de.ExcludedDiscountId });

            builder.Property(de => de.DiscountId)
                .IsRequired();

            builder.Property(de => de.ExcludedDiscountId)
                .IsRequired();

            //Relationships
            builder.HasOne(de => de.Discount)
                .WithMany(d => d.DiscountExclusions)
                .HasForeignKey(de => de.DiscountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

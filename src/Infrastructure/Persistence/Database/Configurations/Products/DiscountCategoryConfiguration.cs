using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Products
{
    public class DiscountCategoryConfiguration : IEntityTypeConfiguration<DiscountCategory>
    {
        public void Configure(EntityTypeBuilder<DiscountCategory> builder)
        {
            builder.ToTable("DiscountCategory");

            builder.HasKey(dc => new { dc.DiscountId, dc.CategoryId });

            builder.Property(dc => dc.DiscountId)
                .IsRequired();

            builder.Property(dc => dc.CategoryId)
                .IsRequired();

            builder.HasOne(dc => dc.Discount)
                .WithMany(d => d.DiscountCategories)
                .HasForeignKey(dc => dc.DiscountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(dc => dc.Category)
                .WithMany(c => c.DiscountCategories)
                .HasForeignKey(dc => dc.CategoryId);
        }
    }
}

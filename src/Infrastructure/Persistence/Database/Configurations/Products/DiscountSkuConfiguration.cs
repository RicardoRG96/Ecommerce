using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Products
{
    public class DiscountSkuConfiguration : IEntityTypeConfiguration<DiscountSku>
    {
        public void Configure(EntityTypeBuilder<DiscountSku> builder)
        {
            builder.ToTable("DiscountSku");

            builder.HasKey(ds => new { ds.DiscountId, ds.ProductSkuId });

            builder.Property(ds => ds.DiscountId)
                .IsRequired();

            builder.Property(ds => ds.ProductSkuId)
                .IsRequired();

            builder.HasOne(ds => ds.Discount)
                .WithMany(d => d.DiscountSkus)
                .HasForeignKey(ds => ds.DiscountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ds => ds.ProductSku)
                .WithMany(s => s.DiscountSkus)
                .HasForeignKey(ds => ds.ProductSkuId);
        }
    }
}

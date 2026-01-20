using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Products
{
    public class DiscountProductConfiguration : IEntityTypeConfiguration<DiscountProduct>
    {
        public void Configure(EntityTypeBuilder<DiscountProduct> builder)
        {
            builder.ToTable("DiscountProduct");

            builder.HasKey(dp => new { dp.DiscountId,  dp.ProductId });

            builder.Property(dp => dp.DiscountId)
                .IsRequired();

            builder.Property(dp => dp.ProductId)
                .IsRequired();

            builder.HasOne(dp => dp.Discount)
                .WithMany(d => d.DiscountProducts)
                .HasForeignKey(dp => dp.DiscountId);

            builder.HasOne(dp => dp.Product)
                .WithMany(p => p.DiscountProducts)
                .HasForeignKey(dp => dp.ProductId);
        }
    }
}

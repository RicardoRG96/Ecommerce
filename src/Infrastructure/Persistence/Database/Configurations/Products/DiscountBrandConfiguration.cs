using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Products
{
    public class DiscountBrandConfiguration : IEntityTypeConfiguration<DiscountBrand>
    {
        public void Configure(EntityTypeBuilder<DiscountBrand> builder)
        {
            builder.ToTable("DiscountBrand");

            builder.HasKey(db => new { db.DiscountId, db.BrandId });

            builder.Property(db => db.DiscountId)
                .IsRequired();

            builder.Property(db => db.BrandId)
                .IsRequired();

            builder.HasOne(db => db.Discount)
                .WithMany(d => d.DiscountBrands)
                .HasForeignKey(db => db.DiscountId);

            builder.HasOne(db => db.Brand)
                .WithMany(b => b.DiscountBrands)
                .HasForeignKey(db => db.BrandId);
        }
    }
}

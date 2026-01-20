using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Products
{
    public class ProductSkuConfiguration : IEntityTypeConfiguration<ProductSku>
    {
        public void Configure(EntityTypeBuilder<ProductSku> builder)
        {
            builder.ToTable("ProductSku", t =>
            {
                t.HasCheckConstraint(
                    "CK_ProductSku_Price",
                    "[Price] >= 0");

                t.HasCheckConstraint(
                    "CK_ProductSku_ComparedAtPrice",
                    "[ComparedAtPrice] IS NULL OR [ComparedAtPrice] >= [Price]");
            });

            builder.HasKey(ps => ps.Id);

            builder.Property(ps => ps.Id)
                .ValueGeneratedOnAdd();

            builder.Property(ps => ps.ProductId)
                .IsRequired();

            builder.Property(ps => ps.SkuCode)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(ps => ps.BarCode)
                .HasMaxLength(50);

            builder.Property(ps => ps.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(ps => ps.ComparedAtPrice)
                .HasPrecision(18, 2);

            builder.Property(ps => ps.Cost)
                .HasPrecision(18, 2);

            builder.Property(ps => ps.Weight)
                .HasPrecision(10, 3);

            builder.Property(ps => ps.Length)
                .HasPrecision(10, 2);

            builder.Property(ps => ps.Width)
                .HasPrecision(10, 2);

            builder.Property(ps => ps.Height)
                .HasPrecision(10, 2);

            builder.Property(ps => ps.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(ps => ps.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            //Relationships
            builder.HasOne(ps => ps.Product)
                .WithMany(p => p.ProductSkus)
                .HasForeignKey(ps => ps.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(ps => ps.ProductGalleries)
                .WithOne(pg => pg.ProductSku)
                .HasForeignKey(pg => pg.ProductSkuId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(ps => ps.ProductAttributeValues)
                .WithOne(pav => pav.ProductSku)
                .HasForeignKey(pav => pav.ProductSkuId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(ps => ps.ProductSkuStocks)
                .WithOne(pss => pss.ProductSku)
                .HasForeignKey(pss => pss.ProductSkuId)
                .OnDelete(DeleteBehavior.Cascade); 

            //Indexes
            builder.HasIndex(ps => ps.SkuCode)
                .IsUnique();
        }
    }
}

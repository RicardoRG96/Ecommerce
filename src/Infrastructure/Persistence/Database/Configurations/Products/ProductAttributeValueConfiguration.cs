using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Products
{
    public class ProductAttributeValueConfiguration : IEntityTypeConfiguration<ProductAttributeValue>
    {
        public void Configure(EntityTypeBuilder<ProductAttributeValue> builder)
        {
            builder.ToTable("ProductAttributeValue");

            builder.HasKey(pav => pav.Id);

            builder.Property(pav => pav.Id)
                .ValueGeneratedOnAdd();

            builder.Property(pav => pav.ProductSkuId)
                .IsRequired();

            builder.Property(pav => pav.AttributeValueId)
                .IsRequired();

            // relationships
            builder.HasOne(pav => pav.ProductSku)
                .WithMany(p => p.ProductAttributeValues)
                .HasForeignKey(pav => pav.ProductSkuId);

            builder.HasOne(pav => pav.AttributeValue)
                .WithMany(av => av.ProductAttributeValues)
                .HasForeignKey(pav => pav.AttributeValueId);

            // indexes
            builder.HasIndex(pav => new { pav.ProductSkuId, pav.AttributeValueId })
                .HasDatabaseName("UX_ProductAttributeValue_Sku_Attribute")
                .IsUnique();

            builder.HasIndex(pav => pav.ProductSkuId)
                .HasDatabaseName("IX_PAV_ProductSku");

            builder.HasIndex(pav => pav.AttributeValueId)
                .HasDatabaseName("IX_PAV_AttributeValue");
        }
    }
}

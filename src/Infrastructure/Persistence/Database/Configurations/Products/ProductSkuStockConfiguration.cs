using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Products
{
    public class ProductSkuStockConfiguration : IEntityTypeConfiguration<ProductSkuStock>
    {
        public void Configure(EntityTypeBuilder<ProductSkuStock> builder)
        {
            builder.ToTable("ProductSkuStock", t =>
            {
                t.HasCheckConstraint(
                    "CK_ProductSkuStock_Stock",
                    "[Stock] >= 0");

                t.HasCheckConstraint(
                    "CK_ProductSkuStock_Reserved",
                    "[ReservedStock] >= 0");

                t.HasCheckConstraint(
                    "CK_ProductSkuStock_Min",
                    "[MinStock] >= 0");
            });

            builder.HasKey(pss => pss.Id);

            builder.Property(pss => pss.Id)
                .ValueGeneratedOnAdd();

            builder.Property(pss => pss.ProductSkuId)
                .IsRequired();

            builder.Property(pss => pss.WarehouseId)
                .IsRequired();

            builder.Property(pss => pss.Stock)
                .IsRequired();

            builder.Property(pss => pss.ReservedStock)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(pss => pss.MinStock)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(pss => pss.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(pss => pss.RowVersion)
                .IsRequired();

            //Relationships
            builder.HasOne(pss => pss.ProductSku)
                .WithMany(ps => ps.ProductSkuStocks)
                .HasForeignKey(pss => pss.ProductSkuId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pss => pss.Warehouse)
                .WithMany(w => w.ProductSkuStocks)
                .HasForeignKey(pss => pss.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            //Indexes
            builder.HasIndex(pss => new { pss.ProductSkuId, pss.WarehouseId })
                .IsUnique();
        }
    }
}

using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Products
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable("Warehouse");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Id)
                .ValueGeneratedOnAdd();

            builder.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(w => w.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(w => w.Address)
                .HasMaxLength(200);

            builder.Property(w => w.City)
                .HasMaxLength(100);

            builder.Property(w => w.CountryCode)
                .IsRequired()
                .HasMaxLength(2);

            builder.Property(w => w.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(w => w.IsDefault)
                .IsRequired()
                .HasDefaultValue(false);

            //Relationships
            builder.HasMany(w => w.ProductSkuStocks)
                .WithOne(pss => pss.Warehouse)
                .HasForeignKey(pss => pss.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            //Indexes
            builder.HasIndex(w => w.Code)
                .IsUnique();
        }
    }
}

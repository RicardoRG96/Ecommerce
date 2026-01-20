using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Products
{
    public class ProductGalleryConfiguration : IEntityTypeConfiguration<ProductGallery>
    {
        public void Configure(EntityTypeBuilder<ProductGallery> builder)
        {
            builder.ToTable("ProductGallery");

            builder.HasKey(pg => pg.Id);

            builder.Property(pg => pg.Id)
                .ValueGeneratedOnAdd();

            builder.Property(pg => pg.ProductId)
                .IsRequired();

            builder.Property(pg => pg.ProductSkuId)
                .IsRequired();

            builder.Property(pg => pg.MediaUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(pg => pg.MediaType)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(pg => pg.MimeType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(pg => pg.IsPrimary)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(pg => pg.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(pg => pg.AltText)
                .HasMaxLength(200);

            //Relationships
            builder.HasOne(pg => pg.Product)
                .WithMany(p => p.ProductGalleries)
                .HasForeignKey(pg => pg.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pg => pg.ProductSku)
                .WithMany(ps => ps.ProductGalleries)
                .HasForeignKey(pg => pg.ProductSkuId)
                .OnDelete(DeleteBehavior.Restrict);

            // unique index to ensure a product can have only one primary gallery
            builder.HasIndex(pg => new { pg.ProductId, pg.IsPrimary })
                .IsUnique()
                .HasFilter("[IsPrimary] = 1");
        }
    }
}

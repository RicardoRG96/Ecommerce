using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Products
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Slug)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.ShortDescription)
                .HasMaxLength(500);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.CategoryId)
                .IsRequired();

            builder.Property(p => p.ProductTaxCategoryId)
                .IsRequired();

            builder.Property(p => p.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(p => p.IsDigital)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(p => p.MetaTitle)
                .HasMaxLength(200);

            builder.Property(p => p.MetaDescription)
                .HasMaxLength(500);

            builder.Property(p => p.MetaKeywords)
                .HasMaxLength(500);

            //Relationships
            builder.HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.ProductTaxCategory)
                .WithMany(ptc => ptc.Products)
                .HasForeignKey(p => p.ProductTaxCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

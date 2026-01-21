using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Products
{
    public class ProductTaxCategoryConfiguration : IEntityTypeConfiguration<ProductTaxCategory>
    {
        public void Configure(EntityTypeBuilder<ProductTaxCategory> builder)
        {
            builder.ToTable("ProductTaxCategory");

            builder.HasKey(ptc => ptc.Id);

            builder.Property(ptc => ptc.Id)
                .ValueGeneratedOnAdd();

            builder.Property(ptc => ptc.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(ptc => ptc.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(ptc => ptc.Description)
                .HasMaxLength(200);

            builder.Property(ptc => ptc.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            //Relationships
            builder.HasMany(ptc => ptc.ProductTaxCategoryRates)
                .WithOne(ptcr => ptcr.ProductTaxCategory)
                .HasForeignKey(ptcr => ptcr.ProductTaxCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(ptc => ptc.Products)
                .WithOne(p => p.ProductTaxCategory)
                .HasForeignKey(p => p.ProductTaxCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            //Indexes
            builder.HasIndex(ptc => ptc.Code)
                .IsUnique();
        }
    }
}

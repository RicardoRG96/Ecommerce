using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Products
{
    public class ProductTaxCategoryRateConfiguration : IEntityTypeConfiguration<ProductTaxCategoryRate>
    {
        public void Configure(EntityTypeBuilder<ProductTaxCategoryRate> builder)
        {
            builder.ToTable("ProductTaxCategoryRate");

            builder.HasKey(ptcr => new { ptcr.ProductTaxCategoryId, ptcr.TaxRateId });

            builder.Property(ptcr => ptcr.ProductTaxCategoryId)
                .IsRequired();

            builder.Property(ptcr => ptcr.TaxRateId)
                .IsRequired();

            //Relationships
            builder.HasOne(ptcr => ptcr.ProductTaxCategory)
                .WithMany(ptc => ptc.ProductTaxCategoryRates)
                .HasForeignKey(ptcr => ptcr.ProductTaxCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ptcr => ptcr.TaxRate)
                .WithMany(tc => tc.ProductTaxCategoryRates)
                .HasForeignKey(ptcr => ptcr.ProductTaxCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Products
{
    public class TaxRateConfiguration : IEntityTypeConfiguration<TaxRate>
    {
        public void Configure(EntityTypeBuilder<TaxRate> builder)
        {
            builder.ToTable("TaxRate", t =>
            {
                t.HasCheckConstraint(
                    "CK_TaxRate_Rate",
                    "[Rate] >= 0 AND [Rate] <= 1");
            });

            builder.HasKey(tr => tr.Id);

            builder.Property(tr => tr.Id)
                .ValueGeneratedOnAdd();

            builder.Property(tr => tr.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(tr => tr.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(tr => tr.CountryCode)
                .IsRequired()
                .HasMaxLength(2);

            builder.Property(tr => tr.TaxType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(tr => tr.Rate)
                .IsRequired()
                .HasPrecision(5, 4);

            builder.Property(tr => tr.IsCompound)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(tr => tr.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(tr => tr.ValidFrom)
                .IsRequired();

            //Relationships
            builder.HasMany(tr => tr.ProductTaxCategoryRates)
                .WithOne(ptcr => ptcr.TaxRate)
                .HasForeignKey(ptcr => ptcr.TaxRateId)
                .OnDelete(DeleteBehavior.Cascade);

            //Indexes
            builder.HasIndex(tr => tr.Code)
                .IsUnique();
        }
    }
}

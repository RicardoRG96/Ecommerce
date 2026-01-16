using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Products
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brand");

            builder.HasKey(x => x.BrandId);

            builder.Property(b => b.BrandId)
                .ValueGeneratedOnAdd();

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(120);

            builder.Property(b => b.Description)
                .IsRequired()
                .HasMaxLength(120);

            builder.Property(b => b.LogoUrl)
                .IsRequired()
                .HasMaxLength(120);
        }
    }
}

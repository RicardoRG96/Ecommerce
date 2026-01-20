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

            builder.HasKey(x => x.Id);

            builder.Property(b => b.Id)
                .ValueGeneratedOnAdd();

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(b => b.Slug)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(b => b.LogoUrl)
                .HasMaxLength(500);

            builder.Property(b => b.BannerUrl)
                .HasMaxLength(500);

            builder.Property(b => b.WebsiteUrl)
                .HasMaxLength(500);

            builder.Property(b => b.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(b => b.IsFeatured)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(b => b.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(b => b.MetaTitle)
                .HasDefaultValue(160);

            builder.Property(b => b.MetaDescription)
                .HasDefaultValue(300);

            builder.Property(b => b.MetaKeywords)
                .HasDefaultValue(500);

            //Indexes
            builder.HasIndex(b => b.Slug)
                .IsUnique();

            builder.HasIndex(b => b.IsActive)
                .HasDatabaseName("IX_Brand_IsActive");

            builder.HasIndex(b => b.DisplayOrder)
                .HasDatabaseName("IX_Brand_Featured");
        }
    }
}

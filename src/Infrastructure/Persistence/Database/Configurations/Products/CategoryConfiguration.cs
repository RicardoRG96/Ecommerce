using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Products
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.Slug)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.ImageUrl)
                .HasMaxLength(500);

            builder.Property(c => c.Icon)
                .HasMaxLength(100);

            builder.Property(c => c.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(c => c.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(c => c.IsVisibleInMenu)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(c => c.MetaTitle)
                .HasMaxLength(160);

            builder.Property(c => c.MetaDescription)
                .HasMaxLength(300);

            builder.Property(c => c.MetaKeywords)
                .HasMaxLength(500);

            //Relationships
            builder.HasOne(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            //Indexes
            builder.HasIndex(c => c.Slug)
                .IsUnique();

            builder.HasIndex(c => c.ParentId)
                .HasDatabaseName("IX_Category_ParentId");

            builder.HasIndex(c => c.IsActive)
                .HasDatabaseName("IX_Category_IsActive");

            builder.HasIndex(c => new { c.IsVisibleInMenu, c.DisplayOrder })
                .HasDatabaseName("IX_Category_Menu");
        }
    }
}

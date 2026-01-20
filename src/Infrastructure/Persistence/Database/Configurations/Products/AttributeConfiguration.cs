using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Products
{
    public class AttributeConfiguration : IEntityTypeConfiguration<Domain.Entities.Products.Attribute>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Products.Attribute> builder)
        {
            builder.ToTable("Attribute");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Description)
                .HasMaxLength(255);

            builder.Property(a => a.DataType)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(a => a.IsVariant)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(a => a.IsFilterable)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(a => a.IsRequired)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(a => a.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(a => a.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Relationships
            builder.HasMany(a => a.AttributeValues)
                .WithOne(av => av.Attribute)
                .HasForeignKey(av => av.AttributeId)
                .OnDelete(DeleteBehavior.Cascade);

            // indexes
            builder.HasIndex(a => a.Code)
                .IsUnique();
        }
    }
}

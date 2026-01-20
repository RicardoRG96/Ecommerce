using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Products
{
    public class AttributeValueConfiguration : IEntityTypeConfiguration<AttributeValue>
    {
        public void Configure(EntityTypeBuilder<AttributeValue> builder)
        {
            builder.ToTable("AttributeValue");

            builder.HasKey(av => av.Id);

            builder.Property(av => av.Id)
                .ValueGeneratedOnAdd();

            builder.Property(av => av.AttributeId)
                .IsRequired();

            builder.Property(av => av.Value)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(av => av.NormalizedValue)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(av => av.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(av => av.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Relationships
            builder.HasOne(av => av.Attribute)
                .WithMany(a => a.AttributeValues)
                .HasForeignKey(av => av.AttributeId)
                .OnDelete(DeleteBehavior.Restrict);

            // indexes
            builder.HasIndex(av => new { av.AttributeId, av.NormalizedValue })
                .HasDatabaseName("UX_AttributeValue_Attribute_Value")
                .IsUnique();

            builder.HasIndex(av => av.AttributeId)
                .HasDatabaseName("IX_AttributeValue_AttributeId");
        }
    }
}

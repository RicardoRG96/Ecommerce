using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Products
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.ToTable("Discount");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                .ValueGeneratedOnAdd();

            builder.Property(d => d.Code)
                .HasMaxLength(50);

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(d => d.Description)
                .HasMaxLength(150);

            builder.Property(d => d.DiscountType)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(d => d.Value)
                .IsRequired();

            builder.Property(d => d.IsStackable)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(d => d.Priority)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(d => d.StartsAt)
                .IsRequired();

            builder.Property(d => d.EndsAt)
                .IsRequired();

            builder.Property(d => d.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            //Relationships
            builder.HasMany(d => d.DiscountCodes)
                .WithOne(dc => dc.Discount)
                .HasForeignKey(dc => dc.DiscountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.DiscountExclusions)
                .WithOne(de => de.Discount)
                .HasForeignKey(de => de.DiscountId)
                .OnDelete(DeleteBehavior.Cascade);

            //Indexes
            builder.HasIndex(d => d.Code)
                .IsUnique()
                .HasDatabaseName("UX_Discount_Code");
        }
    }
}

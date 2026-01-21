using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Products
{
    public class DiscountCodeConfiguration : IEntityTypeConfiguration<DiscountCode>
    {
        public void Configure(EntityTypeBuilder<DiscountCode> builder)
        {
            builder.ToTable("DiscountCode");

            builder.HasKey(dc => dc.Id);

            builder.Property(dc => dc.Id)
                .ValueGeneratedOnAdd();

            builder.Property(dc => dc.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(dc => dc.DiscountId)
                .IsRequired();

            builder.Property(dc => dc.IsSingleUse)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(dc => dc.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(dc => dc.StartsAt)
                .IsRequired();

            builder.Property(dc => dc.EndsAt)
                .IsRequired();

            //Relationships
            builder.HasOne(dc => dc.Discount)
                .WithMany(d => d.DiscountCodes)
                .HasForeignKey(dc => dc.DiscountId)
                .OnDelete(DeleteBehavior.Restrict);

            //Indexes
            builder.HasIndex(dc => dc.Code)
                .IsUnique()
                .HasDatabaseName("UX_DiscountCode_Code");
        }
    }
}

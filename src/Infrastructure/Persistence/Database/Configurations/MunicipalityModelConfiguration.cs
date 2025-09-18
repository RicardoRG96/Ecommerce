using Infrastructure.Persistence.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations
{
    public class MunicipalityModelConfiguration : IEntityTypeConfiguration<MunicipalityModel>
    {
        public void Configure(EntityTypeBuilder<MunicipalityModel> builder)
        {
            builder.ToTable("Municipality");

            builder.HasKey(m => m.MunicipalityId);

            builder.Property(m => m.MunicipalityId)
                .ValueGeneratedOnAdd();

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(120);

            builder.Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(m => m.UpdatedAt)
                .ValueGeneratedOnUpdate()
                .HasDefaultValueSql("NULL");

            // Relationships
            builder.HasMany(m => m.Addresses)
                .WithOne(a => a.Municipality)
                .HasForeignKey(a => a.MunicipalityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

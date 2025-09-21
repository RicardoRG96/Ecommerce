using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations
{
    public class AddressUserConfiguration : IEntityTypeConfiguration<AddressUser>
    {
        public void Configure(EntityTypeBuilder<AddressUser> builder)
        {
            builder.HasKey(au => new { au.UserId, au.AddressId });

            builder.Property(au => au.IsDefault)
                .IsRequired()
                .HasDefaultValue(false);

            // unique index to ensure a user can have only one default address
            builder.HasIndex(au => new { au.UserId, au.IsDefault })
                .IsUnique()
                .HasFilter("[IsDefault] = 1");
        }
    }
}

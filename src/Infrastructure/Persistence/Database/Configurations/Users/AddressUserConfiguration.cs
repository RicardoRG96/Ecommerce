using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Database.Configurations.Users
{
    public class AddressUserConfiguration : IEntityTypeConfiguration<AddressUser>
    {
        public void Configure(EntityTypeBuilder<AddressUser> builder)
        {
            builder.ToTable("AddressUser");

            builder.HasKey(au => new { au.ApplicationUserId, au.AddressId });

            builder.HasOne(x => x.Address)
                .WithMany(a => a.AddressUsers)
                .HasForeignKey(x => x.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(au => au.IsDefault)
                .IsRequired()
                .ValueGeneratedNever();

            // unique index to ensure a user can have only one default address
            builder.HasIndex(au => new { au.ApplicationUserId, au.IsDefault })
                .IsUnique()
                .HasFilter("[IsDefault] = 1");
        }
    }
}

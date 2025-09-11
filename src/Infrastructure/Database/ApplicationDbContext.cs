using Infrastructure.Database.Configurations;
using Infrastructure.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<RegionModel> Regions { get; set; }
        public DbSet<MunicipalityModel> Municipalities { get; set; }
        public DbSet<CountryModel> Countries { get; set; }
        public DbSet<AddressModel> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserModelConfiguration());
            modelBuilder.ApplyConfiguration(new RegionModelConfiguration());
            modelBuilder.ApplyConfiguration(new MunicipalityModelConfiguration());
            modelBuilder.ApplyConfiguration(new CountryModelConfiguration());
            modelBuilder.ApplyConfiguration(new AddressModelConfiguration());
        }
    }
}

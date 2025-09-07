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
            modelBuilder.Entity<UserModel>().ToTable("User");
            modelBuilder.Entity<RegionModel>().ToTable("Region");
            modelBuilder.Entity<MunicipalityModel>().ToTable("Municipality");
            modelBuilder.Entity<CountryModel>().ToTable("Country");
            modelBuilder.Entity<AddressModel>().ToTable("Address");

            //modelbuilder.entity<addressmodel>()
            //    .hasone(a => a.user)
            //    .withmany()
            //    .hasforeignkey(a => a.userid)
            //    .ondelete(deletebehavior.cascade);
        }
    }
}

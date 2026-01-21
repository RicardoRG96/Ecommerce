using Domain.Entities.Products;
using Domain.Entities.Users;
using Infrastructure.Identity;
using Infrastructure.Persistence.Database.Configurations.Products;
using Infrastructure.Persistence.Database.Configurations.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Database
{
    public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public new DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressUser> AddressUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Domain.Entities.Products.Attribute> Attributes { get; set; }
        public DbSet<AttributeValue> AttributeValues { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<DiscountBrand> DiscountBrands { get; set; }
        public DbSet<DiscountCategory> DiscountCategories { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<DiscountExclusion> DiscountExclusions { get; set; }
        public DbSet<DiscountProduct> DiscountProducts { get; set; }
        public DbSet<DiscountSku> DiscountSkus { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
        public DbSet<ProductGallery> ProductGalleries { get; set; }
        public DbSet<ProductSku> ProductSkus { get; set; }
        public DbSet<ProductSkuStock> ProductSkuStocks { get; set; }
        public DbSet<ProductTaxCategory> ProductTaxCategories { get; set; }
        public DbSet<ProductTaxCategoryRate> ProductTaxCategoryRates { get; set; }
        public DbSet<TaxRate> TaxRates { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Users configurations
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RegionConfiguration());
            modelBuilder.ApplyConfiguration(new MunicipalityConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new AddressConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            modelBuilder.ApplyConfiguration(new AddressUserConfiguration());

            //Products configurations
            modelBuilder.ApplyConfiguration(new AttributeConfiguration());
            modelBuilder.ApplyConfiguration(new AttributeValueConfiguration());
            modelBuilder.ApplyConfiguration(new BrandConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new DiscountConfiguration());
            modelBuilder.ApplyConfiguration(new DiscountBrandConfiguration());
            modelBuilder.ApplyConfiguration(new DiscountCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new DiscountCodeConfiguration());
            modelBuilder.ApplyConfiguration(new DiscountExclusionConfiguration());
            modelBuilder.ApplyConfiguration(new DiscountProductConfiguration());
            modelBuilder.ApplyConfiguration(new DiscountSkuConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductAttributeValueConfiguration());
            modelBuilder.ApplyConfiguration(new ProductGalleryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductSkuConfiguration());
            modelBuilder.ApplyConfiguration(new ProductSkuStockConfiguration());
            modelBuilder.ApplyConfiguration(new ProductTaxCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductTaxCategoryRateConfiguration());
            modelBuilder.ApplyConfiguration(new TaxRateConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseConfiguration());
        }
    }
}

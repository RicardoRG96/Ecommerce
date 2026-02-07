using Algolia.Search.Clients;
using Application.Abstractions.Authentication;
using Application.Abstractions.Common;
using Application.Abstractions.Data.Repositories;
using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Search;
using Application.Abstractions.Storage;
using Azure.Storage.Blobs;
using Infrastructure.Authentication;
using Infrastructure.Authorization;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Database;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.Repositories.Products;
using Infrastructure.Persistence.Repositories.Users;
using Infrastructure.Search;
using Infrastructure.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration) =>
            services
                .AddDatabase(configuration)
                .AddRepositories()
                .AddUnitOfWork()
                .AddIdentity()
                .AddAuthenticationInternal(configuration)
                .AddAuthorizationInternal()
                .AddSearch(configuration)
                .AddFileStorage(configuration);

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("EcommerceDb");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IMunicipalityRepository, MunicipalityRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IAddressUserRepository, AddressUserRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductTaxCategoryRepository, ProductTaxCategoryRepository>();
            services.AddScoped<IAttributeRepository, AttributeRepository>();
            services.AddScoped<IAttributeValueRepository, AttributeValueRepository>();
            services.AddScoped<IProductSkuRepository, ProductSkuRepository>();
            services.AddScoped<IProductAttributeValueRepository, ProductAttributeValueRepository>();

            return services;
        }

        private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        private static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole<long>>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders(); // for email confirmation

            services.AddTransient<IIdentityService, IdentityService>();

            return services;
        }

        private static IServiceCollection AddAuthenticationInternal(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)),
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();
            services.AddSingleton<ITokenProvider, TokenProvider>();

            return services;
        }

        private static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
        {
            services.AddAuthorization();

            services.AddScoped<PermissionProvider>();

            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

            return services;
        }

        private static IServiceCollection AddSearch(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<ISearchClient, SearchClient>(sp =>
            {
                var appId = configuration["Algolia:ApplicationId"];
                var apiKey = configuration["Algolia:AdminApiKey"];

                return new SearchClient(appId, apiKey);
            });

            services.AddScoped<ISearchService, AlgoliaSearchService>();

            services.AddHostedService<AlgoliaIndexInitializer>();

            return services;
        }

        private static IServiceCollection AddFileStorage(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<BlobStorageOptions>(
                configuration.GetSection(BlobStorageOptions.SectionName));

            services.AddSingleton(sp =>
            {
                string connectionString = configuration[$"{BlobStorageOptions.SectionName}:ConnectionString)"]!;
                string containerName = configuration[$"{BlobStorageOptions.SectionName}:ContainerName)"]!;

                BlobServiceClient blobServiceClient = new(connectionString);

                return blobServiceClient.GetBlobContainerClient(containerName);
            });

            services.AddScoped<IFileStorageService, AzureFileStorageService>();

            return services;
        }
    }
}

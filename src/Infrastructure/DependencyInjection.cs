using Application.Abstractions.Common;
using Application.Abstractions.Data.Repositories;
using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Database;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.Repositories.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                .AddIdentity();

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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IMunicipalityRepository, MunicipalityRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();

            return services;
        }

        private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders(); // for email confirmation

            services.AddTransient<IIdentityService, IdentityService>();

            return services;
        }
    }
}

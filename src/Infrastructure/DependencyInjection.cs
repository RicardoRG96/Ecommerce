using Infrastructure.Database;
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
                /* in a near future, we will have more services to add */
                //.AddServices()
                .AddDatabase(configuration);
                //.AddHealthChecks(configuration);

        // Placeholder for adding more services in the future

        //public static IServiceCollection AddServices(this IServiceCollection services)
        //{
        //    // add transient, scoped, or singleton services here
        //    return services;
        //}

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("EcommerceDb");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}

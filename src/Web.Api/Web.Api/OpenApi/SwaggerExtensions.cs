using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Web.Api.OpenApi
{
    /// <summary>
    /// Provides extension methods for configuring Swagger/OpenAPI services.
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Adds and configures Swagger generation services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection to add Swagger services to.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Ecommerce API"
                });

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }
    }
}

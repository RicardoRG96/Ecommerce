using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
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
            services.AddTransient<IConfigureNamedOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(c =>
            {
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }
    }
}

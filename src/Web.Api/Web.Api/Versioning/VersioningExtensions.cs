using Asp.Versioning;

namespace Web.Api.Versioning
{
    /// <summary>
    /// Provides extension methods for configuring API versioning services.
    /// </summary>
    public static class VersioningExtensions
    {
        /// <summary>
        /// Adds API versioning and related services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection to add versioning to.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            services.AddEndpointsApiExplorer();

            return services;
        }
    }
}

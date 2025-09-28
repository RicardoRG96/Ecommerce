using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Web.Api.OpenApi
{
    /// <summary>
    /// Configures Swagger generation options for API versioning.
    /// </summary>
    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="IApiVersionDescriptionProvider"/> class.
        /// </summary>
        /// <param name="provider">The API version description provider.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Configures the specified <see cref="SwaggerGenOptions"/>.
        /// </summary>
        /// <param name="options">The Swagger generation options to configure.</param>
        public void Configure(SwaggerGenOptions options)
        {
            // Add a Swagger document for each discovered API Version
            // Note: You might choose to skip or document deprecated API Versions differently
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        public void Configure(string? name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        /// <summary>
        /// Creates an <see cref="OpenApiInfo"/> instance for the specified API version description.
        /// </summary>
        /// <param name="description">The API version description.</param>
        /// <returns>An <see cref="OpenApiInfo"/> object containing information for the API version.</returns>
        public static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            OpenApiInfo info = new()
            {
                Version = description.ApiVersion.ToString(),
                Title = $"Ecommerce.Api v{description.ApiVersion}"
            };

            return info;
        }
    }
}

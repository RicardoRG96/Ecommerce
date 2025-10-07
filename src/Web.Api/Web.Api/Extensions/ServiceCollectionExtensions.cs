using Microsoft.OpenApi.Models;

namespace Web.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGenWithAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(o =>
            {
                o.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));

                OpenApiSecurityScheme securityScheme = new()
                {
                    Name = "JWT Authentication",
                    Description = "Enter your JWT token in this field",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    //Scheme = JwtBearerDefaults.AuthenticationScheme, // here it will change to the 'Identity' config
                    BearerFormat = "JWT"
                };

                // here it will change to the 'Identity' config
                //o.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

                // here it will change to the 'Identity' config
                //OpenApiSecurityRequirement securityRequirement = new()
                //{
                //    {
                //        new OpenApiSecurityRequirement
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = JwtBearerDefaults.AuthenticationScheme
                //            }

                //        },
                //        []
                //    }
                //};

                //o.AddSecurityRequirement(securityRequirement);
            });

            return services;
        }
    }
}

using Asp.Versioning.ApiExplorer;
using Infrastructure.Access;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Web.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerWithUi(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();

                foreach (ApiVersionDescription description in descriptions)
                {
                    string url = $"/swagger/{description.GroupName}/swagger.json";
                    string name = description.GroupName.ToUpperInvariant();

                    options.SwaggerEndpoint(url, name);
                    options.DisplayRequestDuration();
                    options.EnableDeepLinking();
                    options.ShowExtensions();
                }
            });

            return app;
        }

        public static async Task SeedRolesAndPermissions(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();

            foreach (var roleEntry in RolePermissions.Map)
            {
                string roleName = roleEntry.Key;

                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<long>(roleName));
                }

                IdentityRole<long>? role = await roleManager.FindByNameAsync(roleName);

                IList<Claim> existingClaims = await roleManager.GetClaimsAsync(role!);

                foreach (string permission in roleEntry.Value)
                {
                    if (!existingClaims.Any(c => 
                        c.Type == CustomClaimTypes.Permission && c.Value == permission))
                    {
                        await roleManager.AddClaimAsync(
                            role!,
                            new Claim(CustomClaimTypes.Permission, permission));
                    }
                }
            }
        }
    }
}

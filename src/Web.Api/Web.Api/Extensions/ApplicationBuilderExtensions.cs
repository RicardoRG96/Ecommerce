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

            var adminRole = await roleManager.FindByNameAsync(Roles.Admin);

            if (adminRole is null)
            {
                await roleManager.CreateAsync(adminRole = new IdentityRole<long>(Roles.Admin));

                await roleManager.AddClaimAsync(
                    adminRole, 
                    new Claim(CustomClaimTypes.Permission, Permissions.UsersRead));

                await roleManager.AddClaimAsync(
                    adminRole,
                    new Claim(CustomClaimTypes.Permission, Permissions.UsersUpdate));

                await roleManager.AddClaimAsync(
                    adminRole,
                    new Claim(CustomClaimTypes.Permission, Permissions.UsersDelete));

                await roleManager.AddClaimAsync(
                    adminRole,
                    new Claim(CustomClaimTypes.Permission, Permissions.UsersCreate));
            }

            var memberRole = await roleManager.FindByNameAsync(Roles.Member);

            if (memberRole is null)
            {
                await roleManager.CreateAsync(memberRole = new IdentityRole<long>(Roles.Member));

                await roleManager.AddClaimAsync(
                    memberRole,
                    new Claim(CustomClaimTypes.Permission, Permissions.UsersRead));

                await roleManager.AddClaimAsync(
                    memberRole,
                    new Claim(CustomClaimTypes.Permission, Permissions.UsersUpdate));
            }
        }

        public static async Task SeedAsync(this WebApplication app)
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
            }
        }
    }
}

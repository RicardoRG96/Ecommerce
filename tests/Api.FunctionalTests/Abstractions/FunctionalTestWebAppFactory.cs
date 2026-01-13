using Infrastructure.Identity;
using Infrastructure.Persistence.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http.Headers;
using Testcontainers.MsSql;
using Web.Api;

namespace Api.FunctionalTests.Abstractions
{
    public class FunctionalTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("P@ssw0rd123")
            .Build();

        public HttpClient AuthenticatedClient { get; private set; } = default!;
        public AuthFixture.AdminUser AuthAdminUser { get; } = new();
        public AuthFixture.CustomerUser AuthCustomerUser { get; } = new();
        public AuthFixture.CustomerSupportUser AuthCustomerSupportUser { get; } = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(_dbContainer.GetConnectionString());
                });
            });
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            ExecuteMigrations();

            var dbSeedSql = await File.ReadAllTextAsync("db_seed.sql");

            await _dbContainer.ExecScriptAsync(dbSeedSql);

            await InitializeTestAuthenticationAsync();
        }

        public new Task DisposeAsync()
        {
            return _dbContainer.StopAsync();
        }

        private void ExecuteMigrations()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(_dbContainer.GetConnectionString())
                .Options;

            using var db = new ApplicationDbContext(options);
            db.Database.Migrate();
        }

        private async Task InitializeTestAuthenticationAsync()
        {
            using var scope = Services.CreateScope();

            UserManager<ApplicationUser> userManager =
                scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var client = CreateClient();

            await AuthAdminUser.InitializeAsync(client, userManager);

            await AuthCustomerUser.InitializeAsync(client);

            await AuthCustomerSupportUser.InitializeAsync(client, userManager);

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", AuthAdminUser.AccessToken);

            AuthenticatedClient = client;
        }
    }
}

using Infrastructure.Persistence.Database;
using Microsoft.AspNetCore.Hosting;
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
        public AuthFixture Auth { get; } = new();

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

            var client = CreateClient();
            await Auth.InitializeAsync(client);

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Auth.AccessToken);

            AuthenticatedClient = client;
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
    }
}

using Application.Abstractions.Search;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Search;

public sealed class AlgoliaIndexInitializer : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AlgoliaIndexInitializer> _logger;

    public AlgoliaIndexInitializer(
        IServiceScopeFactory scopeFactory,
        ILogger<AlgoliaIndexInitializer> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Initializing Algolia search index configuration...");
            
            using var scope = _scopeFactory.CreateScope();

            var searchService = scope.ServiceProvider.GetRequiredService<ISearchService>();

            await searchService.InitializeIndexAsync(cancellationToken);

            _logger.LogInformation("Algolia search index initialized successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing Algolia search index");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Algolia Index Initializer is stopping");
        return Task.CompletedTask;
    }
}
using Application.Abstractions.Search;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Search;

public sealed class AlgoliaIndexInitializer : IHostedService
{
    private readonly ISearchService _searchService;
    private readonly ILogger<AlgoliaIndexInitializer> _logger;

    public AlgoliaIndexInitializer(
        ISearchService searchService,
        ILogger<AlgoliaIndexInitializer> logger)
    {
        _searchService = searchService;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Initializing Algolia search index configuration...");
            
            await _searchService.InitializeIndexAsync(cancellationToken);
            
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
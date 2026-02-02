using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Messaging;
using Application.Abstractions.Search;
using SharedKernel;

namespace Application.Products.IndexProducts;

public sealed class IndexProductsCommandHandler : ICommandHandler<IndexProductsCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductSkuRepository _productSkuRepository;
    private readonly ISearchService _searchService;

    public IndexProductsCommandHandler(
        IProductRepository productRepository,
        IProductSkuRepository productSkuRepository,
        ISearchService searchService)
    {
        _productRepository = productRepository;
        _productSkuRepository = productSkuRepository;
        _searchService = searchService;
    }

    public async Task<Result<bool>> Handle(
        IndexProductsCommand command, 
        CancellationToken cancellationToken)
    {
        // Get all published and active products with related data
        var products = await _productRepository
            .GetAll()
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.ProductSkus.Where(sku => sku.IsActive))
                .ThenInclude(sku => sku.ProductAttributeValues)
                    .ThenInclude(av => av.AttributeValue)
                        .ThenInclude(av => av!.Attribute)
            .Include(p => p.ProductGalleries.OrderBy(g => g.DisplayOrder))
            .Where(p => p.IsPublished && p.IsActive)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        // Map to search models
        var searchModels = products
            .SelectMany(product => product.ProductSkus.Select(sku => new ProductSearchModel
            {
                ObjectID = $"{product.Id}_{sku.Id}",
                Name = product.Name!,
                Slug = product.Slug!,
                Description = product.Description ?? string.Empty,
                SkuCode = sku.SkuCode,
                Brand = product.Brand?.Name,
                Category = product.Category?.Name,
                Price = sku.Price,
                ImageUrl = product.ProductGalleries.FirstOrDefault()?.ImageUrl,
                Attributes = sku.ProductAttributeValues
                    .Where(av => av.AttributeValue?.Attribute != null)
                    .ToDictionary(
                        av => av.AttributeValue!.Attribute!.Name!,
                        av => (object)av.AttributeValue!.Value!
                    )
            }))
            .ToList();

        // Initialize index configuration
        await _searchService.InitializeIndexAsync(cancellationToken);

        // Index products in Algolia
        await _searchService.IndexProductsAsync(searchModels, cancellationToken);

        return true;
    }
}
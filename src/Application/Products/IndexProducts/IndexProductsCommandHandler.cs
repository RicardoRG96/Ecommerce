using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Messaging;
using Application.Abstractions.Search;
using Domain.Entities.Products;
using SharedKernel;

namespace Application.Products.IndexProducts;

public sealed class IndexProductsCommandHandler : ICommandHandler<IndexProductsCommand>
{
    private readonly IProductSkuRepository _productSkuRepository;
    private readonly ISearchService _searchService;

    public IndexProductsCommandHandler(
        IProductSkuRepository productSkuRepository,
        ISearchService searchService)
    {
        _productSkuRepository = productSkuRepository;
        _searchService = searchService;
    }

    public async Task<Result> Handle(IndexProductsCommand command, CancellationToken cancellationToken)
    {
        List<ProductSku> productSkus = await _productSkuRepository.GetAllAsync(cancellationToken);

        IEnumerable<ProductSearchModel> searchModels = productSkus
            .Where(sku => sku.Product.IsPublished)
            .Select(sku => new ProductSearchModel
            {
                ObjectID = $"{sku.ProductId}_{sku.Id}",
                ProductId = sku.ProductId.ToString(),
                Name = sku.Product.Name!,
                Slug = sku.Product.Slug!,
                Description = sku.Product.Description ?? string.Empty,
                SkuCode = sku.SkuCode,
                Brand = sku.Product.Brand?.Name,
                Category = sku.Product.Category?.Name,
                Price = sku.Price,
                ImageUrl = sku.ProductGalleries.FirstOrDefault(pg => pg.IsPrimary)?.MediaUrl,
                Attributes = sku.ProductAttributeValues
                    .Where(av => av.AttributeValue?.Attribute != null)
                    .ToDictionary(
                        av => av.AttributeValue!.Attribute!.Name!,
                        av => (object)av.AttributeValue!.Value!
                    )
            })
            .ToList();

        // Index products in Algolia
        await _searchService.IndexProductsAsync(searchModels, cancellationToken);

        return Result.Success();
    }
}
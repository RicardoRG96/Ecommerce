using Application.Abstractions.Messaging;
using Application.Abstractions.Search;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Products.FilterByBrand
{
    internal sealed class FilterByBrandQueryHandler : IQueryHandler<FilterByBrandQuery, IEnumerable<ProductSearchModel>>
    {
        private readonly ISearchService _searchService;

        public FilterByBrandQueryHandler(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public async Task<Result<IEnumerable<ProductSearchModel>>> Handle(
            FilterByBrandQuery query, 
            CancellationToken cancellationToken)
        {
            IEnumerable<ProductSearchModel> productsResult = await _searchService.FilterByBrandAsync(
                query.Brands, 
                cancellationToken);

            if (productsResult is null || !productsResult.Any())
            {
                return Result.Failure<IEnumerable<ProductSearchModel>>(ProductErrors.SearchNotFound);
            }

            return Result.Success(productsResult);
        }
    }
}

using Application.Abstractions.Messaging;
using Application.Abstractions.Search;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Products.FilterByPriceRange
{
    internal sealed class FilterByPriceRangeQueryHandler 
        : IQueryHandler<FilterByPriceRangeQuery, IEnumerable<ProductSearchModel>>
    {
        private readonly ISearchService _searchService;

        public FilterByPriceRangeQueryHandler(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public async Task<Result<IEnumerable<ProductSearchModel>>> Handle(
            FilterByPriceRangeQuery query, 
            CancellationToken cancellationToken)
        {
            IEnumerable<ProductSearchModel> productsResult = await _searchService.FilterByPriceRange(
                query.MinPrice,
                query.MaxPrice,
                cancellationToken);

            if (productsResult is null || !productsResult.Any())
            {
                return Result.Failure<IEnumerable<ProductSearchModel>>(ProductErrors.SearchNotFound);
            }

            return Result.Success(productsResult);
        }
    }
}

using Application.Abstractions.Messaging;
using Application.Abstractions.Search;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Products.SearchProduct
{
    internal sealed class SearchProductQueryHandler : IQueryHandler<SearchProductQuery, IEnumerable<ProductSearchModel>>
    {
        private readonly ISearchService _searchService;

        public SearchProductQueryHandler(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public async Task<Result<IEnumerable<ProductSearchModel>>> Handle(
            SearchProductQuery query, 
            CancellationToken cancellationToken)
        {
            IEnumerable<ProductSearchModel> productsResult = await _searchService.SearchProductsAsync(
                query.Query, 
                cancellationToken);

            if (productsResult is null || !productsResult.Any())
            {
                return Result.Failure<IEnumerable<ProductSearchModel>>(ProductErrors.NotFoundByName(query.Query));
            }

            return Result.Success(productsResult);
        }
    }
}

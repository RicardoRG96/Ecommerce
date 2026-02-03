using Application.Abstractions.Messaging;
using Application.Abstractions.Search;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Products.FilterByCategory
{
    internal sealed class FilterByCategoryQueryHandler : IQueryHandler<FilterByCategoryQuery, IEnumerable<ProductSearchModel>>
    {
        private readonly ISearchService _searchService;

        public FilterByCategoryQueryHandler(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public async Task<Result<IEnumerable<ProductSearchModel>>> Handle(FilterByCategoryQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<ProductSearchModel> productsResult = await _searchService.FilterByCategoryAsync(
                query.Categories,
                cancellationToken);

            if (productsResult is null || !productsResult.Any())
            {
                return Result.Failure<IEnumerable<ProductSearchModel>>(ProductErrors.SearchNotFound);
            }

            return Result.Success(productsResult);
        }
    }
}

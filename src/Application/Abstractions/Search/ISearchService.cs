namespace Application.Abstractions.Search
{
    public interface ISearchService
    {
        Task InitializeIndexAsync(CancellationToken cancellationToken = default);

        Task IndexProductsAsync(
            IEnumerable<ProductSearchModel> products, 
            CancellationToken cancellationToken = default);

        Task<IEnumerable<ProductSearchModel>> SearchProductsAsync(
            string query, 
            CancellationToken cancellationToken = default);

        Task<IEnumerable<ProductSearchModel>> FilterByBrandAsync(
            List<string> brands, 
            CancellationToken cancellationToken = default);

        Task<IEnumerable<ProductSearchModel>> FilterByCategoryAsync(
            List<string> categories, 
            CancellationToken cancellationToken = default);

        Task<IEnumerable<ProductSearchModel>> FilterByPriceRange(
            decimal minPrice,
            decimal maxPrice,
            CancellationToken cancellationToken = default);
    }
}

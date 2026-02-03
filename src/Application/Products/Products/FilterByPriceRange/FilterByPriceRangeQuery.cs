using Application.Abstractions.Messaging;
using Application.Abstractions.Search;

namespace Application.Products.Products.FilterByPriceRange
{
    public sealed record FilterByPriceRangeQuery(
        int MinPrice,
        int MaxPrice) : IQuery<IEnumerable<ProductSearchModel>>;
}

using Application.Abstractions.Messaging;
using Application.Abstractions.Search;

namespace Application.Products.Products.FilterByPriceRange
{
    public sealed record FilterByPriceRangeQuery(
        decimal MinPrice,
        decimal MaxPrice) : IQuery<IEnumerable<ProductSearchModel>>;
}

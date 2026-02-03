using Application.Abstractions.Messaging;
using Application.Abstractions.Search;

namespace Application.Products.Products.FilterByBrand
{
    public sealed record FilterByBrandQuery(List<string> Brands) : IQuery<IEnumerable<ProductSearchModel>>;
}

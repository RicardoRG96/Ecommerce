using Application.Abstractions.Messaging;
using Application.Abstractions.Search;

namespace Application.Products.Products.FilterByCategory
{
    public sealed record FilterByCategoryQuery(List<string> Categories) 
        : IQuery<IEnumerable<ProductSearchModel>>;
}

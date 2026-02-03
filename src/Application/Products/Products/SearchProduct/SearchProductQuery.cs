using Application.Abstractions.Messaging;
using Application.Abstractions.Search;

namespace Application.Products.Products.SearchProduct
{
    public sealed record SearchProductQuery(string Query) 
        : IQuery<IEnumerable<ProductSearchModel>>; 
}

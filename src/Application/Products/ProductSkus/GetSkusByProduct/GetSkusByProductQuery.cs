using Application.Abstractions.Messaging;
using Application.Products.ProductSkus.Common.Mappers;

namespace Application.Products.ProductSkus.GetSkusByProduct
{
    public sealed record GetSkusByProductQuery(long ProductId) 
        : IQuery<List<ProductSkuResponse>>;
}

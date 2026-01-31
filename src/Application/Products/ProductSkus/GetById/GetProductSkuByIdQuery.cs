using Application.Abstractions.Messaging;

namespace Application.Products.ProductSkus.GetById
{
    public sealed record GetProductSkuByIdQuery(long Id) : IQuery<ProductSkuResponse>;
}

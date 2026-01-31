using Application.Abstractions.Messaging;
using Application.Products.ProductSkus.Common.Mappers;

namespace Application.Products.ProductSkus.GetById
{
    public sealed record GetProductSkuByIdQuery(long Id) : IQuery<ProductSkuResponse>;
}

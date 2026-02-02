using Application.Abstractions.Messaging;
using Application.Products.ProductAttributeValues.Common.Mappers;

namespace Application.Products.ProductAttributeValues.GetSkuAttributes
{
    public sealed record GetSkuAttributesQuery(
        long ProductSkuId) : IQuery<List<ProductAttributeValueResponse>>;
}

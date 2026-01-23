using Application.Abstractions.Messaging;

namespace Application.Products.Products.GetProductDetail
{
    public sealed record GetProductDetailQuery(long Id) : IQuery<ProductDetailResponse>;
}

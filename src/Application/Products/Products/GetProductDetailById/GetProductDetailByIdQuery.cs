using Application.Abstractions.Messaging;

namespace Application.Products.Products.GetProductDetailById
{
    public sealed record GetProductDetailByIdQuery(long Id) : IQuery<ProductDetailResponse>;
}

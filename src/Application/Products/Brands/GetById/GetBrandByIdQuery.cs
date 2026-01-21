using Application.Abstractions.Messaging;

namespace Application.Products.Brands.GetById
{
    public sealed record GetBrandByIdQuery(long BrandId) : IQuery<BrandResponse>;
}

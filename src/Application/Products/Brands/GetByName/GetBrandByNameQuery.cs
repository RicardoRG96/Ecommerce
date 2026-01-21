using Application.Abstractions.Messaging;

namespace Application.Products.Brands.GetByName
{
    public sealed record GetBrandByNameQuery(string BrandName) : IQuery<BrandResponse>;
}

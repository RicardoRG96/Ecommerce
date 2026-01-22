using Application.Abstractions.Messaging;

namespace Application.Products.Brands.GetFeaturedBrands
{
    public sealed record GetFeaturedBrandsQuery : IQuery<List<BrandResponse>>;
}

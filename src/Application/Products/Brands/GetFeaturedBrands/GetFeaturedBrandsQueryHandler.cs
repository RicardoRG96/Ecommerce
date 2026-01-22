using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using SharedKernel;

namespace Application.Products.Brands.GetFeaturedBrands
{
    internal sealed class GetFeaturedBrandsQueryHandler : IQueryHandler<GetFeaturedBrandsQuery, List<BrandResponse>>
    {
        private readonly IBrandRepository _brandRepository;

        public GetFeaturedBrandsQueryHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<Result<List<BrandResponse>>> Handle(GetFeaturedBrandsQuery query, CancellationToken cancellationToken)
        {
            List<Brand?> featuredBrands = await _brandRepository.GetFeaturedBrandsAsync(cancellationToken);

            List<BrandResponse> featuredBrandsResponse = [.. featuredBrands.Select(brand => new BrandResponse
                {
                    Id = brand!.Id,
                    Name = brand.Name,
                    Slug = brand.Slug,
                    Description = brand.Description,
                    LogoUrl = brand.LogoUrl,
                    BannerUrl = brand.BannerUrl,
                    WebsiteUrl = brand.WebsiteUrl,
                    IsActive = brand.IsActive,
                    IsFeatured = brand.IsFeatured,
                    DisplayOrder = brand.DisplayOrder,
                    MetaTitle = brand.MetaTitle,
                    MetaDescription = brand.MetaDescription,
                    MetaKeywords = brand.MetaKeywords
                })];

            return Result.Success(featuredBrandsResponse);
        }
    }
}

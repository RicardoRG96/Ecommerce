using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Brands.GetByName
{
    internal sealed class GetBrandByNameQueryHandler : IQueryHandler<GetBrandByNameQuery, BrandResponse>
    {
        private readonly IBrandRepository _brandRepository;

        public GetBrandByNameQueryHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<Result<BrandResponse>> Handle(GetBrandByNameQuery query, CancellationToken cancellationToken)
        {
            Brand? brand = await _brandRepository.GetByNameAsync(query.BrandName, cancellationToken);

            if (brand is null)
            {
                return Result.Failure<BrandResponse>(BrandErrors.NotFoundByName(query.BrandName));
            }

            BrandResponse brandResponse = new()
            {
                Id = brand.Id,
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
                MetaKeywords = brand.MetaKeywords,
            };

            return Result.Success(brandResponse);
        }
    }
}

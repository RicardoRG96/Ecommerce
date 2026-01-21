using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Brands.GetById
{
    internal sealed class GetBrandByIdQueryHandler : IQueryHandler<GetBrandByIdQuery, BrandResponse>
    {
        private readonly IBrandRepository _brandRepository;

        public GetBrandByIdQueryHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<Result<BrandResponse>> Handle(GetBrandByIdQuery query, CancellationToken cancellationToken)
        {
            Brand? brand = await _brandRepository.GetByIdAsync(query.BrandId, cancellationToken);

            if (brand is null)
            {
                return Result.Failure<BrandResponse>(BrandErrors.NotFound(query.BrandId));
            }

            BrandResponse brandResponse = new()
            {
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

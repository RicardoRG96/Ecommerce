using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using SharedKernel;

namespace Application.Products.Brands.GetWithPagination
{
    internal sealed class GetBrandsWithPaginationQueryHandler 
        : IQueryHandler<GetBrandsWithPaginationQuery, PaginatedList<BrandResponse>>
    {
        private readonly IBrandRepository _brandRepository;

        public GetBrandsWithPaginationQueryHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<Result<PaginatedList<BrandResponse>>> Handle(GetBrandsWithPaginationQuery query, CancellationToken cancellationToken)
        {
            PaginatedList<Brand> brands = await _brandRepository.GetAllAsync(
                query.PageNumber,
                query.PageSize, 
                cancellationToken);

            PaginatedList<BrandResponse> paginatedBrandsResponse = MapToBrandResponsePaginatedList(brands, query);

            return Result.Success(paginatedBrandsResponse);
        }

        private static PaginatedList<BrandResponse> MapToBrandResponsePaginatedList(
            PaginatedList<Brand> brandsPaginatedList,
            GetBrandsWithPaginationQuery query)
        {
            List<BrandResponse> brandResponse = brandsPaginatedList.Items.Select(b =>
            {
                BrandResponse brandResponse = new()
                {
                    Id = b.Id,
                    Name = b.Name,
                    Slug = b.Slug,
                    Description = b.Description,
                    LogoUrl = b.LogoUrl,
                    BannerUrl = b.BannerUrl,
                    WebsiteUrl = b.WebsiteUrl,
                    IsActive = b.IsActive,
                    IsFeatured = b.IsFeatured,
                    DisplayOrder = b.DisplayOrder,
                    MetaTitle = b.MetaTitle,
                    MetaDescription = b.MetaDescription,
                    MetaKeywords = b.MetaKeywords,
                };

                return brandResponse;
            }).ToList();

            PaginatedList<BrandResponse> paginatedBrandResponse = PaginatedList<BrandResponse>.Create(
                brandResponse,
                brandsPaginatedList.TotalCount,
                brandsPaginatedList.PageNumber,
                query.PageSize);

            return paginatedBrandResponse;
        }
    }
}

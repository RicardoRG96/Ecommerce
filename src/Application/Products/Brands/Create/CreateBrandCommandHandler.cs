using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Brands.Create
{
    internal sealed class CreateBrandCommandHandler : ICommandHandler<CreateBrandCommand, long>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<long>> Handle(CreateBrandCommand command, CancellationToken cancellationToken)
        {
            Brand? existingBrand = await _brandRepository.GetByNameAsync(command.Name, cancellationToken);

            if (existingBrand is not null)
            {
                return Result.Failure<long>(BrandErrors.DuplicatedBrandName);
            }

            Brand brand = Brand.Create(
                name: command.Name,
                description: command.Description,
                logoUrl: command.LogoUrl,
                bannerUrl: command.BannerUrl,
                websiteUrl: command.WebsiteUrl,
                isActive: command.IsActive,
                isFeatured: command.IsFeatured,
                displayOrder: command.DisplayOrder,
                metaTitle: command.MetaTitle,
                metaDescription: command.MetaDescription,
                metaKeywords: command.MetaKeywords
            );

            await _brandRepository.AddAsync(brand, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(brand.Id);
        }
    }
}

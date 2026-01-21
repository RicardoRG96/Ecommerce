using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Brands.Update
{
    internal sealed class UpdateBrandCommandHandler : ICommandHandler<UpdateBrandCommand>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateBrandCommand command, CancellationToken cancellationToken)
        {
            Brand? brand = await _brandRepository.GetByIdAsync(command.BrandId, cancellationToken);

            if (brand is null)
            {
                return Result.Failure(BrandErrors.NotFound(command.BrandId));
            }

            if (brand.Name == command.Name)
            {
                return Result.Failure(BrandErrors.DuplicatedBrandName);
            }

            brand.Name = command.Name;
            brand.Slug = SlugGenerator.GenerateSlug(command.Name);
            brand.Description = command.Description;
            brand.LogoUrl = command.LogoUrl;
            brand.BannerUrl = command.BannerUrl;
            brand.WebsiteUrl = command.WebsiteUrl;
            brand.IsActive = command.IsActive;
            brand.IsFeatured = command.IsFeatured;
            brand.DisplayOrder = command.DisplayOrder;
            brand.MetaTitle = command.MetaTitle;
            brand.MetaDescription = command.MetaDescription;
            brand.MetaKeywords = command.MetaKeywords;

            _brandRepository.Update(brand);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

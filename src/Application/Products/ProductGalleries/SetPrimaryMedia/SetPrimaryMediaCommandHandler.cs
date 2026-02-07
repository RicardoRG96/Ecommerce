using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.ProductGalleries.SetPrimaryMedia
{
    internal sealed class SetPrimaryMediaCommandHandler : ICommandHandler<SetPrimaryMediaCommand>
    {
        private readonly IProductGalleryRepository _productGalleryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SetPrimaryMediaCommandHandler(
            IProductGalleryRepository productGalleryRepository, 
            IUnitOfWork unitOfWork)
        {
            _productGalleryRepository = productGalleryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(SetPrimaryMediaCommand command, CancellationToken cancellationToken)
        {
            ProductGallery? productGallery = await _productGalleryRepository.GetByIdAsync(command.Id, cancellationToken);

            if (productGallery is null)
            {
                return Result.Failure(ProductGalleryErrors.NotFound(command.Id));
            }

            if (productGallery.IsPrimary)
            {
                return Result.Success();
            }

            ProductGallery? primaryMedia = await _productGalleryRepository.GetPrimaryMediaAsync(
                productGallery.ProductId, 
                cancellationToken);

            if (primaryMedia is not null)
            {
                primaryMedia.UnsetPrimary();
            }

            productGallery.SetPrimary();

            _productGalleryRepository.Update(productGallery);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

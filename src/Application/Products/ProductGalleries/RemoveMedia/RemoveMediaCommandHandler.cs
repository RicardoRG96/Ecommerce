using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.ProductGalleries.RemoveMedia
{
    internal sealed class RemoveMediaCommandHandler : ICommandHandler<RemoveMediaCommand>
    {
        private readonly IProductGalleryRepository _productGalleryRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveMediaCommandHandler(
            IProductGalleryRepository productGalleryRepository, 
            IFileStorageService fileStorageService, 
            IUnitOfWork unitOfWork)
        {
            _productGalleryRepository = productGalleryRepository;
            _fileStorageService = fileStorageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(RemoveMediaCommand command, CancellationToken cancellationToken)
        {
            ProductGallery? productGallery = await _productGalleryRepository.GetByIdAsync(
                command.Id, 
                cancellationToken);

            if (productGallery is null)
            {
                return Result.Failure(ProductGalleryErrors.NotFound(command.Id));
            }

            Result deletionFromStorageServiceResult = await _fileStorageService.DeleteAsync(
                productGallery.MediaUrl,
                cancellationToken);

            if (deletionFromStorageServiceResult.IsFailure)
            {
                return Result.Failure(deletionFromStorageServiceResult.Error);
            }

            _productGalleryRepository.Delete(productGallery);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

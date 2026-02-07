using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Application.Products.ProductGalleries.Common.Services;
using Domain.Entities.Products;
using SharedKernel;

namespace Application.Products.ProductGalleries.AddMedia
{
    internal sealed class AddMediaCommandHandler : ICommandHandler<AddMediaCommand, long>
    {
        private readonly IProductGalleryRepository _productGalleryRepository;
        private readonly IProductGalleryValidator _validator;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;

        public AddMediaCommandHandler(
            IProductGalleryRepository productGalleryRepository,
            IProductGalleryValidator validator,
            IFileStorageService fileStorageService,
            IUnitOfWork unitOfWork)
        {
            _productGalleryRepository = productGalleryRepository;
            _validator = validator;
            _fileStorageService = fileStorageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<long>> Handle(AddMediaCommand command, CancellationToken cancellationToken)
        {
            Result validations = await RelatedEntitiesValidation(command, cancellationToken);

            if (validations.IsFailure)
            {
                return Result.Failure<long>(validations.Error);
            }

            Result<string> uploadResult = await _fileStorageService.UploadAsync(
                command.Stream,
                command.FileName,
                command.ContentType,
                cancellationToken);

            if (uploadResult.IsFailure)
            {
                return Result.Failure<long>(uploadResult.Error);
            }

            string mediaUrl = uploadResult.Value;
            string mimeType = command.ContentType;
            string mediaType = MediaTypeMapper.Map(mimeType);

            ProductGallery productGallery = ProductGallery.Create(
                command.ProductId,
                command.SkuId,
                mediaUrl,
                mediaType,
                mimeType,
                command.IsPrimary,
                command.DisplayOrder,
                command.AltText);

            await _productGalleryRepository.AddAsync(productGallery, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(productGallery.Id);
        }

        private async Task<Result> RelatedEntitiesValidation(
            AddMediaCommand command,
            CancellationToken cancellationToken)
        {
            Result productIdValidation = await _validator.ValidateProductIdExistsAsync(command.ProductId, cancellationToken);

            if (productIdValidation.IsFailure)
            {
                return Result.Failure(productIdValidation.Error);
            }

            Result skuIdValidation = await _validator.ValidateSkuIdExistsAsync(command.SkuId, cancellationToken);

            if (skuIdValidation.IsFailure)
            {
                return Result.Failure(skuIdValidation.Error);
            }

            Result skuBelongsToProductValidation = await _validator.ValidateSkuBelongsToProductAsync(command.ProductId, command.SkuId, cancellationToken);

            if (skuBelongsToProductValidation.IsFailure)
            {
                return Result.Failure(skuBelongsToProductValidation.Error);
            }

            return Result.Success();
        }
    }
}

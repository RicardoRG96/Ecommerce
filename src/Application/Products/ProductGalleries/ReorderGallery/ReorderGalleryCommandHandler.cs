using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Application.Products.ProductGalleries.Common.Services;
using Domain.Entities.Products;
using SharedKernel;

namespace Application.Products.ProductGalleries.ReorderGallery
{
    internal sealed class ReorderGalleryCommandHandler : ICommandHandler<ReorderGalleryCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductSkuRepository _productSkuRepository;
        private readonly IProductGalleryValidator _validator;
        private readonly IUnitOfWork _unitOfWork;

        public ReorderGalleryCommandHandler(
            IProductRepository productRepository, 
            IProductSkuRepository productSkuRepository,
            IProductGalleryValidator validator,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _productSkuRepository = productSkuRepository;
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ReorderGalleryCommand command, CancellationToken cancellationToken)
        {
            Result validations = await RelatedEntitiesValidation(command, cancellationToken);

            if (validations.IsFailure)
            {
                return Result.Failure<long>(validations.Error);
            }

            Product? product = await _productRepository.GetByIdIncludingRelatedEntitiesAsync(
                command.ProductId, 
                cancellationToken);

            Result reorderedGalleryResult = product!.ReorderGallery(command.OrderedGalleryItemIds);

            if (reorderedGalleryResult.IsFailure)
            {
                return Result.Failure(reorderedGalleryResult.Error);
            }

            _productRepository.Update(product);

            ProductSku? productSku = await _productSkuRepository.GetByIdWithRelatedEntitiesAsync(
                command.SkuId!.Value, 
                cancellationToken);

            if (productSku is not null)
            {
                Result skuReorderResult = productSku.ReorderGallery(command.OrderedGalleryItemIds);

                if (skuReorderResult.IsFailure)
                {
                    return Result.Failure(skuReorderResult.Error);
                }

                _productSkuRepository.Update(productSku);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task<Result> RelatedEntitiesValidation(
            ReorderGalleryCommand command,
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

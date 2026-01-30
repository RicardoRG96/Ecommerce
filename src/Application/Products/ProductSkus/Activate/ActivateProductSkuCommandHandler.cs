using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Application.Products.ProductSkus.Common.Services;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.ProductSkus.Activate
{
    internal sealed class ActivateProductSkuCommandHandler : ICommandHandler<ActivateProductSkuCommand>
    {
        private readonly IProductSkuRepository _productSkuRepository;
        private readonly IProductSkuValidator _validator;
        private readonly IUnitOfWork _unitOfWork;

        public ActivateProductSkuCommandHandler(
            IProductSkuRepository productSkuRepository, 
            IProductSkuValidator validator, 
            IUnitOfWork unitOfWork)
        {
            _productSkuRepository = productSkuRepository;
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ActivateProductSkuCommand command, CancellationToken cancellationToken)
        {
            ProductSku? productSku = await _productSkuRepository.GetByIdAsync(command.Id, cancellationToken);

            if (productSku is null)
            {
                return Result.Failure(ProductSkuErrors.NotFound(command.Id));
            }

            if (productSku.IsActive)
            {
                return Result.Success();
            }

            Result validations = await Validations(productSku, cancellationToken);

            if (validations.IsFailure)
            {
                return Result.Failure(validations.Error);
            }

            productSku.Activate();

            _productSkuRepository.Update(productSku);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task<Result> Validations(ProductSku productSku, CancellationToken cancellationToken)
        {
            Result productPublishedValidation = await _validator.ValidateProductIsPublishedAsync(
                productSku.ProductId,
                cancellationToken);

            if (productPublishedValidation.IsFailure)
            {
                return Result.Failure(productPublishedValidation.Error);
            }

            Result attributeValuesValidation = await _validator.ValidateAttributeValuesAreActive(
                productSku.Id,
                cancellationToken);

            if (attributeValuesValidation.IsFailure)
            {
                return Result.Failure(attributeValuesValidation.Error);
            }

            return Result.Success();
        }
    }
}

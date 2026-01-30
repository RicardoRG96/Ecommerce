using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Application.Products.ProductSkus.Common.Services;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.ProductSkus.Update
{
    internal sealed class UpdateProductSkuCommandHandler : ICommandHandler<UpdateProductSkuCommand>
    {
        private readonly IProductSkuRepository _productSkuRepository;
        private readonly IProductSkuValidator _validator;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductSkuCommandHandler(
            IProductSkuRepository productSkuRepository, 
            IProductSkuValidator validator, 
            IUnitOfWork unitOfWork)
        {
            _productSkuRepository = productSkuRepository;
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateProductSkuCommand command, CancellationToken cancellationToken)
        {
            ProductSku? productSku = await _productSkuRepository.GetByIdAsync(command.Id, cancellationToken);

            if (productSku is null)
            {
                return Result.Failure(ProductSkuErrors.NotFound(command.Id));
            }

            Result validations = await Validations(command, cancellationToken);

            if (validations.IsFailure)
            {
                return validations;
            }

            productSku.Update(
                command.ProductId,
                command.BarCode,
                command.Price,
                command.Cost,
                command.Weight,
                command.Length,
                command.Width,
                command.Height,
                command.DisplayOrder);

            _productSkuRepository.Update(productSku);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task<Result> Validations(UpdateProductSkuCommand command, CancellationToken cancellationToken)
        {
            Result productPublishedValidation = await _validator.ValidateProductIsPublishedAsync(
                command.ProductId,
                cancellationToken);

            if (productPublishedValidation.IsFailure)
            {
                return Result.Failure<long>(productPublishedValidation.Error);
            }

            Result barCodeUniquenessValidation = await _validator.ValidateBarCodeIsUnique(
                command.BarCode, 
                command.Id, 
                cancellationToken);

            if (barCodeUniquenessValidation.IsFailure)
            {
                return Result.Failure(barCodeUniquenessValidation.Error);
            }

            return Result.Success();
        }
    }
}

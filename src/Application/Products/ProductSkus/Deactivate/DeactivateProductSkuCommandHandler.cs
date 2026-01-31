using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Application.Products.ProductSkus.Common.Services;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.ProductSkus.Deactivate
{
    internal sealed class DeactivateProductSkuCommandHandler : ICommandHandler<DeactivateProductSkuCommand>
    {
        private readonly IProductSkuRepository _productSkuRepository;
        private readonly IProductSkuValidator _validator;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateProductSkuCommandHandler(
            IProductSkuRepository productSkuRepository, 
            IProductSkuValidator validator, 
            IUnitOfWork unitOfWork)
        {
            _productSkuRepository = productSkuRepository;
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeactivateProductSkuCommand command, CancellationToken cancellationToken)
        {
            ProductSku? productSku = await _productSkuRepository.GetByIdAsync(
                command.Id, 
                cancellationToken);

            if (productSku is null)
            {
                return Result.Failure(ProductSkuErrors.NotFound(command.Id));
            }

            if (!productSku.IsActive)
            {
                return Result.Success();
            }

            // TODO: Implement this validation when order management is available.

                //Result currentOrdersValidation = await _validator.ValidateSkuHasNoActiveOrders(
                //    command.Id, 
                //    cancellationToken);

                //if (currentOrdersValidation.IsFailure)
                //{
                //    return Result.Failure(currentOrdersValidation.Error);
                //}

            productSku.Deactivate();

            _productSkuRepository.Update(productSku);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

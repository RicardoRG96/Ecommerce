using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Application.Products.ProductAttributeValues.Common.Services;
using SharedKernel;

namespace Application.Products.ProductAttributeValues.Remove
{
    internal sealed class RemoveAttributeValueFromSkuCommandHandler
        : ICommandHandler<RemoveAttributeValueFromSkuCommand>
    {
        private readonly IProductAttributeValueRepository _productAttributeValueRepository;
        private readonly IProductAttributeValueValidator _validator;

        public RemoveAttributeValueFromSkuCommandHandler(
            IProductAttributeValueRepository productAttributeValueRepository, 
            IProductAttributeValueValidator validator,
            IUnitOfWork unitOfWork)
        {
            _productAttributeValueRepository = productAttributeValueRepository;
            _validator = validator;
        }

        public async Task<Result> Handle(RemoveAttributeValueFromSkuCommand command, CancellationToken cancellationToken)
        {
            Result removeAttributeValueFromSkuValidation = await _validator.ValidateCanRemoveAttributeValueFromSku(
                command.SkuId, 
                command.AttributeValueId, 
                cancellationToken);

            if (removeAttributeValueFromSkuValidation.IsFailure)
            {
                return Result.Failure(removeAttributeValueFromSkuValidation.Error);
            }

            await _productAttributeValueRepository.RemoveAttributeValueFromSku(
                command.SkuId, 
                command.AttributeValueId, 
                cancellationToken);

            return Result.Success();
        }
    }
}

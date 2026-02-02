using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Application.Products.ProductAttributeValues.Common.Services;
using Domain.Entities.Products;
using SharedKernel;

namespace Application.Products.ProductAttributeValues.Assign
{
    internal sealed class AssignAttributeValueToSkuCommandHandler : ICommandHandler<AssignAttributeValueToSkuCommand>
    {
        private readonly IProductAttributeValueRepository _productAttributeValueRepository;
        private readonly IProductAttributeValueValidator _validator;
        private readonly IUnitOfWork _unitOfWork;

        public AssignAttributeValueToSkuCommandHandler(
            IProductAttributeValueRepository productAttributeValueRepository, 
            IProductAttributeValueValidator validator, 
            IUnitOfWork unitOfWork)
        {
            _productAttributeValueRepository = productAttributeValueRepository;
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AssignAttributeValueToSkuCommand command, CancellationToken cancellationToken)
        {
            Result validations = await Validations(command, cancellationToken);

            if (validations.IsFailure)
            {
                return Result.Failure(validations.Error);
            }

            ProductAttributeValue productAttributeValue = ProductAttributeValue.Create(
                command.SkuId, 
                command.AttributeValueId);

            await _productAttributeValueRepository.AddAsync(
                productAttributeValue, 
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task<Result> Validations(
            AssignAttributeValueToSkuCommand command, 
            CancellationToken cancellationToken)
        {
            Result attributeValueValidation = await _validator.ValidateAttributeValueIsActive(
                command.AttributeValueId, 
                cancellationToken);

            if (attributeValueValidation.IsFailure)
            {
                return Result.Failure(attributeValueValidation.Error);
            }

            Result productskuValidation = await _validator.ValidateSkuIsActive(
                command.SkuId, 
                cancellationToken);

            if (productskuValidation.IsFailure)
            {
                return Result.Failure(productskuValidation.Error);
            }

            Result alreadyAssignValidation = await _validator.ValidateAttributeValueIsNotAlreadyAssignedToSku(
                command.SkuId, 
                command.AttributeValueId, 
                cancellationToken);

            if (alreadyAssignValidation.IsFailure)
            {
                return Result.Failure(alreadyAssignValidation.Error);
            }

            return Result.Success();
        }
    }
}

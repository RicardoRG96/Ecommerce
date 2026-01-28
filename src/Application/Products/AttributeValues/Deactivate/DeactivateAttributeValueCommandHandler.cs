using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Application.Products.AttributeValues.Common.Services;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.AttributeValues.Deactivate
{
    internal sealed class DeactivateAttributeValueCommandHandler : ICommandHandler<DeactivateAttributeValueCommand>
    {
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly IAttributeValueValidator _validator;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateAttributeValueCommandHandler(
            IAttributeValueRepository attributeValueRepository, 
            IAttributeValueValidator validator, 
            IUnitOfWork unitOfWork)
        {
            _attributeValueRepository = attributeValueRepository;
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeactivateAttributeValueCommand command, CancellationToken cancellationToken)
        {
            Result skusValidation = await _validator.ValidateSkuIsDisabledBeforeDeactivation(
                command.Id, cancellationToken);

            if (skusValidation.IsFailure)
            {
                return Result.Failure(skusValidation.Error);
            }

            AttributeValue? attributeValue = await _attributeValueRepository.GetByIdAsync(
                command.Id, cancellationToken);

            attributeValue!.Deactivate();

            _attributeValueRepository.Update(attributeValue);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

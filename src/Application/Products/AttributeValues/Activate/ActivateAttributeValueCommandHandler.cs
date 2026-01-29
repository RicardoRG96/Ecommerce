using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Application.Products.AttributeValues.Common.Services;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.AttributeValues.Activate
{
    internal sealed class ActivateAttributeValueCommandHandler : ICommandHandler<ActivateAttributeValueCommand>
    {
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly IAttributeValueValidator _validator;
        private readonly IUnitOfWork _unitOfWork;

        public ActivateAttributeValueCommandHandler(
            IAttributeValueRepository attributeValueRepository, 
            IAttributeValueValidator validator, 
            IUnitOfWork unitOfWork)
        {
            _attributeValueRepository = attributeValueRepository;
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ActivateAttributeValueCommand command, CancellationToken cancellationToken)
        {
            AttributeValue? attributeValue = await _attributeValueRepository.GetByIdAsync(
                command.Id,
                cancellationToken);

            if (attributeValue is null)
            {
                return Result.Failure(AttributeValueErrors.NotFound(command.Id));
            }

            Result attributeEntityValidation = await _validator.ValidateAttributeIsActiveAsync(
                attributeValue.AttributeId,
                cancellationToken);

            if (attributeEntityValidation.IsFailure)
            {
                return Result.Failure<long>(attributeEntityValidation.Error);
            }

            if (attributeValue.IsActive)
            {
                return Result.Success();
            }

            attributeValue.Activate();

            _attributeValueRepository.Update(attributeValue);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Application.Products.AttributeValues.Common.Services;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.AttributeValues.Update
{
    internal sealed class UpdateAttributeValueCommandHandler : ICommandHandler<UpdateAttributeValueCommand>
    {
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly IAttributeValueValidator _validator;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAttributeValueCommandHandler(
            IAttributeValueRepository attributeValueRepository, 
            IAttributeValueValidator validator, 
            IUnitOfWork unitOfWork)
        {
            _attributeValueRepository = attributeValueRepository;
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateAttributeValueCommand command, CancellationToken cancellationToken)
        {
            AttributeValue? attributeValue = await _attributeValueRepository.GetByIdAsync(
                command.Id,
                cancellationToken);

            if (attributeValue is null)
            {
                return Result.Failure(AttributeValueErrors.NotFound(command.Id));
            }

            Result attributeEntityValidation = await _validator.ValidateAttributeIsActiveAsync(
                command.AttributeId,
                cancellationToken);

            if (attributeEntityValidation.IsFailure)
            {
                return Result.Failure<long>(attributeEntityValidation.Error);
            }

            Result valueValidation = await _validator.ValidateAttributeValueIsUniqueAsync(
                command.AttributeId,
                command.Value,
                command.Id,
                cancellationToken);

            if (valueValidation.IsFailure)
            {
                return Result.Failure<long>(valueValidation.Error);
            }

            attributeValue.Update(
                command.AttributeId,
                command.Value,
                command.NumericValue,
                command.BooleanValue,
                command.DisplayOrder);

            _attributeValueRepository.Update(attributeValue);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

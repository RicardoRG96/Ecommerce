using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Application.Products.AttributeValues.Common.Services;
using Domain.Entities.Products;
using SharedKernel;

namespace Application.Products.AttributeValues.Create
{
    internal sealed class CreateAttributeValueCommandHandler : ICommandHandler<CreateAttributeValueCommand, long>
    {
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly IAttributeValueValidator _validator;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAttributeValueCommandHandler(
            IAttributeValueRepository attributeValueRepository, 
            IAttributeValueValidator validator, 
            IUnitOfWork unitOfWork)
        {
            _attributeValueRepository = attributeValueRepository;
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<long>> Handle(CreateAttributeValueCommand command, CancellationToken cancellationToken)
        {
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
                cancellationToken: cancellationToken);

            if (valueValidation.IsFailure)
            {
                return Result.Failure<long>(valueValidation.Error);
            }

            AttributeValue attributeValue = AttributeValue.Create(
                command.AttributeId, 
                command.Value, 
                command.NumericValue, 
                command.BooleanValue, 
                command.DisplayOrder, 
                command.IsActive);

            await _attributeValueRepository.AddAsync(attributeValue, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(attributeValue.Id);
        }
    }
}

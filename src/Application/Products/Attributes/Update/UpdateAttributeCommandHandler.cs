using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Application.Products.Attributes.Common.Services;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Attributes.Update
{
    internal sealed class UpdateAttributeCommandHandler : ICommandHandler<UpdateAttributeCommand>
    {
        private readonly IAttributeRepository _attributeRepository;
        private readonly IUniquenessAttributeCodeValidator _validator;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAttributeCommandHandler(IAttributeRepository attributeRepository, IUniquenessAttributeCodeValidator validator, IUnitOfWork unitOfWork)
        {
            _attributeRepository = attributeRepository;
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateAttributeCommand command, CancellationToken cancellationToken)
        {
            Domain.Entities.Products.Attribute? attribute = await _attributeRepository.GetByIdAsync(
                command.AttributeId, cancellationToken);

            if (attribute is null)
            {
                return Result.Failure(AttributeErrors.NotFound(command.AttributeId));
            }

            Result codeValidation = await _validator.ValidateAsync(
                command.Code, 
                command.AttributeId, 
                cancellationToken);

            if (codeValidation.IsFailure)
            {
                return Result.Failure(codeValidation.Error);
            }

            attribute.Update(
                command.Code,
                command.Name,
                command.Description,
                command.DataType,
                command.IsVariant,
                command.IsFilterable,
                command.IsRequired,
                command.DisplayOrder);

            _attributeRepository.Update(attribute);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Application.Products.Attributes.Common.Services;
using SharedKernel;

namespace Application.Products.Attributes.Create
{
    internal sealed class CreateAttributeCommandHandler : ICommandHandler<CreateAttributeCommand, long>
    {
        private readonly IAttributeRepository _attributeRepository;
        private readonly IUniquenessAttributeCodeValidator _Validator;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAttributeCommandHandler(
            IAttributeRepository attributeRepository, 
            IUniquenessAttributeCodeValidator validator, 
            IUnitOfWork unitOfWork)
        {
            _attributeRepository = attributeRepository;
            _Validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<long>> Handle(CreateAttributeCommand command, CancellationToken cancellationToken)
        {
            Result codeValidation = await _Validator.ValidateAsync(
                command.Code, 
                cancellationToken: cancellationToken);

            if (codeValidation.IsFailure)
            {
                return Result.Failure<long>(codeValidation.Error);
            }

            Domain.Entities.Products.Attribute attribute = Domain.Entities.Products.Attribute.Create(
                command.Code,
                command.Name,
                command.Description,
                command.DataType,
                command.IsVariant,
                command.IsFilterable,
                command.IsRequired,
                command.DisplayOrder,
                command.IsActive);

            await _attributeRepository.AddAsync(attribute, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(attribute.Id);
        }
    }
}

using FluentValidation;

namespace Application.Products.Attributes.Update
{
    public sealed class UpdateAttributeCommandValidator : AbstractValidator<UpdateAttributeCommand>
    {
        public UpdateAttributeCommandValidator()
        {
            RuleFor(x => x.AttributeId).NotEmpty().WithMessage("AttributeId is required.")
                .GreaterThan(0).WithMessage("AttributeId must be greater than 0.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.")
                .MaximumLength(50).WithMessage("Code must not exceed 50 characters.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(255).WithMessage("Description must not exceed 255 characters.");

            RuleFor(x => x.DataType)
                .NotEmpty().WithMessage("DataType is required.")
                .MaximumLength(20).WithMessage("DataType must not exceed 20 characters.");

            RuleFor(x => x.IsVariant)
                .NotNull().WithMessage("IsVariant is required.");

            RuleFor(x => x.IsFilterable)
                .NotNull().WithMessage("IsFilterable is required.");

            RuleFor(x => x.IsRequired)
                .NotNull().WithMessage("IsRequired is required.");

            RuleFor(x => x.DisplayOrder).NotNull()
                .GreaterThanOrEqualTo(0).WithMessage("DisplayOrder must be greater than or equal to 0.");
        }
    }
}

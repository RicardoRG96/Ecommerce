using FluentValidation;

namespace Application.Products.AttributeValues.Update
{
    public sealed class UpdateAttributeValueCommandValidator : AbstractValidator<UpdateAttributeValueCommand>
    {
        public UpdateAttributeValueCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Attribute value ID must be provided.")
                .GreaterThan(0).WithMessage("Attribute value ID must be greater than zero.");

            RuleFor(c => c.AttributeId)
                .NotEmpty().WithMessage("AttributeId is required.")
                .GreaterThan(0).WithMessage("AttributeId must be greater than zero.");

            RuleFor(c => c.Value)
                .NotEmpty().WithMessage("Value is required.")
                .MaximumLength(100).WithMessage("Value must not exceed 100 characters.");

            RuleFor(c => c.DisplayOrder)
                .NotEmpty().WithMessage("DisplayOrder is required.")
                .GreaterThanOrEqualTo(0).WithMessage("DisplayOrder must be zero or greater.");
        }
    }
}

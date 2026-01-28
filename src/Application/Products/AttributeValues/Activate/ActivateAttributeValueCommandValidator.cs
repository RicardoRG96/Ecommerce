using FluentValidation;

namespace Application.Products.AttributeValues.Activate
{
    public sealed class ActivateAttributeValueCommandValidator : AbstractValidator<ActivateAttributeValueCommand>
    {
        public ActivateAttributeValueCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Attribute value ID must be provided.")
                .GreaterThan(0).WithMessage("Attribute value ID must be greater than zero.");
        }
    }
}

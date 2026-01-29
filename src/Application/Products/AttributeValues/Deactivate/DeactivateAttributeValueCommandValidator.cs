using FluentValidation;

namespace Application.Products.AttributeValues.Deactivate
{
    public sealed class DeactivateAttributeValueCommandValidator : AbstractValidator<DeactivateAttributeValueCommand>
    {
        public DeactivateAttributeValueCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Attribute value ID must be provided.")
                .GreaterThan(0).WithMessage("Attribute value ID must be greater than zero.");
        }
    }
}

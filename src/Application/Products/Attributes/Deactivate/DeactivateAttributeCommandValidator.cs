using FluentValidation;

namespace Application.Products.Attributes.Deactivate
{
    public sealed class DeactivateAttributeCommandValidator : AbstractValidator<DeactivateAttributeCommand>
    {
        public DeactivateAttributeCommandValidator()
        {
            RuleFor(c => c.AttributeId)
                .NotEmpty().WithMessage("AttributeId is required.")
                .GreaterThan(0).WithMessage("AttributeId must be greater than zero.");
        }
    }
}

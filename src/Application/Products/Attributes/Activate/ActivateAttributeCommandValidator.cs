using FluentValidation;

namespace Application.Products.Attributes.Activate
{
    public sealed class ActivateAttributeCommandValidator : AbstractValidator<ActivateAttributeCommand>
    {
        public ActivateAttributeCommandValidator()
        {
            RuleFor(x => x.AttributeId).NotEmpty().WithMessage("AttributeId is required.")
                .GreaterThan(0).WithMessage("AttributeId must be greater than zero.");
        }
    }
}

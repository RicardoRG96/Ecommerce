using FluentValidation;

namespace Application.Products.ProductAttributeValues.Remove
{
    public sealed class RemoveAttributeValueFromSkuCommandValidator 
        : AbstractValidator<RemoveAttributeValueFromSkuCommand>
    {
        public RemoveAttributeValueFromSkuCommandValidator()
        {
            RuleFor(c => c.SkuId)
                .NotEmpty().WithMessage("Product SKU ID must not be empty.")
                .GreaterThan(0).WithMessage("Product SKU ID must be greater than zero.");

            RuleFor(c => c.AttributeValueId)
                .NotEmpty().WithMessage("Attribute Value ID must not be empty.")
                .GreaterThan(0).WithMessage("Attribute Value ID must be greater than zero.");
        }
    }
}

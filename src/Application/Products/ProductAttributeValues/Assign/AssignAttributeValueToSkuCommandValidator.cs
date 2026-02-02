using FluentValidation;

namespace Application.Products.ProductAttributeValues.Assign
{
    public sealed class AssignAttributeValueToSkuCommandValidator 
        : AbstractValidator<AssignAttributeValueToSkuCommand>
    {
        public AssignAttributeValueToSkuCommandValidator()
        {
            RuleFor(c => c.SkuId)
                .NotEmpty().WithMessage("SKU ID must be provided.")
                .GreaterThan(0).WithMessage("SKU ID must be greater than zero.");

            RuleFor(c => c.AttributeValueId)
                .NotEmpty().WithMessage("Attribute Value ID must be provided.")
                .GreaterThan(0).WithMessage("Attribute Value ID must be greater than zero.");
        }
    }
}

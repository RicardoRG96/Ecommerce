using FluentValidation;

namespace Application.Products.ProductSkus.Deactivate
{
    public sealed class DeactivateProductSkuCommandValidator : AbstractValidator<DeactivateProductSkuCommand>
    {
        public DeactivateProductSkuCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Product SKU ID is required.")
                .GreaterThan(0).WithMessage("Product SKU ID must be greater than zero.");
        }
    }
}

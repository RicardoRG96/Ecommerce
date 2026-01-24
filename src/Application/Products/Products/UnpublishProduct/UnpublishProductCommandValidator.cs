using FluentValidation;

namespace Application.Products.Products.UnpublishProduct
{
    public sealed class UnpublishProductCommandValidator : AbstractValidator<UnpublishProductCommand>
    {
        public UnpublishProductCommandValidator()
        {
            RuleFor(c => c.ProductId).NotEmpty().WithMessage("Product ID is required.")
                .GreaterThan(0).WithMessage("Product ID must be a positive number.");
        }
    }
}

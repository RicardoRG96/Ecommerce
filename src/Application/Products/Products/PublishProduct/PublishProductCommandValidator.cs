using FluentValidation;

namespace Application.Products.Products.PublishProduct
{
    public sealed class PublishProductCommandValidator : AbstractValidator<PublishProductCommand>
    {
        public PublishProductCommandValidator()
        {
            RuleFor(c => c.ProductId).NotEmpty().WithMessage("Product ID is required.")
                .GreaterThan(0).WithMessage("Product ID must be a positive number.");
        }
    }
}

using FluentValidation;

namespace Application.Products.ProductSkus.Update
{
    public sealed class UpdateProductSkuCommandValidator : AbstractValidator<UpdateProductSkuCommand>
    {
        public UpdateProductSkuCommandValidator()
        {
            RuleFor(c => c.ProductId)
                .NotEmpty().WithMessage("ProductId is required.")
                .GreaterThan(0).WithMessage("ProductId must be greater than zero.");

            RuleFor(c => c.BarCode)
                .MaximumLength(50).WithMessage("BarCode cannot exceed 50 characters.");

            RuleFor(c => c.Price)
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to zero.");

            RuleFor(c => c.Cost)
                .GreaterThanOrEqualTo(0).When(c => c.Cost.HasValue)
                .WithMessage("Cost must be greater than or equal to zero.");

            RuleFor(c => c.Weight)
                .GreaterThanOrEqualTo(0).When(c => c.Weight.HasValue)
                .WithMessage("Weight must be greater than or equal to zero.");

            RuleFor(c => c.Length)
                .GreaterThanOrEqualTo(0).When(c => c.Length.HasValue)
                .WithMessage("Length must be greater than or equal to zero.");

            RuleFor(c => c.Width)
                .GreaterThanOrEqualTo(0).When(c => c.Width.HasValue)
                .WithMessage("Width must be greater than or equal to zero.");

            RuleFor(c => c.Height)
                .GreaterThanOrEqualTo(0).When(c => c.Height.HasValue)
                .WithMessage("Height must be greater than or equal to zero.");

            RuleFor(c => c.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("DisplayOrder must be greater than or equal to zero.");
        }
    }
}

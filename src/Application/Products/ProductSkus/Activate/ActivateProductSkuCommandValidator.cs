using FluentValidation;

namespace Application.Products.ProductSkus.Activate
{
    public sealed class ActivateProductSkuCommandValidator 
        : AbstractValidator<ActivateProductSkuCommand>
    {
        public ActivateProductSkuCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Id is required.")
                .GreaterThan(0).WithMessage("Id must be greater than zero.");
        }
    }
}

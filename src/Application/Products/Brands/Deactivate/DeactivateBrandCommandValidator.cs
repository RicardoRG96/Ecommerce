using FluentValidation;

namespace Application.Products.Brands.Deactivate
{
    public sealed class DeactivateBrandCommandValidator : AbstractValidator<DeactivateBrandCommand>
    {
        public DeactivateBrandCommandValidator()
        {
            RuleFor(x => x.BrandId).NotEmpty();
        }
    }
}

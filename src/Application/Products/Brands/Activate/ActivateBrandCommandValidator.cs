using FluentValidation;

namespace Application.Products.Brands.Activate
{
    public sealed class ActivateBrandCommandValidator : AbstractValidator<ActivateBrandCommand>
    {
        public ActivateBrandCommandValidator()
        {
            RuleFor(x => x.BrandId).NotEmpty();
        }
    }
}

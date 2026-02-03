using FluentValidation;

namespace Application.Products.Products.FilterByBrand
{
    public sealed class FilterByBrandQueryValidator : AbstractValidator<FilterByBrandQuery>
    {
        public FilterByBrandQueryValidator()
        {
            RuleFor(q => q.Brands)
                .NotEmpty().WithMessage("Brands list must not be empty.")
                .Must(brands => brands.All(brand => !string.IsNullOrWhiteSpace(brand)))
                .WithMessage("Brands list must not contain empty or whitespace-only strings.");
        }
    }
}

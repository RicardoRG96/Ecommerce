using FluentValidation;

namespace Application.Products.Products.FilterByPriceRange
{
    public sealed class FilterByPriceRangeQueryValidator : AbstractValidator<FilterByPriceRangeQuery>
    {
        public FilterByPriceRangeQueryValidator()
        {
            RuleFor(x => x.MinPrice)
                .NotEmpty().WithMessage("MinPrice is required.")
                .GreaterThanOrEqualTo(0)
                .WithMessage("MinPrice must be greater than or equal to 0.");

            RuleFor(x => x.MaxPrice)
                .NotEmpty().WithMessage("MaxPrice is required.")
                .GreaterThanOrEqualTo(x => x.MinPrice)
                .WithMessage("MaxPrice must be greater than or equal to MinPrice.");
        }
    }
}

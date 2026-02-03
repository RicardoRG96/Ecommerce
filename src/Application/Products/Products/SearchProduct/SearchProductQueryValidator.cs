using FluentValidation;

namespace Application.Products.Products.SearchProduct
{
    public sealed class SearchProductQueryValidator : AbstractValidator<SearchProductQuery>
    {
        public SearchProductQueryValidator()
        {
            RuleFor(x => x.Query)
                .NotEmpty().WithMessage("Search query must not be empty.")
                .MaximumLength(100).WithMessage("Search query must not exceed 100 characters.");
        }
    }
}

using FluentValidation;

namespace Application.Products.Products.FilterByCategory
{
    public sealed class FilterByCategoryQueryValidator : AbstractValidator<FilterByCategoryQuery>
    {
        public FilterByCategoryQueryValidator()
        {
            RuleFor(x => x.Categories)
                .NotEmpty().WithMessage("Categories list must not be empty.")
                .Must(categories => categories.All(c => !string.IsNullOrWhiteSpace(c)))
                .WithMessage("Categories must not contain null or whitespace values.");
        }
    }
}

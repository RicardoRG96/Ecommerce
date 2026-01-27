using FluentValidation;

namespace Application.Products.Products.GetAllPublishedProductsWithPagination
{
    public sealed class GetAllPublishedProductsWithPaginationQueryValidator : AbstractValidator<GetAllPublishedProductsWithPaginationQuery>
    {
        public GetAllPublishedProductsWithPaginationQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .LessThanOrEqualTo(100).WithMessage("PageSize must be less than or equal to 100.");
        }
    }
}

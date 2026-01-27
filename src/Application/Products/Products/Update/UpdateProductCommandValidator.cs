using FluentValidation;

namespace Application.Products.Products.Update
{
    public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(c => c.ProductId).NotEmpty().WithMessage("Product ID is required.")
                .GreaterThan(0).WithMessage("Product ID must be a positive number.");

            RuleFor(c => c.Name).NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(200).WithMessage("Product name must not exceed 200 characters.");

            RuleFor(c => c.ShortDescription)
                .MaximumLength(500).WithMessage("Short description must not exceed 500 characters.");

            RuleFor(c => c.Description)
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");

            RuleFor(c => c.BrandId).NotEmpty().WithMessage("Brand ID is required.")
                .GreaterThan(0).WithMessage("Brand ID must be a positive number.");

            RuleFor(c => c.CategoryId).NotEmpty().WithMessage("Category ID is required.")
                .GreaterThan(0).WithMessage("Category ID must be a positive number.");

            RuleFor(c => c.ProductTaxCategoryId).NotEmpty().WithMessage("Product Tax Category ID is required.")
                .GreaterThan(0).WithMessage("Product Tax Category ID must be a positive number.");

            RuleFor(c => c.IsDigital).NotNull().WithMessage("IsDigital flag must be specified.");

            RuleFor(c => c.MetaTitle)
                .MaximumLength(200).WithMessage("Meta title must not exceed 200 characters.");

            RuleFor(c => c.MetaDescription)
                .MaximumLength(500).WithMessage("Meta description must not exceed 500 characters.");

            RuleFor(c => c.MetaKeywords)
                .MaximumLength(500).WithMessage("Meta keywords must not exceed 500 characters.");
        }
    }
}

using FluentValidation;

namespace Application.Products.Brands.Delete
{
    public sealed class DeleteBrandCommandValidator : AbstractValidator<DeleteBrandCommand>
    {
        public DeleteBrandCommandValidator()
        {
            RuleFor(c => c.BrandId).NotEmpty();
        }
    }
}

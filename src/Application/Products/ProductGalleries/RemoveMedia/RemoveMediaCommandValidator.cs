using FluentValidation;

namespace Application.Products.ProductGalleries.RemoveMedia
{
    public sealed class RemoveMediaCommandValidator : AbstractValidator<RemoveMediaCommand>
    {
        public RemoveMediaCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotNull().WithMessage("ProductGallery ID is required.")
                .GreaterThan(0).WithMessage("Invalid ProductGallery ID.");
        }
    }
}

using FluentValidation;

namespace Application.Products.ProductGalleries.SetPrimaryMedia
{
    public sealed class SetPrimaryMediaCommandValidator : AbstractValidator<SetPrimaryMediaCommand>
    {
        public SetPrimaryMediaCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotNull().WithMessage("ProductGallery ID is required.")
                .GreaterThan(0).WithMessage("Invalid ProductGallery ID.");
        }
    }
}

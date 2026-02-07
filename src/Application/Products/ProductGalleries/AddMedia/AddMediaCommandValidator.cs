using FluentValidation;

namespace Application.Products.ProductGalleries.AddMedia
{
    public sealed class AddMediaCommandValidator : AbstractValidator<AddMediaCommand>
    {
        public AddMediaCommandValidator()
        {
            RuleFor(c => c.Stream).NotNull().WithMessage("Stream cannot be null.");

            RuleFor(c => c.FileName).NotEmpty().WithMessage("FileName cannot be empty.");

            RuleFor(c => c.ContentType).NotEmpty().WithMessage("ContentType cannot be empty.");
        }
    }
}

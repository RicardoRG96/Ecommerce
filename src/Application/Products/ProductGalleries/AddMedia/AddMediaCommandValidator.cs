using FluentValidation;

namespace Application.Products.ProductGalleries.AddMedia
{
    public sealed class AddMediaCommandValidator : AbstractValidator<AddMediaCommand>
    {
        public AddMediaCommandValidator()
        {
            RuleFor(c => c.ProductId)
                .NotNull().WithMessage("ProductId cannot be empty.")
                .GreaterThan(0).WithMessage("ProductId must be greater than 0.");

            RuleFor(c => c.Stream).NotNull().WithMessage("Stream cannot be null.");

            RuleFor(c => c.FileName).NotEmpty().WithMessage("FileName cannot be empty.");

            RuleFor(c => c.ContentType).NotEmpty().WithMessage("ContentType cannot be empty.");

            RuleFor(c => c.IsPrimary)
                .NotNull().WithMessage("IsPrimary cannot be null.");

            RuleFor(c => c.DisplayOrder)
                .NotNull().WithMessage("DisplayOrder cannot be empty.")
                .GreaterThanOrEqualTo(0).WithMessage("DisplayOrder must be greater than or equal to 0.");

            RuleFor(c => c.AltText)
                .MaximumLength(200).WithMessage("AltText cannot exceed 200 characters.");
        }
    }
}

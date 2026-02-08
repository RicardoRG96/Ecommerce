using FluentValidation;

namespace Application.Products.ProductGalleries.ReorderGallery
{
    public sealed class ReorderGalleryCommandValidator : AbstractValidator<ReorderGalleryCommand>
    {
        public ReorderGalleryCommandValidator()
        {
            RuleFor(c => c.ProductId)
                .NotNull().WithMessage("ProductId is required.")
                .GreaterThan(0).WithMessage("ProductId must be greater than zero.");

            RuleFor(c => c.OrderedGalleryItemIds)
                .NotNull().WithMessage("OrderedGalleryItemIds is required.")
                .Must(ids => ids.Length > 0).WithMessage("OrderedGalleryItemIds must contain at least one ID.");
        }
    }
}

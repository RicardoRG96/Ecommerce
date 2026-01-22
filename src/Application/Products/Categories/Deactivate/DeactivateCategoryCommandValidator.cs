using FluentValidation;

namespace Application.Products.Categories.Deactivate
{
    public sealed class DeactivateCategoryCommandValidator : AbstractValidator<DeactivateCategoryCommand>
    {
        public DeactivateCategoryCommandValidator()
        {
            RuleFor(c => c.CategoryId)
                .NotEmpty().WithMessage("El ID de la categoría no puede estar vacío.");
        }
    }
}

using FluentValidation;

namespace Application.Products.Categories.Activate
{
    public sealed class ActivateCategoryCommandValidator : AbstractValidator<ActivateCategoryCommand>
    {
        public ActivateCategoryCommandValidator()
        {
            RuleFor(c => c.CategoryId)
                .NotEmpty().WithMessage("El ID de la categoría no puede estar vacío.");
        }
    }
}

using FluentValidation;

namespace Application.Products.Categories.ChangeCategoryParent
{
    public sealed class ChangeCategoryParentCommandValidator : AbstractValidator<ChangeCategoryParentCommand>
    {
        public ChangeCategoryParentCommandValidator()
        {
            RuleFor(c => c.CategoryId)
                .NotEmpty().WithMessage("El ID de la categoría no puede estar vacío.");

            RuleFor(c => c.NewParentId)
                .NotEmpty().WithMessage("El ID del nuevo padre no puede estar vacío.");
        }
    }
}

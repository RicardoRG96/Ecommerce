using FluentValidation;

namespace Application.Products.Categories.ChangeCategoryParent
{
    public sealed class ChangeCategoryParentCommandValidator : AbstractValidator<ChangeCategoryParentCommand>
    {
        public ChangeCategoryParentCommandValidator()
        {
            RuleFor(c => c.CategoryId)
                .NotEmpty().WithMessage("El ID de la categoría no puede estar vacío.");
        }
    }
}

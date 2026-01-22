using FluentValidation;

namespace Application.Products.Categories.Update
{
    public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("El nombre de la categoría no puede estar vacío.")
                .MaximumLength(150).WithMessage("El nombre de la categoría no puede exceder los 150 caracteres.");

            RuleFor(c => c.Description).MaximumLength(1000)
                .WithMessage("La descripción de la categoría no puede exceder los 1000 caracteres.");

            RuleFor(c => c.ImageUrl).MaximumLength(500)
                .WithMessage("La URL de la imagen no puede exceder los 500 caracteres.");

            RuleFor(c => c.Icon).MaximumLength(100)
                .WithMessage("El ícono de la categoría no puede exceder los 100 caracteres.");

            RuleFor(c => c.DisplayOrder).NotEmpty()
                .WithMessage("El orden de visualización no puede estar vacío.");

            RuleFor(c => c.IsVisibleInMenu).NotNull()
                .WithMessage("La visibilidad en el menú de la categoría debe ser especificada.");

            RuleFor(c => c.MetaTitle).MaximumLength(160)
                .WithMessage("El título meta no puede exceder los 160 caracteres.");

            RuleFor(c => c.Description).MaximumLength(300)
                .WithMessage("El título meta no puede exceder los 300 caracteres.");

            RuleFor(c => c.MetaKeywords).MaximumLength(500)
                .WithMessage("Las palabras clave meta no pueden exceder los 500 caracteres.");
        }
    }
}

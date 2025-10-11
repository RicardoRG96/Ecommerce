using FluentValidation;

namespace Application.Users.Countries.Create
{
    internal sealed class CreateCountryCommandValidator : AbstractValidator<CreateCountryCommand>
    {
        public CreateCountryCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MaximumLength(70);
        }
    }
}

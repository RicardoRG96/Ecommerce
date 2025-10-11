using FluentValidation;

namespace Application.Users.Countries.Delete
{
    internal sealed class DeleteCountryCommandValidator : AbstractValidator<DeleteCountryCommand>
    {
        public DeleteCountryCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
        }
    }
}

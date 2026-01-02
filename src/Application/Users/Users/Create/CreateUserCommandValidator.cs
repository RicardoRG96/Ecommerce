using FluentValidation;

namespace Application.Users.Users.Create
{
    internal sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(c => c.Email).NotEmpty().EmailAddress().MaximumLength(150);
            RuleFor(c => c.Password).NotEmpty().MaximumLength(150);
        }
    }
}

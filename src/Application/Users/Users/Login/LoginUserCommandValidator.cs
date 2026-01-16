using FluentValidation;

namespace Application.Users.Users.Login
{
    internal sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(c => c.Email).NotEmpty().EmailAddress().MaximumLength(100);
            RuleFor(c => c.Password).NotEmpty().MaximumLength(80);
        }
    }
}

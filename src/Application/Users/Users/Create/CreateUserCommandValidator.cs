using FluentValidation;

namespace Application.Users.Users.Create
{
    internal sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(c => c.LastName).NotEmpty().MaximumLength(100);
            RuleFor(c => c.UserName).NotEmpty().MaximumLength(80);
            RuleFor(c => c.Email).NotEmpty().EmailAddress().MaximumLength(200);
            RuleFor(c => c.Password).NotEmpty().MinimumLength(8).MaximumLength(80);
            RuleFor(c => c.DateOfBirth).NotEmpty();
            RuleFor(c => c.PhoneNumber).NotEmpty().MaximumLength(50);
        }
    }
}

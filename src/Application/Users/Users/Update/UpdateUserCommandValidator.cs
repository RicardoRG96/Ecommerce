using FluentValidation;

namespace Application.Users.Users.Update
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.FirstName).NotEmpty().MaximumLength(80);
            RuleFor(c => c.LastName).NotEmpty().MaximumLength(80);
            RuleFor(c => c.PhoneNumber).NotEmpty().Length(9);
        }
    }
}

using FluentValidation;

namespace Application.Users.Users.Update
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.FirstName).MaximumLength(80);
            RuleFor(c => c.LastName).MaximumLength(80);
            RuleFor(c => c.PhoneNumber).Length(9);
        }
    }
}

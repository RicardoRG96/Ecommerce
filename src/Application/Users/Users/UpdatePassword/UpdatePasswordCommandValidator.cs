using FluentValidation;

namespace Application.Users.Users.UpdatePassword
{
    public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
    {
        public UpdatePasswordCommandValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.CurrentPassword).NotEmpty().MaximumLength(100);
            RuleFor(c => c.NewPassword).NotEmpty().MaximumLength(100);
        }
    }
}

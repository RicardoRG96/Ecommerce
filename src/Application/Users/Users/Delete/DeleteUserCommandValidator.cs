using FluentValidation;

namespace Application.Users.Users.Delete
{
    internal sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
        }
    }
}

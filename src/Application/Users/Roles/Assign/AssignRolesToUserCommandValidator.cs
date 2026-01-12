using FluentValidation;

namespace Application.Users.Roles.Assign
{
    internal sealed class AssignRolesToUserCommandValidator : AbstractValidator<AssingRolesToUserCommand>
    {
        public AssignRolesToUserCommandValidator()
        {
            RuleFor(c => c.Roles).NotEmpty();
            RuleFor(c => c.UserId).NotEmpty();
        }
    }
}

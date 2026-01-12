using FluentValidation;

namespace Application.Users.Roles.Assign
{
    internal sealed class AssignRolesToUserCommandValidator : AbstractValidator<AssignRolesToUserCommand>
    {
        public AssignRolesToUserCommandValidator()
        {
            RuleFor(c => c.Roles).NotEmpty();
            //RuleFor(c => c.Roles).ForEach(role => role.NotEmpty());
            RuleFor(c => c.UserId).NotEmpty();
        }
    }
}

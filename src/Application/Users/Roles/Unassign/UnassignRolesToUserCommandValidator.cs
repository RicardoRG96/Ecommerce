using FluentValidation;

namespace Application.Users.Roles.Unassign
{
    internal sealed class UnassignRolesToUserCommandValidator : AbstractValidator<UnassignRolesToUserCommand>
    {
        public UnassignRolesToUserCommandValidator()
        {
            RuleFor(c => c.Roles).NotEmpty();
            RuleFor(c => c.Roles).ForEach(role => role.NotEmpty());
            RuleFor(c => c.UserId).NotEmpty();
        }
    }
}

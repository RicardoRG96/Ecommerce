using FluentValidation;

namespace Application.Users.Roles.UnassignPermission
{
    internal sealed class UnassignPermissionToRoleCommandValidator : AbstractValidator<UnassignPermissionToRoleCommand>
    {
        public UnassignPermissionToRoleCommandValidator()
        {
            RuleFor(c => c.Role).NotEmpty().MaximumLength(100);
            RuleFor(c => c.Permission).NotEmpty().MaximumLength(100);
        }
    }
}

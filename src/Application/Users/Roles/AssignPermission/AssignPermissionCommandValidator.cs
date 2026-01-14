using FluentValidation;

namespace Application.Users.Roles.AssignPermission
{
    internal sealed class AssignPermissionCommandValidator : AbstractValidator<AssignPermissionCommand>
    {
        public AssignPermissionCommandValidator()
        {
            RuleFor(c => c.Role).NotEmpty().MaximumLength(100);
            RuleFor(c => c.Permission).NotEmpty().MaximumLength(100);
        }
    }
}

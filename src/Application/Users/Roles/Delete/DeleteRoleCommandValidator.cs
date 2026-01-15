using FluentValidation;

namespace Application.Users.Roles.Delete
{
    internal sealed class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
        }
    }
}

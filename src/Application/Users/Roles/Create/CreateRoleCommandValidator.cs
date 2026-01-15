using FluentValidation;

namespace Application.Users.Roles.Create
{
    internal sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MaximumLength(100);
        }
    }
}

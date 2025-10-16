using FluentValidation;

namespace Application.Users.Municipalities.Delete
{
    public sealed class DeleteMunicipalityCommandValidator : AbstractValidator<DeleteMunicipalityCommand>
    {
        public DeleteMunicipalityCommandValidator()
        {
            RuleFor(c => c.MunicipalityId).NotEmpty();
        }
    }
}

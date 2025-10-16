using FluentValidation;

namespace Application.Users.Municipalities.Create
{
    public sealed class CreateMunicipalityCommandValidator : AbstractValidator<CreateMunicipalityCommand>
    {
        public CreateMunicipalityCommandValidator()
        {
            RuleFor(c => c.RegionId).NotEmpty();
            RuleFor(c => c.Name).NotEmpty().MaximumLength(70);
        }
    }
}

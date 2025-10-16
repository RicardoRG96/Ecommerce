using FluentValidation;

namespace Application.Users.Municipalities.Update
{
    public sealed class UpdateMunicipalityCommandValidator : AbstractValidator<UpdateMunicipalityCommand>
    {
        public UpdateMunicipalityCommandValidator()
        {
            RuleFor(c => c.MunicipalityId).NotEmpty();
            RuleFor(c => c.RegionId).NotEmpty();
            RuleFor(c => c.Name).NotEmpty().MaximumLength(70);
        }
    }
}

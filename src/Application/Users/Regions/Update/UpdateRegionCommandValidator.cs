using FluentValidation;

namespace Application.Users.Regions.Update
{
    public sealed class UpdateRegionCommandValidator : AbstractValidator<UpdateRegionCommand>
    {
        public UpdateRegionCommandValidator()
        {
            RuleFor(c => c.RegionId).NotEmpty();
            RuleFor(c => c.Name).NotEmpty().MaximumLength(70);
        }
    }
}

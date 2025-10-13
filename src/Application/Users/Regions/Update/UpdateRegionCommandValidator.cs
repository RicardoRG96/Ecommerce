using FluentValidation;

namespace Application.Users.Regions.Update
{
    public sealed class UpdateRegionCommandValidator : AbstractValidator<UpdateRegionCommand>
    {
        public UpdateRegionCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MaximumLength(70);
        }
    }
}

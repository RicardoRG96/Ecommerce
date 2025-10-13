using FluentValidation;

namespace Application.Users.Regions.Create
{
    public sealed class CreateRegionCommandValidator : AbstractValidator<CreateRegionCommand>
    {
        public CreateRegionCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MaximumLength(70);
        }
    }
}

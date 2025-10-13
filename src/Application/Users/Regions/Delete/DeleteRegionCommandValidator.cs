using FluentValidation;

namespace Application.Users.Regions.Delete
{
    public sealed class DeleteRegionCommandValidator : AbstractValidator<DeleteRegionCommand>
    {
        public DeleteRegionCommandValidator()
        {
            RuleFor(c => c.RegionId).NotEmpty();
        }
    }
}

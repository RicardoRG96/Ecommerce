using FluentValidation;

namespace Application.Users.Addresses.AssignDefaultAddress
{
    public class AssignDefaultAddressCommandValidator : AbstractValidator<AssignDefaultAddressCommand>
    {
        public AssignDefaultAddressCommandValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.AddressId).NotEmpty();
        }
    }
}

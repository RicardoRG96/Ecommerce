using FluentValidation;

namespace Application.Users.Addresses.Delete
{
    public class DeleteAddressCommandValidator : AbstractValidator<DeleteAddressCommand>
    {
        public DeleteAddressCommandValidator()
        {
            RuleFor(c => c.AddressId).NotEmpty();
        }
    }
}

using FluentValidation;

namespace Application.Users.Addresses.Create
{
    public sealed class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
    {
        public CreateAddressCommandValidator()
        {
            RuleFor(c => c.CountryId).NotEmpty();
            RuleFor(c => c.MunicipalityId).NotEmpty();
            RuleFor(c => c.Title).NotEmpty().MaximumLength(40);
            RuleFor(c => c.City).NotEmpty().MaximumLength(60);
            RuleFor(c => c.Street).NotEmpty().MaximumLength(60);
            RuleFor(c => c.Number).NotEmpty().MaximumLength(60);
            RuleFor(c => c.Apartament).MaximumLength(30);
            RuleFor(c => c.Reference).MaximumLength(70);
            RuleFor(c => c.PostalCode).NotEmpty().MaximumLength(40);
        }
    }
}

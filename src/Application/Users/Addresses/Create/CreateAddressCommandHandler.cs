using Application.Abstractions.Authentication;
using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Addresses.Create
{
    internal sealed class CreateAddressCommandHandler : ICommandHandler<CreateAddressCommand, long>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMunicipalityRepository _municipalityRepository;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAddressCommandHandler(
            IAddressRepository addressRepository,
            ICountryRepository countryRepository,
            IMunicipalityRepository municipalityRepository,
            IUserContext userContext,
            IUnitOfWork unitOfWork)
        {
            _addressRepository = addressRepository;
            _countryRepository = countryRepository;
            _municipalityRepository = municipalityRepository;
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<long>> Handle(CreateAddressCommand command, CancellationToken cancellationToken)
        {
            Country? country = await _countryRepository.GetByIdAsync(command.CountryId, cancellationToken);

            if (country is null)
            {
                return Result.Failure<long>(CountryErrors.NotFound(command.CountryId));
            }
            
            Municipality? municipality = await _municipalityRepository.GetByIdAsync(command.MunicipalityId, cancellationToken);

            if (municipality is null)
            {
                return Result.Failure<long>(MunicipalityErrors.NotFound(command.MunicipalityId));
            }

            Address address = new()
            {
                Country = country,
                Municipality = municipality,
                Title = command.Title,
                City = command.City,
                Street = command.Street,
                Number = command.Number,
                Apartament = command.Apartament,
                Reference = command.Reference,
                PostalCode = command.PostalCode
            };

            await _addressRepository.AddAsync(address, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(address.AddressId);
        }
    }
}

using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Application.Users.Countries.GetById;
using Application.Users.Municipalities.GetById;
using Application.Users.Regions.GetById;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Addresses.GetById
{
    internal sealed class GetAddressByIdQueryHandler : IQueryHandler<GetAddressByIdQuery, AddressResponse>
    {
        private readonly IAddressRepository _addressRepository;

        public GetAddressByIdQueryHandler(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<Result<AddressResponse>> Handle(GetAddressByIdQuery query, CancellationToken cancellationToken)
        {
            Address? address = await _addressRepository.GetByIdAsync(query.AddressId, cancellationToken);

            if (address is null)
            {
                return Result.Failure<AddressResponse>(AddressErrors.NotFound(query.AddressId));
            }

            AddressResponse response = MapToAddressResponse(address);

            return Result.Success(response);
        }

        private AddressResponse MapToAddressResponse(Address address)
        {
            CountryResponse countryResponse = new()
            {
                Id = address.CountryId,
                Name = address.Country.Name!
            };

            RegionResponse regionResponse = new()
            {
                Id = address.Municipality.Region.RegionId,
                Name = address.Municipality.Region.Name!
            };

            MunicipalityResponse municipalityResponse = new()
            {
                Id = address.Municipality.MunicipalityId,
                Region = regionResponse,
                Name = address.Municipality.Name!
            };

            AddressResponse response = new()
            {
                Id = address.AddressId,
                Country = countryResponse,
                Municipality = municipalityResponse,
                Title = address.Title!,
                Street = address.Street!,
                Number = address.Number!,
                Apartament = address.Apartament!,
                Reference = address.Reference!,
                PostalCode = address.PostalCode!
            };

            return response;
        }
    }
}

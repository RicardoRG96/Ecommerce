using Application.Users.Countries.GetById;
using Application.Users.Municipalities.GetById;
using Application.Users.Regions.GetById;
using Domain.Entities.Users;

namespace Application.Users.Addresses
{
    internal static class AddressToAddressResponseMapper
    {
        public static AddressResponse Map(Address address)
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

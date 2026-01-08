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
                City = address.City!,
                Street = address.Street!,
                Number = address.Number!,
                Apartament = address.Apartament!,
                Reference = address.Reference!,
                PostalCode = address.PostalCode!
            };

            return response;
        }

        public static AddressResponse Map(AddressUser addressUser)
        {
            CountryResponse countryResponse = new()
            {
                Id = addressUser.Address.CountryId,
                Name = addressUser.Address.Country.Name!
            };

            RegionResponse regionResponse = new()
            {
                Id = addressUser.Address.Municipality.Region.RegionId,
                Name = addressUser.Address.Municipality.Region.Name!
            };

            MunicipalityResponse municipalityResponse = new()
            {
                Id = addressUser.Address.Municipality.MunicipalityId,
                Region = regionResponse,
                Name = addressUser.Address.Municipality.Name!
            };

            AddressResponse response = new()
            {
                Id = addressUser.Address.AddressId,
                Country = countryResponse,
                Municipality = municipalityResponse,
                Title = addressUser.Address.Title!,
                City = addressUser.Address.City!,
                Street = addressUser.Address.Street!,
                Number = addressUser.Address.Number!,
                Apartament = addressUser.Address.Apartament!,
                Reference = addressUser.Address.Reference!,
                PostalCode = addressUser.Address.PostalCode!
            };

            return response;
        }
    }
}

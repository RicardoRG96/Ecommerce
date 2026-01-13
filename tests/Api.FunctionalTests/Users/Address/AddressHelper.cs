using Api.FunctionalTests.Abstractions;
using Application.Users.Addresses;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Address.Create;

namespace Api.FunctionalTests.Users.Address
{
    internal sealed class AddressHelper : BaseFunctionalTest
    {
        public AddressHelper(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        public async Task<long> CreateAddressForAdminUser()
        {
            CreateAddressRequest request =
                 new(1, 1, "AdminAddress", "TestCity", "TestStreet", "1010", "TestApartament", "TestReference", "TestPostalCode");

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, request);

            return await response.Content.ReadFromJsonAsync<long>();
        }

        public async Task<string> GetAddressTitleForAdminUser()
        {
            long createdAddressId = await CreateAddressForAdminUser();

            AddressResponse? address = await HttpClient.GetFromJsonAsync<AddressResponse>($"{addressesBaseUrl}/{createdAddressId}");

            return address!.Title;
        }
    }
}

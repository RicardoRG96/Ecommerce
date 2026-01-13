using Api.FunctionalTests.Abstractions;
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
    }
}

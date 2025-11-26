using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Address.Create;

namespace Api.FunctionalTests.Users.Address
{
    public class CreateAddressTests : BaseFunctionalTest
    {
        private static readonly CreateAddressRequest _request = 
            new(1, 1, "TestAddress", "TestCity", "TestStreet", "TestNumber", "TestApartament", "TestReference", "TestPostalCode");

        public CreateAddressTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenCountryIdIsMissing()
        {
            CreateAddressRequest invalidRequest = _request with { CountryId = 0 };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}

using Api.FunctionalTests.Abstractions;
using Application.Users.Addresses;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Address
{
    public class GetAddressByIdTests : BaseFunctionalTest
    {
        public GetAddressByIdTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenAddressIdDoesNotExist()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{addressesBaseUrl}/2500");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_And_Address_WhenAddressIdExists()
        {
            AddressResponse? address = await HttpClient.GetFromJsonAsync<AddressResponse>($"{addressesBaseUrl}/1");

            address.Should().NotBeNull();
        }
    }
}

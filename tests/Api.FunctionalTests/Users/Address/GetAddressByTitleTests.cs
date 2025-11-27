using Api.FunctionalTests.Abstractions;
using Application.Users.Addresses;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Address
{
    public class GetAddressByTitleTests : BaseFunctionalTest
    {
        public GetAddressByTitleTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenAddressTitleDoesNotExist()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{addressesBaseUrl}/title/noTitle");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_And_Address_WhenAddressTitleExists()
        {
            AddressResponse? address = await HttpClient.GetFromJsonAsync<AddressResponse>($"{addressesBaseUrl}/title/Casa 1");

            address.Should().NotBeNull();
        }
    }
}

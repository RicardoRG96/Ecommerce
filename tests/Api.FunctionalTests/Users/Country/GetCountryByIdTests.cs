using Api.FunctionalTests.Abstractions;
using Application.Users.Countries.GetById;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Country
{
    public class GetCountryByIdTests : BaseFunctionalTest
    {
        public GetCountryByIdTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenCountryIdDoesNotExist()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{countriesBaseUrl}/2500");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_AndCountry_WhenCountryExists()
        {
            CountryResponse? country = await HttpClient.GetFromJsonAsync<CountryResponse>($"{countriesBaseUrl}/1");

            country.Should().NotBeNull();
        }
    }
}

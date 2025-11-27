using Api.FunctionalTests.Abstractions;
using Application.Users.Countries.GetByName;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Country
{
    public class GetCountryByNameTests : BaseFunctionalTest
    {
        public GetCountryByNameTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenCountryNameDoesNotExist()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{countriesBaseUrl}/name/noCountry");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_And_Country_WhenCountryNameExists()
        {
            CountryResponse? country = await HttpClient.GetFromJsonAsync<CountryResponse>($"{countriesBaseUrl}/name/chile");

            country.Should().NotBeNull();
        }
    }
}

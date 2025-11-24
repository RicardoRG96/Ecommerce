using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Country.Create;

namespace Api.FunctionalTests.Users.Country
{
    public class CreateCountryTests : BaseFunctionalTest
    {
        private static readonly CreateCountryRequest _request = new("Chile");

        public CreateCountryTests(FunctionalTestWebAppFactory factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenCountryNameIsMissing()
        {
            CreateCountryRequest invalidRequest = _request with { Name = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{countriesBaseUrl}", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenCountryNameExceedsTheMaximumLength()
        {
            CreateCountryRequest invalidRequest =
                _request with { Name = "La República Federal del Crisantemo Esmeralda de los Montes Orientales del Viento" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(countriesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnOk_WhenRequestIsValid()
        {
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(countriesBaseUrl, _request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            long userId = await response.Content.ReadFromJsonAsync<long>();
                
            userId.Should().BeGreaterThan(0);
        }
    }
}

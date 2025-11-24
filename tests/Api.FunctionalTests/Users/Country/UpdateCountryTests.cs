using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Country.Create;
using Web.Api.Endpoints.v1.Users.Country.Update;

namespace Api.FunctionalTests.Users.Country
{
    public class UpdateCountryTests : BaseFunctionalTest
    {
        private static readonly UpdateCountryRequest _request = new("UpdatedName");

        public UpdateCountryTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenCountryIdIsMissing()
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{countriesBaseUrl}/0", _request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenCountryNameIsMissing()
        {
            UpdateCountryRequest invalidRequest = _request with { Name = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{countriesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenCountryNameExceedsTheMaximumLength()
        {
            UpdateCountryRequest invalidRequest =
                _request with { Name = "La República Federal del Crisantemo Esmeralda de los Montes Orientales del Viento" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{countriesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}

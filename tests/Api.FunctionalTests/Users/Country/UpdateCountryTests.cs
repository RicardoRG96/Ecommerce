using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Countries.GetById;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
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
                _request with { Name = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{countriesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenCountryIdDoesNotExist()
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{countriesBaseUrl}/{Constants.NotExistingId}", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenRequestIsValid_And_CountryIdExists()
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{countriesBaseUrl}/1", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_UpdateName_WhenRequestIsValid_And_CountryIdExists()
        {
            await HttpClient.PutAsJsonAsync($"{countriesBaseUrl}/1", _request);

            CountryResponse? country = await HttpClient.GetFromJsonAsync<CountryResponse>($"{countriesBaseUrl}/1");

            country!.Name.Should().Be(_request.Name);
        }
    }
}

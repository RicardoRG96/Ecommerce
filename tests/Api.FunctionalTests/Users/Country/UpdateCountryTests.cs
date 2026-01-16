using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Countries.GetById;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
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
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{countriesBaseUrl}/0", _request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenCountryNameIsMissing()
        {
            SetAdminAuthentication();

            UpdateCountryRequest invalidRequest = _request with { Name = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{countriesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenCountryNameExceedsTheMaximumLength()
        {
            SetAdminAuthentication();

            UpdateCountryRequest invalidRequest =
                _request with { Name = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{countriesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenCountryIdDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{countriesBaseUrl}/{Constants.NotExistingId}", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenRequestIsValid_And_CountryIdExists()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{countriesBaseUrl}/1", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_UpdateName_WhenRequestIsValid_And_CountryIdExists()
        {
            SetAdminAuthentication();

            await HttpClient.PutAsJsonAsync($"{countriesBaseUrl}/1", _request);

            CountryResponse? country = await HttpClient.GetFromJsonAsync<CountryResponse>($"{countriesBaseUrl}/1");

            country!.Name.Should().Be(_request.Name);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{countriesBaseUrl}/1", _request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{countriesBaseUrl}/1", _request);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

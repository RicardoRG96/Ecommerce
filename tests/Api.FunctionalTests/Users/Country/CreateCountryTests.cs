using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
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
            SetAdminAuthentication();

            CreateCountryRequest invalidRequest = _request with { Name = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.Locations.Countries}", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenCountryNameExceedsTheMaximumLength()
        {
            SetAdminAuthentication();

            CreateCountryRequest invalidRequest =
                _request with { Name = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Locations.Countries, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnOk_WhenRequestIsValid()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Locations.Countries, _request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            long countryId = await response.Content.ReadFromJsonAsync<long>();

            countryId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Locations.Countries, _request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Locations.Countries, _request);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Municipality.Create;

namespace Api.FunctionalTests.Users.Municipality
{
    public class CreateMunicipalityTests : BaseFunctionalTest
    {
        private static readonly CreateMunicipalityRequest _request = new(1, "TestMuni");

        public CreateMunicipalityTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRegionIdIsMissing()
        {
            SetAdminAuthentication();

            CreateMunicipalityRequest invalidRequest = _request with { RegionId = 0 };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(municipalitiesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMunicipalityNameIsMissing()
        {
            SetAdminAuthentication();

            CreateMunicipalityRequest invalidRequest = _request with { Name = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(municipalitiesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenNameExceedsTheMaximumLength()
        {
            SetAdminAuthentication();

            CreateMunicipalityRequest invalidRequest = 
                _request with { Name = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(municipalitiesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenRegionIdDoesNotExist()
        {
            SetAdminAuthentication();

            CreateMunicipalityRequest invalidRequest = _request with { RegionId = Constants.NotExistingId };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(municipalitiesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_WhenRequestIsValid()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(municipalitiesBaseUrl, _request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            long municipalityId = await response.Content.ReadFromJsonAsync<long>();

            municipalityId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(municipalitiesBaseUrl, _request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(municipalitiesBaseUrl, _request);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

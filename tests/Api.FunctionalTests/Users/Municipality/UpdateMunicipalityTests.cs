using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Municipalities.GetById;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Municipality.Update;

namespace Api.FunctionalTests.Users.Municipality
{
    public class UpdateMunicipalityTests : BaseFunctionalTest
    {
        private static readonly UpdateMunicipalityRequest _request = new(1, "UpdatedName");

        public UpdateMunicipalityTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMunicipalityIdIsMissing()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Locations.Municipalities}/0", _request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRegionIdIsMissing()
        {
            SetAdminAuthentication();

            UpdateMunicipalityRequest invalidRequest = _request with { RegionId = 0 };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Locations.Municipalities}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRegionNameIsMissing()
        {
            SetAdminAuthentication();

            UpdateMunicipalityRequest invalidRequest = _request with { Name = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Locations.Municipalities}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRegionNameExceedsTheMaximumLength()
        {
            SetAdminAuthentication();

            UpdateMunicipalityRequest invalidRequest = 
                _request with { Name = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Locations.Municipalities}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenMunicipalityIdDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Locations.Municipalities}/{Constants.NotExistingId}", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenRegionIdDoesNotExist()
        {
            SetAdminAuthentication();

            UpdateMunicipalityRequest invalidRequest = _request with { RegionId = Constants.NotExistingId };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Locations.Municipalities}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenRequestIsValid_And_MunicipalityIdExists()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Locations.Municipalities}/1", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_UpdateName_WhenRequestIsValid_And_MunicipalityIdExists()
        {
            SetAdminAuthentication();

            await HttpClient.PutAsJsonAsync($"{ApiRoutes.Locations.Municipalities}/1", _request);

            MunicipalityResponse? municipality = 
                await HttpClient.GetFromJsonAsync<MunicipalityResponse>($"{ApiRoutes.Locations.Municipalities}/1");

            municipality!.Name.Should().Be(_request.Name);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Locations.Municipalities}/1", _request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Locations.Municipalities}/1", _request);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

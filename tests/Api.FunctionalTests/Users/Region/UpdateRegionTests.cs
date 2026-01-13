using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Regions.GetById;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Region.Update;

namespace Api.FunctionalTests.Users.Region
{
    public class UpdateRegionTests : BaseFunctionalTest
    {
        private static readonly UpdateRegionRequest _request = new("UpdatedName");

        public UpdateRegionTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRegionIdIsMissing()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{regionsBaseUrl}/0", _request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRegionNameIsMissing()
        {
            SetAdminAuthentication();

            UpdateRegionRequest invalidRequest = _request with { Name = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{regionsBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRegionNameExceedsTheMaximumLength()
        {
            SetAdminAuthentication();

            UpdateRegionRequest invalidRequest =
                _request with { Name = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{regionsBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenRegionIdDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{regionsBaseUrl}/{Constants.NotExistingId}", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]  
        public async Task Should_ReturnNoContent_WhenRequestIsValid_And_RegionIdExists()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{regionsBaseUrl}/1", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_UpdateName_WhenRequestIsValid_And_RegionIdExists()
        {
            SetAdminAuthentication();

            await HttpClient.PutAsJsonAsync($"{regionsBaseUrl}/1", _request);

            RegionResponse? region = await HttpClient.GetFromJsonAsync<RegionResponse>($"{regionsBaseUrl}/1");

            region!.Name.Should().Be(_request.Name);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{regionsBaseUrl}/1", _request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{regionsBaseUrl}/1", _request);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

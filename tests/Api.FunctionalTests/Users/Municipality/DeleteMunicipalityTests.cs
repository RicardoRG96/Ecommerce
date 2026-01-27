using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;

namespace Api.FunctionalTests.Users.Municipality
{
    public class DeleteMunicipalityTests : BaseFunctionalTest
    {
        public DeleteMunicipalityTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMunicipalityIdIsMissing()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.Locations.Municipalities}/0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenMunicipalityIdDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.Locations.Municipalities}/{Constants.NotExistingId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenMunicipalityIdExists()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.Locations.Municipalities}/21");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.Locations.Municipalities}/21");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.Locations.Municipalities}/21");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

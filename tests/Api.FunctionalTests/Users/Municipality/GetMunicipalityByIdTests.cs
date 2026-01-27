using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Municipalities.GetById;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Municipality
{
    public class GetMunicipalityByIdTests : BaseFunctionalTest
    {
        public GetMunicipalityByIdTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenMunicipalityIdDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Municipalities}/{Constants.NotExistingId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_And_Municipality_WhenMunicipalityIdExists()
        {
            SetAdminAuthentication();

            MunicipalityResponse? municipality = 
                await HttpClient.GetFromJsonAsync<MunicipalityResponse>($"{ApiRoutes.Locations.Municipalities}/1");

            municipality.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Municipalities}/1");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Municipalities}/1");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

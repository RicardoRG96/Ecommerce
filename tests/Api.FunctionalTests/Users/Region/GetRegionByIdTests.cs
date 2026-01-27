using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Regions.GetById;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Region
{
    public class GetRegionByIdTests : BaseFunctionalTest
    {
        public GetRegionByIdTests(FunctionalTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenRegionIdDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Regions}/{Constants.NotExistingId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_And_Region_WhenRegionIdExists()
        {
            SetAdminAuthentication();

            RegionResponse? region = await HttpClient.GetFromJsonAsync<RegionResponse>($"{ApiRoutes.Locations.Regions}/1");

            region.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Regions}/1");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Regions}/1");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

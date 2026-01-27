using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Countries.GetById;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Country
{
    public class GetCountryByIdTests : BaseFunctionalTest
    {
        public GetCountryByIdTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenCountryIdDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Countries}/{Constants.NotExistingId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_AndCountry_WhenCountryExists()
        {
            SetAdminAuthentication();

            CountryResponse? country = await HttpClient.GetFromJsonAsync<CountryResponse>($"{ApiRoutes.Locations.Countries}/1");

            country.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Countries}/1");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Countries}/1");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Region.Create;

namespace Api.FunctionalTests.Users.Region
{
    public class CreateRegionTests : BaseFunctionalTest
    {
        private static readonly CreateRegionRequest _request = new("TestRegion");

        public CreateRegionTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRegionNameIsMissing()
        {
            SetAdminAuthentication();

            CreateRegionRequest invalidRequest = _request with { Name = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(regionsBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRegionNameExceedsTheMaximumLength()
        {
            SetAdminAuthentication();

            CreateRegionRequest invalidRequest =
                _request with { Name = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(regionsBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnOk_WhenRequestIsValid()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(regionsBaseUrl, _request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            long regionId = await response.Content.ReadFromJsonAsync<long>();

            regionId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(regionsBaseUrl, _request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(regionsBaseUrl, _request);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

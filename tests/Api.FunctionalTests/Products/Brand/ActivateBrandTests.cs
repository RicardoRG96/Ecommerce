using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;

namespace Api.FunctionalTests.Products.Brand
{
    public class ActivateBrandTests : BaseFunctionalTest
    {
        public ActivateBrandTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenBrandIdIsMissing()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PatchAsync($"{brandsBaseUrl}/0/activate", null!);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenBrandIdDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PatchAsync($"{brandsBaseUrl}/{Constants.NotExistingId}/activate", null!);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenBrandIdExists()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PatchAsync($"{brandsBaseUrl}/1/activate", null!);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenBrandIsAlreadyActive()
        {
            SetAdminAuthentication();

            // first activation
            await HttpClient.PatchAsync($"{brandsBaseUrl}/1/activate", null!);

            // second activation (the brand is already active)
            HttpResponseMessage response = await HttpClient.PatchAsync($"{brandsBaseUrl}/1/activate", null!);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.PatchAsync($"{brandsBaseUrl}/1/activate", null!);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.PatchAsync($"{brandsBaseUrl}/1/activate", null!);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

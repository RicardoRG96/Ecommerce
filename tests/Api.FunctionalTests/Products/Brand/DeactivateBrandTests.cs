using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;

namespace Api.FunctionalTests.Products.Brand
{
    public class DeactivateBrandTests : BaseFunctionalTest
    {
        public DeactivateBrandTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenBrandIdIsMissing()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Brands.Base}/0/deactivate", null!);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenBrandIdDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Brands.Base}/{Constants.NotExistingId}/deactivate", null!);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenBrandIdExists()
        {
            SetAdminAuthentication();

            // Assume that there is an active brand with ID 1 in the seed data
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Brands.Base}/1/deactivate", null!);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenBrandIsAlreadyInactive()
        {
            SetAdminAuthentication();

            // First deactivation
            await HttpClient.PatchAsync($"{ApiRoutes.Brands.Base}/1/deactivate", null!);

            // Second deactivation (the brand is already inactive)
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Brands.Base}/1/deactivate", null!);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Brands.Base}/1/deactivate", null!);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Brands.Base}/1/deactivate", null!);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

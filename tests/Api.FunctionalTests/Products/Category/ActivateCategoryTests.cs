using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;

namespace Api.FunctionalTests.Products.Category
{
    public class ActivateCategoryTests : BaseFunctionalTest
    {
        public ActivateCategoryTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenCategoryIdIsMissing()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Categories.Base}/0/activate", null!);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenCategoryIdDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Categories.Base}/{Constants.NotExistingId}/activate", null!);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenCategoryIdExists()
        {
            SetAdminAuthentication();

            // Assume there is a category with ID 1 in the seed data
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Categories.Base}/1/activate", null!);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenCategoryIsAlreadyActive()
        {
            SetAdminAuthentication();

            // first activation
            await HttpClient.PatchAsync($"{ApiRoutes.Categories.Base}/1/activate", null!);

            // Second activation (the category is already active)
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Categories.Base}/1/activate", null!);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Categories.Base}/1/activate", null!);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Categories.Base}/1/activate", null!);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

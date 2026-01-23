using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;

namespace Api.FunctionalTests.Products.Category
{
    public class DeactivateCategoryTests : BaseFunctionalTest
    {
        public DeactivateCategoryTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenCategoryIdIsMissing()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{categoriesBaseUrl}/0/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenCategoryIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{categoriesBaseUrl}/{Constants.NotExistingId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenCategoryIdExists()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Assumes that a category with ID 1 exists in seed data
            HttpResponseMessage response = await HttpClient.PatchAsync($"{categoriesBaseUrl}/1/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenCategoryIsAlreadyInactive()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - First deactivation
            await HttpClient.PatchAsync($"{categoriesBaseUrl}/1/deactivate", null!);

            // Act - Second deactivation (category is already inactive)
            HttpResponseMessage response = await HttpClient.PatchAsync($"{categoriesBaseUrl}/1/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{categoriesBaseUrl}/1/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{categoriesBaseUrl}/1/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

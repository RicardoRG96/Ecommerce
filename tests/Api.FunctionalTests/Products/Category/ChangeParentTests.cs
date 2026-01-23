using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.Category.ChangeParent;

namespace Api.FunctionalTests.Products.Category
{
    public class ChangeParentTests : BaseFunctionalTest
    {
        private static readonly ChangeParentRequest _request = new(NewParentId: 2);

        public ChangeParentTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenCategoryIdIsMissing()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsJsonAsync($"{categoriesBaseUrl}/0/parent", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenCategoryIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsJsonAsync($"{categoriesBaseUrl}/{Constants.NotExistingId}/parent", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenCategoryExistsAndRequestIsValid()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Change parent of category 7 (Televisions) to category 2 (Computers)
            HttpResponseMessage response = await HttpClient.PatchAsJsonAsync($"{categoriesBaseUrl}/7/parent", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_AllowChangingToRootCategory_WhenNewParentIdIsNull()
        {
            // Arrange
            SetAdminAuthentication();
            ChangeParentRequest rootRequest = new(NewParentId: null);

            // Act - Change category 10 (DSLR Cameras) to root category (no parent)
            HttpResponseMessage response = await HttpClient.PatchAsJsonAsync($"{categoriesBaseUrl}/10/parent", rootRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_AllowChangingParentMultipleTimes()
        {
            // Arrange
            SetAdminAuthentication();
            ChangeParentRequest firstChange = new(NewParentId: 1);
            ChangeParentRequest secondChange = new(NewParentId: 3);

            // Act - First change: move category 12 (Laptops) to Electronics (1)
            HttpResponseMessage firstResponse = await HttpClient.PatchAsJsonAsync($"{categoriesBaseUrl}/12/parent", firstChange);

            // Act - Second change: move category 12 (Laptops) to Smartphones (3)
            HttpResponseMessage secondResponse = await HttpClient.PatchAsJsonAsync($"{categoriesBaseUrl}/12/parent", secondChange);

            // Assert
            firstResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            secondResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsJsonAsync($"{categoriesBaseUrl}/7/parent", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsJsonAsync($"{categoriesBaseUrl}/7/parent", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

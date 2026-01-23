using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.Category.Update;

namespace Api.FunctionalTests.Products.Category
{
    public class UpdateCategoryTests : BaseFunctionalTest
    {
        private static readonly UpdateCategoryRequest _request = new(
            Name: "Home Electronics Updated",
            Description: "Updated description for home electronic devices and smart home systems",
            ImageUrl: "https://example.com/images/electronics-updated.png",
            Icon: "fa-bolt-lightning",
            DisplayOrder: 5,
            IsVisibleInMenu: true,
            MetaTitle: "Home Electronics - Smart Devices Updated",
            MetaDescription: "Shop for updated home electronics and smart home devices",
            MetaKeywords: "electronics, smart home, devices, updated");

        public UpdateCategoryTests(FunctionalTestWebAppFactory factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenCategoryIdIsMissing()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{categoriesBaseUrl}/0", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenNameIsMissing()
        {
            // Arrange
            SetAdminAuthentication();
            UpdateCategoryRequest invalidRequest = _request with { Name = "" };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{categoriesBaseUrl}/1", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenNameExceedsMaximumLength()
        {
            // Arrange
            SetAdminAuthentication();
            UpdateCategoryRequest invalidRequest = _request with
            {
                Name = Constants.ExceededMaximumLengthField
            };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{categoriesBaseUrl}/1", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenDescriptionExceedsMaximumLength()
        {
            // Arrange
            SetAdminAuthentication();
            UpdateCategoryRequest invalidRequest = _request with
            {
                Description = Constants.ExceededMaximumLengthField
            };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{categoriesBaseUrl}/1", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenImageUrlExceedsMaximumLength()
        {
            // Arrange
            SetAdminAuthentication();
            UpdateCategoryRequest invalidRequest = _request with
            {
                ImageUrl = Constants.ExceededMaximumLengthField
            };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{categoriesBaseUrl}/1", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenIconExceedsMaximumLength()
        {
            // Arrange
            SetAdminAuthentication();
            UpdateCategoryRequest invalidRequest = _request with
            {
                Icon = Constants.ExceededMaximumLengthField
            };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{categoriesBaseUrl}/1", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenDisplayOrderIsMissing()
        {
            // Arrange
            SetAdminAuthentication();
            UpdateCategoryRequest invalidRequest = _request with { DisplayOrder = 0 };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{categoriesBaseUrl}/1", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMetaTitleExceedsMaximumLength()
        {
            // Arrange
            SetAdminAuthentication();
            UpdateCategoryRequest invalidRequest = _request with
            {
                MetaTitle = Constants.ExceededMaximumLengthField
            };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{categoriesBaseUrl}/1", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMetaDescriptionExceedsMaximumLength()
        {
            // Arrange
            SetAdminAuthentication();
            UpdateCategoryRequest invalidRequest = _request with
            {
                MetaDescription = Constants.ExceededMaximumLengthField
            };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{categoriesBaseUrl}/1", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMetaKeywordsExceedsMaximumLength()
        {
            // Arrange
            SetAdminAuthentication();
            UpdateCategoryRequest invalidRequest = _request with
            {
                MetaKeywords = Constants.ExceededMaximumLengthField
            };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{categoriesBaseUrl}/1", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenCategoryIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{categoriesBaseUrl}/{Constants.NotExistingId}", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenCategoryNameAlreadyExists()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Trying to change category 2 name to "Electronics" which already exists with ID 1
            UpdateCategoryRequest invalidRequest = _request with { Name = "Electronics" };
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{categoriesBaseUrl}/2", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenRequestIsValid_And_CategoryIdExists()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{categoriesBaseUrl}/2", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_AllowKeepingSameName_WhenUpdatingSameCategory()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Update category 1 (Electronics) keeping the same name
            UpdateCategoryRequest sameNameRequest = _request with { Name = "Electronics" };
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{categoriesBaseUrl}/1", sameNameRequest);

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
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{categoriesBaseUrl}/1", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{categoriesBaseUrl}/1", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
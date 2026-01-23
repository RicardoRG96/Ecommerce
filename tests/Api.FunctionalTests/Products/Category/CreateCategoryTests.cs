using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.Category.Create;

namespace Api.FunctionalTests.Products.Category
{
    public class CreateCategoryTests : BaseFunctionalTest
    {
        private static readonly CreateCategoryRequest _request = new(
            ParentId: 1,
            Name: "Gaming Consoles",
            Description: "PlayStation, Xbox, Nintendo and gaming accessories",
            ImageUrl: "https://example.com/images/gaming.png",
            Icon: "fa-gamepad",
            DisplayOrder: 4,
            IsActive: true,
            IsVisibleInMenu: true,
            MetaTitle: "Gaming Consoles - PlayStation & Xbox",
            MetaDescription: "Shop for gaming consoles, games and accessories",
            MetaKeywords: "gaming, consoles, PlayStation, Xbox, Nintendo");

        public CreateCategoryTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenNameIsMissing()
        {
            // Arrange
            SetAdminAuthentication();
            CreateCategoryRequest invalidRequest = _request with { Name = "" };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(categoriesBaseUrl, invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenNameExceedsMaximumLength()
        {
            // Arrange
            SetAdminAuthentication();
            CreateCategoryRequest invalidRequest = _request with 
            { 
                Name = Constants.ExceededMaximumLengthField 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(categoriesBaseUrl, invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenDescriptionExceedsMaximumLength()
        {
            // Arrange
            SetAdminAuthentication();
            CreateCategoryRequest invalidRequest = _request with 
            { 
                Description = Constants.ExceededMaximumLengthField 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(categoriesBaseUrl, invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenImageUrlExceedsMaximumLength()
        {
            // Arrange
            SetAdminAuthentication();
            CreateCategoryRequest invalidRequest = _request with 
            { 
                ImageUrl = Constants.ExceededMaximumLengthField 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(categoriesBaseUrl, invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenIconExceedsMaximumLength()
        {
            // Arrange
            SetAdminAuthentication();
            CreateCategoryRequest invalidRequest = _request with 
            { 
                Icon = Constants.ExceededMaximumLengthField 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(categoriesBaseUrl, invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenDisplayOrderIsMissing()
        {
            // Arrange
            SetAdminAuthentication();
            CreateCategoryRequest invalidRequest = _request with { DisplayOrder = 0 };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(categoriesBaseUrl, invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMetaTitleExceedsMaximumLength()
        {
            // Arrange
            SetAdminAuthentication();
            CreateCategoryRequest invalidRequest = _request with 
            { 
                MetaTitle = Constants.ExceededMaximumLengthField 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(categoriesBaseUrl, invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMetaDescriptionExceedsMaximumLength()
        {
            // Arrange
            SetAdminAuthentication();
            CreateCategoryRequest invalidRequest = _request with 
            { 
                MetaDescription = Constants.ExceededMaximumLengthField 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(categoriesBaseUrl, invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMetaKeywordsExceedsMaximumLength()
        {
            // Arrange
            SetAdminAuthentication();
            CreateCategoryRequest invalidRequest = _request with 
            { 
                MetaKeywords = Constants.ExceededMaximumLengthField 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(categoriesBaseUrl, invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenCategoryNameAlreadyExists()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Electronics already exists in seed data
            CreateCategoryRequest invalidRequest = _request with { Name = "Electronics" };
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(categoriesBaseUrl, invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnOk_WhenRequestIsValid()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(categoriesBaseUrl, _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            long categoryId = await response.Content.ReadFromJsonAsync<long>();
            categoryId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Should_CreateRootCategory_WhenParentIdIsZero()
        {
            // Arrange
            SetAdminAuthentication();
            CreateCategoryRequest rootRequest = _request with 
            { 
                ParentId = null,
                Name = "Books & Media"
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(categoriesBaseUrl, rootRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            long categoryId = await response.Content.ReadFromJsonAsync<long>();
            categoryId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Should_CreateSubcategory_WhenParentIdExists()
        {
            // Arrange
            SetAdminAuthentication();
            CreateCategoryRequest subcategoryRequest = _request with 
            { 
                ParentId = 1,
                Name = "Smart Home Devices"
            };

            // Act - Create as subcategory of Electronics (ID 1)
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(categoriesBaseUrl, subcategoryRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            long categoryId = await response.Content.ReadFromJsonAsync<long>();
            categoryId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(categoriesBaseUrl, _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(categoriesBaseUrl, _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

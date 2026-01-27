using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.Product.Create;
using Web.Api.Endpoints.v1.Products.Product.Update;

namespace Api.FunctionalTests.Products.Product
{
    public sealed class UpdateProductTests : BaseFunctionalTest
    {
        private static readonly UpdateProductRequest _request = new(
            Name: "PlayStation 5 Console - Updated Edition",
            Description: "The PlayStation 5 console unleashes new gaming possibilities. Updated with enhanced features, lightning-fast loading with an ultra-high speed SSD, deeper immersion with support for haptic feedback, adaptive triggers and 3D Audio.",
            ShortDescription: "Next-gen gaming console with ultra-high speed SSD - Updated",
            BrandId: 1,
            CategoryId: 1,
            ProductTaxCategoryId: 1,
            IsFeatured: true,
            IsDigital: false,
            MetaTitle: "PlayStation 5 Console - Updated Edition",
            MetaDescription: "Buy PlayStation 5 console with updated features and stunning 4K graphics",
            MetaKeywords: "PS5, PlayStation 5, gaming console, next-gen, updated");

        public UpdateProductTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        // Consolida tests similares usando Theory
        [Theory]
        [MemberData(nameof(GetInvalidMaxLengthRequests))]
        public async Task Should_ReturnBadRequest_WhenFieldExceedsMaximumLength(
            UpdateProductRequest invalidRequest, 
            string fieldName)
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProduct();

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Products.Base}/{productId}", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, 
                because: $"{fieldName} exceeds maximum length");
        }

        public static IEnumerable<object[]> GetInvalidMaxLengthRequests()
        {
            yield return new object[] 
            { 
                _request with { Name = Constants.ExceededMaximumLengthField }, 
                nameof(UpdateProductRequest.Name) 
            };
            yield return new object[] 
            { 
                _request with { ShortDescription = Constants.ExceededMaximumLengthField }, 
                nameof(UpdateProductRequest.ShortDescription) 
            };
            yield return new object[] 
            { 
                _request with { MetaTitle = Constants.ExceededMaximumLengthField }, 
                nameof(UpdateProductRequest.MetaTitle) 
            };
            yield return new object[] 
            { 
                _request with { MetaDescription = Constants.ExceededMaximumLengthField }, 
                nameof(UpdateProductRequest.MetaDescription) 
            };
            yield return new object[] 
            { 
                _request with { MetaKeywords = Constants.ExceededMaximumLengthField }, 
                nameof(UpdateProductRequest.MetaKeywords) 
            };
        }

        // Consolida validaciones de IDs
        [Theory]
        [InlineData(0, "BrandId", "is missing")]
        [InlineData(-1, "BrandId", "is negative")]
        public async Task Should_ReturnBadRequest_WhenBrandIdIsInvalid(
            long invalidBrandId, 
            string fieldName, 
            string reason)
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProduct();
            UpdateProductRequest invalidRequest = _request with { BrandId = invalidBrandId };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Products.Base}/{productId}", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, 
                because: $"{fieldName} {reason}");
        }

        [Theory]
        [InlineData(0, "CategoryId", "is missing")]
        [InlineData(-1, "CategoryId", "is negative")]
        public async Task Should_ReturnBadRequest_WhenCategoryIdIsInvalid(
            long invalidCategoryId, 
            string fieldName,
            string reason)
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProduct();
            UpdateProductRequest invalidRequest = _request with { CategoryId = invalidCategoryId };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Products.Base}/{productId}", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                because: $"{fieldName} {reason}");
        }

        [Theory]
        [InlineData(0, "ProductTaxCategoryId", "is missing")]
        [InlineData(-1, "ProductTaxCategoryId", "is negative")]
        public async Task Should_ReturnBadRequest_WhenProductTaxCategoryIdIsInvalid(
            long invalidId, 
            string fieldName,
            string reason)
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProduct();
            UpdateProductRequest invalidRequest = _request with { ProductTaxCategoryId = invalidId };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Products.Base}/{productId}", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                because: $"{fieldName} {reason}");
        }

        [Theory]
        [InlineData(0, "ProductId", "is missing")]
        [InlineData(-1, "ProductId", "is negative")]
        public async Task Should_ReturnBadRequest_WhenProductIdIsInvalid(
            long invalidProductId,
            string fieldName,
            string reason)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Products.Base}/{invalidProductId}", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                because: $"{fieldName} {reason}");
        }

        // Verifica mensajes de error específicos
        [Fact]
        public async Task Should_ReturnBadRequest_WhenNameIsMissing()
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProduct();
            UpdateProductRequest invalidRequest = _request with { Name = "" };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Products.Base}/{productId}", invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("Product name is required", 
                because: "validation message should be clear for debugging");
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenProductIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Products.Base}/{Constants.NotExistingId}", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        // Verifica entidades relacionadas inexistentes con Theory
        [Theory]
        [MemberData(nameof(GetNonExistentRelatedEntityRequests))]
        public async Task Should_ReturnNotFound_WhenRelatedEntityDoesNotExist(
            UpdateProductRequest invalidRequest,
            string entityName)
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProduct();

            UpdateProductRequest uniqueRequest = invalidRequest with
            {
                Name = $"Unique Product {Guid.NewGuid()}"
            };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Products.Base}/{productId}", uniqueRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().Contain(entityName, 
                because: $"error should indicate which {entityName} was not found");
        }

        public static IEnumerable<object[]> GetNonExistentRelatedEntityRequests()
        {
            yield return new object[] { _request with { BrandId = 999999 }, "Brand" };
            yield return new object[] { _request with { CategoryId = 999999 }, "Category" };
            yield return new object[] { _request with { ProductTaxCategoryId = 999999 }, "ProductTaxCategory" };
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenProductNameAlreadyExists()
        {
            // Arrange
            SetAdminAuthentication();
            string uniqueName1 = $"Test Product 1 {Guid.NewGuid()}";
            string uniqueName2 = $"Test Product 2 {Guid.NewGuid()}";
            
            long productId1 = await CreateTestProduct(uniqueName1);
            long productId2 = await CreateTestProduct(uniqueName2);

            // Act - Try to change product2 name to product1's name
            UpdateProductRequest invalidRequest = _request with { Name = uniqueName1 };
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Products.Base}/{productId2}", invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("already exists", 
                because: "duplicate name should be clearly indicated");
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenRequestIsValid()
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProduct();

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Products.Base}/{productId}", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_AllowKeepingSameName_WhenUpdatingSameProduct()
        {
            // Arrange
            SetAdminAuthentication();
            string productName = $"Unique Product {Guid.NewGuid()}";
            long productId = await CreateTestProduct(productName);

            // Act - Update product keeping the same name
            UpdateProductRequest sameNameRequest = _request with { Name = productName };
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Products.Base}/{productId}", sameNameRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        // Consolida casos exitosos con diferentes configuraciones
        [Theory]
        [MemberData(nameof(GetValidProductUpdateConfigurations))]
        public async Task Should_ReturnNoContent_WhenRequestIsValidWithDifferentConfigurations(
            UpdateProductRequest validRequest,
            string scenario)
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProduct();

            UpdateProductRequest uniqueRequest = validRequest with
            {
                Name = $"Unique Product {Guid.NewGuid()}"
            };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Products.Base}/{productId}", uniqueRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent, because: scenario);
        }

        public static IEnumerable<object[]> GetValidProductUpdateConfigurations()
        {
            yield return new object[]
            {
                new UpdateProductRequest(
                    Name: "Updated Product",
                    Description: "",
                    ShortDescription: "",
                    BrandId: 1,
                    CategoryId: 1,
                    ProductTaxCategoryId: 1,
                    IsFeatured: false,
                    IsDigital: false,
                    MetaTitle: "",
                    MetaDescription: "",
                    MetaKeywords: ""),
                "minimal fields should be accepted"
            };

            yield return new object[]
            {
                _request with 
                { 
                    IsDigital = true 
                },
                "digital products should be updated successfully"
            };

            yield return new object[]
            {
                _request with 
                { 
                    IsFeatured = false 
                },
                "non-featured products should be updated successfully"
            };

            yield return new object[]
            {
                _request with 
                { 
                    BrandId = 2 
                },
                "changing brand should be successful"
            };

            yield return new object[]
            {
                _request with 
                { 
                    CategoryId = 2 
                },
                "changing category should be successful"
            };
        }

        // Usa Theory para tests de autorización también
        [Theory]
        [InlineData(HttpStatusCode.Unauthorized, "no authentication token")]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotAuthenticated(
            HttpStatusCode expectedStatusCode,
            string reason)
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = null;

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Products.Base}/1", _request);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserIsNotAdmin()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Products.Base}/1", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        // Helper privado para crear productos de prueba
        private async Task<long> CreateTestProduct(string? name = null)
        {
            CreateProductRequest createRequest = new(
                Name: name ?? $"Test Product {Guid.NewGuid()}",
                Description: "Test product description",
                ShortDescription: "Test short description",
                BrandId: 1,
                CategoryId: 1,
                ProductTaxCategoryId: 1,
                IsActive: true,
                IsFeatured: false,
                IsDigital: false,
                MetaTitle: "Test Meta Title",
                MetaDescription: "Test Meta Description",
                MetaKeywords: "test, product");

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Products.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long? productId = await response.Content.ReadFromJsonAsync<long?>();
            return productId!.Value;
        }
    }
}

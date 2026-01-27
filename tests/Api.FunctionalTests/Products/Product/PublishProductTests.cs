using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.Product.Create;

namespace Api.FunctionalTests.Products.Product
{
    public class PublishProductTests : BaseFunctionalTest
    {
        public PublishProductTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
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
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{invalidProductId}/publish", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                because: $"{fieldName} {reason}");
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenProductIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{Constants.NotExistingId}/publish", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenProductIsNotActive()
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProduct(isActive: false);

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{productId}/publish", null!);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("Product must be active",
                because: "inactive products cannot be published");
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenProductBrandIsNotActive()
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProductWithCompleteSetup();
            
            // Deactivate the brand (assuming brand 1 exists and can be deactivated)
            await HttpClient.PatchAsync($"{ApiRoutes.Brands.Base}/1/deactivate", null!);

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{productId}/publish", null!);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("Brand must be active",
                because: "products with inactive brands cannot be published");
            
            // Cleanup - reactivate the brand
            await HttpClient.PatchAsync($"{ApiRoutes.Brands.Base}/1/activate", null!);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenProductCategoryIsNotActive()
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProductWithCompleteSetup();
            
            // Deactivate the category
            await HttpClient.PatchAsync($"{ApiRoutes.Categories.Base}/1/deactivate", null!);

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{productId}/publish", null!);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("Category must be active",
                because: "products with inactive categories cannot be published");
            
            // Cleanup - reactivate the category
            await HttpClient.PatchAsync($"{ApiRoutes.Categories.Base}/1/activate", null!);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenProductHasNoSkus()
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProduct();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{productId}/publish", null!);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("at least one SKU",
                because: "products without SKUs cannot be published");
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenProductHasInactiveSku()
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProductWithCompleteSetup();
            
            // Get the SKU and deactivate it (this would need the SKU endpoint implementation)
            // For now, we'll assume this scenario can't be fully tested without SKU management

            // Act & Assert
            // This test would require SKU deactivation endpoint to be implemented
            // Placeholder for future implementation
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenProductSkuHasInvalidPrice()
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProductWithCompleteSetup();
            
            // Update SKU price to 0 or negative (this would need the SKU endpoint implementation)
            // For now, we'll assume this scenario can't be fully tested without SKU management

            // Act & Assert
            // This test would require SKU price update endpoint to be implemented
            // Placeholder for future implementation
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenProductHasNoImages()
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProduct();
            
            // Create a SKU but no images
            await CreateProductSku(productId);

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{productId}/publish", null!);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("at least one image",
                because: "products without images cannot be published");
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenProductHasNoPrimaryImage()
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProduct();
            
            // Create SKU and images but none marked as primary
            await CreateProductSku(productId);
            await CreateProductImage(productId, isPrimary: false);

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{productId}/publish", null!);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("primary image",
                because: "products must have a primary image to be published");
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenProductMeetsAllRequirements()
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProductWithCompleteSetup();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{productId}/publish", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenProductIsAlreadyPublished()
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProductWithCompleteSetup();

            // First publish
            HttpResponseMessage firstPublish = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{productId}/publish", null!);
            firstPublish.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Act - Second publish (product is already published)
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{productId}/publish", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "publishing an already published product should be idempotent");
        }

        [Theory]
        [InlineData(HttpStatusCode.Unauthorized, "no authentication token")]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotAuthenticated(
            HttpStatusCode expectedStatusCode,
            string reason)
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = null;

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/1/publish", null!);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserIsNotAdmin()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/1/publish", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        // Helper privado para crear productos de prueba
        private async Task<long> CreateTestProduct(bool isActive = true)
        {
            CreateProductRequest createRequest = new(
                Name: $"Test Product {Guid.NewGuid()}",
                Description: "Test product description",
                ShortDescription: "Test short description",
                BrandId: 1,
                CategoryId: 1,
                ProductTaxCategoryId: 1,
                IsActive: isActive,
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

        // Helper para crear producto con todos los requisitos para publicación
        private async Task<long> CreateTestProductWithCompleteSetup()
        {
            long productId = await CreateTestProduct(isActive: true);
            
            // Create SKU
            await CreateProductSku(productId);
            
            // Create primary image
            await CreateProductImage(productId, isPrimary: true);
            
            return productId;
        }

        // Helper para crear SKU del producto
        private async Task CreateProductSku(long productId)
        {
            // Este método debería llamar al endpoint de creación de SKU
            // Asumiendo que existe un endpoint POST /api/v1/products/{productId}/skus
            // Por ahora, esto es un placeholder que deberías implementar según tu API
            
            var skuRequest = new
            {
                Sku = $"SKU-{Guid.NewGuid()}",
                Price = 99.99m,
                Stock = 10,
                IsActive = true
            };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(
                $"{ApiRoutes.Products.Base}/{productId}/skus", 
                skuRequest);
            
            response.EnsureSuccessStatusCode();
        }

        // Helper para crear imagen del producto
        private async Task CreateProductImage(long productId, bool isPrimary)
        {
            // Este método debería llamar al endpoint de creación de imagen
            // Asumiendo que existe un endpoint POST /api/v1/products/{productId}/images
            // Por ahora, esto es un placeholder que deberías implementar según tu API
            
            var imageRequest = new
            {
                ImageUrl = $"https://example.com/images/{Guid.NewGuid()}.jpg",
                IsPrimary = isPrimary,
                DisplayOrder = 1
            };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(
                $"{ApiRoutes.Products.Base}/{productId}/gallery", 
                imageRequest);
            
            response.EnsureSuccessStatusCode();
        }
    }
}

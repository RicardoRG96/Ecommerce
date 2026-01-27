using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.Product.Create;

namespace Api.FunctionalTests.Products.Product
{
    public class UnpublishProductTests : BaseFunctionalTest
    {
        public UnpublishProductTests(FunctionalTestWebAppFactory factory) 
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
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{invalidProductId}/unpublish", null!);

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
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{Constants.NotExistingId}/unpublish", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenProductIsPublished()
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProductWithCompleteSetup();
            
            // Publish the product first
            await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{productId}/publish", null!);

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{productId}/unpublish", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenProductIsAlreadyUnpublished()
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProduct();

            // Act - Unpublish a product that is not published
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{productId}/unpublish", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "unpublishing an already unpublished product should be idempotent");
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenUnpublishingTwice()
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProductWithCompleteSetup();
            
            // Publish the product
            await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{productId}/publish", null!);
            
            // First unpublish
            HttpResponseMessage firstUnpublish = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{productId}/unpublish", null!);
            firstUnpublish.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Act - Second unpublish (product is already unpublished)
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{productId}/unpublish", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "unpublishing an already unpublished product should be idempotent");
        }

        [Fact]
        public async Task Should_AllowRepublishingAfterUnpublish()
        {
            // Arrange
            SetAdminAuthentication();
            long productId = await CreateTestProductWithCompleteSetup();

            // Publish
            HttpResponseMessage publishResponse = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{productId}/publish", null!);
            publishResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Unpublish
            HttpResponseMessage unpublishResponse = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{productId}/unpublish", null!);
            unpublishResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Act - Republish
            HttpResponseMessage republishResponse = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{productId}/publish", null!);

            // Assert
            republishResponse.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "products should be able to be republished after unpublishing");
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
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/1/unpublish", null!);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserIsNotAdmin()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/1/unpublish", null!);

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

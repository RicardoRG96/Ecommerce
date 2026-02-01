using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.ProductSku.Create;

namespace Api.FunctionalTests.Products.ProductSku
{
    public class DeactivateProductSkuTests : BaseFunctionalTest
    {
        #region Seed Data Constants - Published Products with SKUs

        /// <summary>
        /// Product ID 1: Smartphone Galaxy X - Published with 2 SKUs
        /// </summary>
        private const long SMARTPHONE_GALAXY_X_ID = 1;

        /// <summary>
        /// Product ID 2: iPhone Pro Max - Published with 2 SKUs
        /// </summary>
        private const long IPHONE_PRO_MAX_ID = 2;

        /// <summary>
        /// Product ID 3: Laptop Ultrabook Pro - Published with 1 SKU
        /// </summary>
        private const long LAPTOP_ULTRABOOK_PRO_ID = 3;

        #endregion

        public DeactivateProductSkuTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Theory]
        [InlineData(0, "ProductSkuId", "is missing")]
        [InlineData(-1, "ProductSkuId", "is negative")]
        public async Task Should_ReturnBadRequest_WhenProductSkuIdIsInvalid(
            long invalidProductSkuId,
            string fieldName,
            string reason)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/{invalidProductSkuId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                because: $"{fieldName} {reason}");
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenProductSkuIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/{Constants.NotExistingId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenProductSkuIdExists()
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateActiveProductSku();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenProductSkuIsAlreadyInactive()
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateActiveProductSku();

            // Act - First deactivation
            await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}/deactivate", null!);

            // Act - Second deactivation (already inactive)
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "deactivating an already inactive product SKU should be idempotent");
        }

        [Fact]
        public async Task Should_DeactivateActiveProductSku_Successfully()
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateActiveProductSku();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_DeactivateInactiveProductSku_Successfully()
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateInactiveProductSku();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "deactivating an inactive product SKU should be idempotent");
        }

        [Theory]
        [InlineData(HttpStatusCode.Unauthorized, "no authentication token")]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotAuthenticated(
            HttpStatusCode expectedStatusCode,
            string reason)
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/1/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserIsNotAdmin()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/1/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Theory]
        [InlineData(SMARTPHONE_GALAXY_X_ID, "Smartphone Galaxy X")]
        [InlineData(IPHONE_PRO_MAX_ID, "iPhone Pro Max")]
        [InlineData(LAPTOP_ULTRABOOK_PRO_ID, "Laptop Ultrabook Pro")]
        public async Task Should_DeactivateProductSku_ForDifferentSeededProducts(
            long productId,
            string productName)
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateActiveProductSku(productId);

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent, 
                because: $"deactivating SKU for {productName}");
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenDeactivatingProductSkuWithPhysicalDimensions()
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateActiveProductSkuWithDimensions();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenDeactivatingProductSkuWithNullOptionalFields()
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateActiveProductSkuWithMinimalFields();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenDeactivatingProductSkuWithHighPrice()
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateActiveProductSkuWithHighPrice();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        #region Helper Methods

        private async Task<long> CreateActiveProductSku(long? productId = null)
        {
            CreateProductSkuRequest createRequest = new(
                ProductId: productId ?? SMARTPHONE_GALAXY_X_ID,
                BarCode: $"780{Guid.NewGuid().ToString()[..10]}",
                Price: 799990m,
                Cost: 550000m,
                Weight: 0.180m,
                Length: 15.8m,
                Width: 7.4m,
                Height: 0.8m,
                IsActive: true,
                DisplayOrder: 1);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long productSkuId = await response.Content.ReadFromJsonAsync<long>();
            return productSkuId;
        }

        private async Task<long> CreateInactiveProductSku()
        {
            CreateProductSkuRequest createRequest = new(
                ProductId: SMARTPHONE_GALAXY_X_ID,
                BarCode: $"780{Guid.NewGuid().ToString()[..10]}",
                Price: 799990m,
                Cost: 550000m,
                Weight: 0.180m,
                Length: 15.8m,
                Width: 7.4m,
                Height: 0.8m,
                IsActive: false,
                DisplayOrder: 1);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long productSkuId = await response.Content.ReadFromJsonAsync<long>();
            return productSkuId;
        }

        private async Task<long> CreateActiveProductSkuWithDimensions()
        {
            CreateProductSkuRequest createRequest = new(
                ProductId: LAPTOP_ULTRABOOK_PRO_ID,
                BarCode: $"780{Guid.NewGuid().ToString()[..10]}",
                Price: 1799990m,
                Cost: 1400000m,
                Weight: 1.350m,
                Length: 35.0m,
                Width: 25.0m,
                Height: 2.0m,
                IsActive: true,
                DisplayOrder: 2);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long productSkuId = await response.Content.ReadFromJsonAsync<long>();
            return productSkuId;
        }

        private async Task<long> CreateActiveProductSkuWithMinimalFields()
        {
            CreateProductSkuRequest createRequest = new(
                ProductId: IPHONE_PRO_MAX_ID,
                BarCode: null,
                Price: 1199990m,
                Cost: null,
                Weight: null,
                Length: null,
                Width: null,
                Height: null,
                IsActive: true,
                DisplayOrder: 3);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long productSkuId = await response.Content.ReadFromJsonAsync<long>();
            return productSkuId;
        }

        private async Task<long> CreateActiveProductSkuWithHighPrice()
        {
            CreateProductSkuRequest createRequest = new(
                ProductId: SMARTPHONE_GALAXY_X_ID,
                BarCode: $"780{Guid.NewGuid().ToString()[..10]}",
                Price: 99999999m,
                Cost: 50000000m,
                Weight: 0.180m,
                Length: 15.8m,
                Width: 7.4m,
                Height: 0.8m,
                IsActive: true,
                DisplayOrder: 1);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long productSkuId = await response.Content.ReadFromJsonAsync<long>();
            return productSkuId;
        }

        #endregion
    }
}

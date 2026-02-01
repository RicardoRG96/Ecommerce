using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.ProductSku.Create;

namespace Api.FunctionalTests.Products.ProductSku
{
    public class ActivateProductSkuTests : BaseFunctionalTest
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

        /// <summary>
        /// Product ID 20: Escritorio Ajustable Pro - Not published on seed, Published with 1 SKU
        /// </summary>
        private const long UNPUBLISHED_PRODUCT_ID = 20;

        /// <summary>
        /// ProductSku ID 22: DESK-ADJ - Not published on seed, assign to Product ID 20
        /// </summary>
        private const long UNPUBLISHED_PRODUCT_SKU_ID = 22;

        #endregion

        public ActivateProductSkuTests(FunctionalTestWebAppFactory factory) 
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
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/{invalidProductSkuId}/activate", null!);

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
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/{Constants.NotExistingId}/activate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenProductSkuIdExists()
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateInactiveProductSku();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}/activate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenProductSkuIsAlreadyActive()
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateActiveProductSku();

            // Act - First activation (already active)
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}/activate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "activating an already active product SKU should be idempotent");
        }

        [Fact]
        public async Task Should_ActivateInactiveProductSku_Successfully()
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateInactiveProductSku();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}/activate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenProductIsNotPublished()
        {
            // Arrange
            SetAdminAuthentication();

            // Uses the product SKU associated with an unpublished product from seed data
            long productSkuId = UNPUBLISHED_PRODUCT_SKU_ID;

            // Act - Try to activate SKU with unpublished product
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}/activate", null!);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("not published",
                because: "error should indicate that the product is not published");
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
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/1/activate", null!);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserIsNotAdmin()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/1/activate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Theory]
        [InlineData(SMARTPHONE_GALAXY_X_ID, "Smartphone Galaxy X")]
        [InlineData(IPHONE_PRO_MAX_ID, "iPhone Pro Max")]
        [InlineData(LAPTOP_ULTRABOOK_PRO_ID, "Laptop Ultrabook Pro")]
        public async Task Should_ActivateProductSku_ForDifferentSeededProducts(
            long productId,
            string productName)
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateInactiveProductSku(productId);

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}/activate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent, 
                because: $"activating SKU for {productName}");
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenActivatingProductSkuWithPhysicalDimensions()
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateInactiveProductSkuWithDimensions();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}/activate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenActivatingProductSkuWithNullOptionalFields()
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateInactiveProductSkuWithMinimalFields();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}/activate", null!);

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

        private async Task<long> CreateInactiveProductSku(long? productId = null)
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
                IsActive: false,
                DisplayOrder: 1);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long productSkuId = await response.Content.ReadFromJsonAsync<long>();
            return productSkuId;
        }

        private async Task<long> CreateInactiveProductSkuWithDimensions()
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
                IsActive: false,
                DisplayOrder: 2);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long productSkuId = await response.Content.ReadFromJsonAsync<long>();
            return productSkuId;
        }

        private async Task<long> CreateInactiveProductSkuWithMinimalFields()
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
                IsActive: false,
                DisplayOrder: 3);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long productSkuId = await response.Content.ReadFromJsonAsync<long>();
            return productSkuId;
        }

        #endregion
    }
}

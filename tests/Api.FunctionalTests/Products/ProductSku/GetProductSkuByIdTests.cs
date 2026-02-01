using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Products.ProductSkus.Common.Mappers;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.ProductSku.Create;

namespace Api.FunctionalTests.Products.ProductSku
{
    public class GetProductSkuByIdTests : BaseFunctionalTest
    {
        #region Seed Data Constants - ProductSkus

        /// <summary>
        /// ProductSku ID 1: GALX-128-BLK from Smartphone Galaxy X (Product 1)
        /// BarCode: 780000000001, Price: 799990, Cost: 550000
        /// Weight: 0.180, Length: 15.8, Width: 7.4, Height: 0.8
        /// IsActive: true, DisplayOrder: 1
        /// </summary>
        private const long SMARTPHONE_SKU_128GB_ID = 1;

        /// <summary>
        /// ProductSku ID 2: GALX-256-BLK from Smartphone Galaxy X (Product 1)
        /// BarCode: 780000000002, Price: 849990, Cost: 600000
        /// Weight: 0.182, Length: 15.8, Width: 7.4, Height: 0.8
        /// IsActive: true, DisplayOrder: 2
        /// </summary>
        private const long SMARTPHONE_SKU_256GB_ID = 2;

        /// <summary>
        /// ProductSku ID 3: IPPM-256-SLV from iPhone Pro Max (Product 2)
        /// BarCode: 780000000003, Price: 1199990, Cost: 900000
        /// Weight: 0.221, Length: 16.0, Width: 7.8, Height: 0.8
        /// IsActive: true, DisplayOrder: 1
        /// </summary>
        private const long IPHONE_SKU_256GB_ID = 3;

        /// <summary>
        /// ProductSku ID 5: ULTRA-I7-16GB from Laptop Ultrabook Pro (Product 3)
        /// BarCode: 780000000005, Price: 1499990, Cost: 1200000
        /// Weight: 1.250, Length: 32.0, Width: 22.0, Height: 1.6
        /// IsActive: true, DisplayOrder: 1
        /// </summary>
        private const long LAPTOP_SKU_I7_ID = 5;

        /// <summary>
        /// ProductSku ID 9: MOUSE-ERGO from Mouse Inalámbrico Ergo (Product 7)
        /// BarCode: 780000000009, Price: 29990, Cost: 15000
        /// Weight: 0.095, Length: 12.0, Width: 7.0, Height: 4.0
        /// IsActive: true, DisplayOrder: 1
        /// </summary>
        private const long MOUSE_SKU_ERGO_ID = 9;

        /// <summary>
        /// ProductSku ID 12: SSD-NVME-1TB from Disco SSD NVMe 1TB (Product 10)
        /// BarCode: 780000000012, Price: 129990, Cost: 90000
        /// Weight: 0.030, Length: 8.0, Width: 2.2, Height: 0.3
        /// IsActive: true, DisplayOrder: 1
        /// </summary>
        private const long SSD_SKU_1TB_ID = 12;

        #endregion

        #region Seed Data Constants - Products

        private const long SMARTPHONE_GALAXY_X_ID = 1;
        private const long IPHONE_PRO_MAX_ID = 2;
        private const long LAPTOP_ULTRABOOK_PRO_ID = 3;

        #endregion

        public GetProductSkuByIdTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Theory]
        [InlineData(0, "ProductSkuId", "is missing")]
        [InlineData(-1, "ProductSkuId", "is negative")]
        public async Task Should_ReturnNotFound_WhenProductSkuIdIsInvalid(
            long invalidProductSkuId,
            string fieldName,
            string reason)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.ProductSkus.Base}/{invalidProductSkuId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                because: $"{fieldName} {reason}");
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenProductSkuIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.ProductSkus.Base}/{Constants.NotExistingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_AndProductSku_WhenProductSkuExists()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded SKU: GALX-128-BLK
            ProductSkuResponse? productSku = await HttpClient.GetFromJsonAsync<ProductSkuResponse>(
                $"{ApiRoutes.ProductSkus.Base}/{SMARTPHONE_SKU_128GB_ID}");

            // Assert
            productSku.Should().NotBeNull();
            productSku!.Id.Should().Be(SMARTPHONE_SKU_128GB_ID);
            productSku.SkuCode.Should().Be("GALX-128-BLK");
            productSku.BarCode.Should().Be("780000000001");
            productSku.Price.Should().Be(799990);
        }

        [Fact]
        public async Task Should_ReturnProductSkuWithAllFields()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded SKU: GALX-128-BLK
            ProductSkuResponse? productSku = await HttpClient.GetFromJsonAsync<ProductSkuResponse>(
                $"{ApiRoutes.ProductSkus.Base}/{SMARTPHONE_SKU_128GB_ID}");

            // Assert
            productSku.Should().NotBeNull();
            productSku!.Id.Should().Be(SMARTPHONE_SKU_128GB_ID);
            productSku!.Product!.Id.Should().Be(SMARTPHONE_GALAXY_X_ID);
            productSku.SkuCode.Should().Be("GALX-128-BLK");
            productSku.BarCode.Should().Be("780000000001");
            productSku.Price.Should().Be(799990);
            productSku.Weight.Should().Be(0.180m);
            productSku.Length.Should().Be(15.8m);
            productSku.Width.Should().Be(7.4m);
            productSku.Height.Should().Be(0.8m);
            productSku.IsActive.Should().BeTrue();
            productSku.DisplayOrder.Should().Be(1);
        }

        [Fact]
        public async Task Should_ReturnProductSkuWithProductInformation()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded SKU: GALX-128-BLK
            ProductSkuResponse? productSku = await HttpClient.GetFromJsonAsync<ProductSkuResponse>(
                $"{ApiRoutes.ProductSkus.Base}/{SMARTPHONE_SKU_128GB_ID}");

            // Assert
            productSku.Should().NotBeNull();
            productSku!.Product.Should().NotBeNull();
            productSku.Product!.Id.Should().Be(SMARTPHONE_GALAXY_X_ID);
            productSku.Product.Name.Should().Be("Smartphone Galaxy X");
            productSku.Product.Slug.Should().Be("smartphone-galaxy-x");
            productSku.Product.IsActive.Should().BeTrue();
        }

        [Theory]
        [InlineData(SMARTPHONE_SKU_128GB_ID, "GALX-128-BLK", "780000000001", 799990)]
        [InlineData(SMARTPHONE_SKU_256GB_ID, "GALX-256-BLK", "780000000002", 849990)]
        [InlineData(IPHONE_SKU_256GB_ID, "IPPM-256-SLV", "780000000003", 1199990)]
        public async Task Should_ReturnSeededProductSku_WithCorrectProperties(
            long productSkuId,
            string expectedSkuCode,
            string expectedBarCode,
            decimal expectedPrice)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            ProductSkuResponse? productSku = await HttpClient.GetFromJsonAsync<ProductSkuResponse>(
                $"{ApiRoutes.ProductSkus.Base}/{productSkuId}");

            // Assert
            productSku.Should().NotBeNull();
            productSku!.SkuCode.Should().Be(expectedSkuCode);
            productSku.BarCode.Should().Be(expectedBarCode);
            productSku.Price.Should().Be(expectedPrice);
        }

        [Fact]
        public async Task Should_ReturnProductSkuWithPhysicalDimensions()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded SKU: ULTRA-I7-16GB (Laptop with large dimensions)
            ProductSkuResponse? productSku = await HttpClient.GetFromJsonAsync<ProductSkuResponse>(
                $"{ApiRoutes.ProductSkus.Base}/{LAPTOP_SKU_I7_ID}");

            // Assert
            productSku.Should().NotBeNull();
            productSku!.Weight.Should().Be(1.250m,
                because: "laptop weight in kg");
            productSku.Length.Should().Be(32.0m,
                because: "laptop length in cm");
            productSku.Width.Should().Be(22.0m,
                because: "laptop width in cm");
            productSku.Height.Should().Be(1.6m,
                because: "laptop height in cm");
        }

        [Fact]
        public async Task Should_ReturnProductSkuWithSmallDimensions()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded SKU: SSD-NVME-1TB (Small product)
            ProductSkuResponse? productSku = await HttpClient.GetFromJsonAsync<ProductSkuResponse>(
                $"{ApiRoutes.ProductSkus.Base}/{SSD_SKU_1TB_ID}");

            // Assert
            productSku.Should().NotBeNull();
            productSku!.Weight.Should().Be(0.030m,
                because: "SSD is very light");
            productSku.Length.Should().Be(8.0m);
            productSku.Width.Should().Be(2.2m);
            productSku.Height.Should().Be(0.3m);
        }

        [Fact]
        public async Task Should_ReturnActiveProductSku()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - All seeded SKUs are active
            ProductSkuResponse? productSku = await HttpClient.GetFromJsonAsync<ProductSkuResponse>(
                $"{ApiRoutes.ProductSkus.Base}/{SMARTPHONE_SKU_128GB_ID}");

            // Assert
            productSku.Should().NotBeNull();
            productSku!.IsActive.Should().BeTrue(
                because: "all seeded product SKUs are active");
        }

        [Fact]
        public async Task Should_ReturnInactiveProductSku()
        {
            // Arrange
            SetAdminAuthentication();
            long inactiveSkuId = await CreateInactiveProductSku();

            // Act
            ProductSkuResponse? productSku = await HttpClient.GetFromJsonAsync<ProductSkuResponse>(
                $"{ApiRoutes.ProductSkus.Base}/{inactiveSkuId}");

            // Assert
            productSku.Should().NotBeNull();
            productSku!.IsActive.Should().BeFalse(
                because: "the SKU was created as inactive");
        }

        [Fact]
        public async Task Should_ReturnProductSkuWithDisplayOrder()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded SKU: GALX-128-BLK (DisplayOrder: 1)
            ProductSkuResponse? productSku = await HttpClient.GetFromJsonAsync<ProductSkuResponse>(
                $"{ApiRoutes.ProductSkus.Base}/{SMARTPHONE_SKU_128GB_ID}");

            // Assert
            productSku.Should().NotBeNull();
            productSku!.DisplayOrder.Should().Be(1);
        }

        [Fact]
        public async Task Should_ReturnProductSkuWithBarCode()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded SKU: GALX-128-BLK
            ProductSkuResponse? productSku = await HttpClient.GetFromJsonAsync<ProductSkuResponse>(
                $"{ApiRoutes.ProductSkus.Base}/{SMARTPHONE_SKU_128GB_ID}");

            // Assert
            productSku.Should().NotBeNull();
            productSku!.BarCode.Should().NotBeNullOrEmpty();
            productSku.BarCode.Should().Be("780000000001");
        }

        [Fact]
        public async Task Should_ReturnProductSkuWithNullBarCode()
        {
            // Arrange
            SetAdminAuthentication();
            long skuIdWithNullBarCode = await CreateProductSkuWithNullBarCode();

            // Act
            ProductSkuResponse? productSku = await HttpClient.GetFromJsonAsync<ProductSkuResponse>(
                $"{ApiRoutes.ProductSkus.Base}/{skuIdWithNullBarCode}");

            // Assert
            productSku.Should().NotBeNull();
            productSku!.BarCode.Should().BeNull();
        }

        [Fact]
        public async Task Should_ReturnProductSkuWithNullDimensions()
        {
            // Arrange
            SetAdminAuthentication();
            long skuIdWithNullDimensions = await CreateProductSkuWithNullDimensions();

            // Act
            ProductSkuResponse? productSku = await HttpClient.GetFromJsonAsync<ProductSkuResponse>(
                $"{ApiRoutes.ProductSkus.Base}/{skuIdWithNullDimensions}");

            // Assert
            productSku.Should().NotBeNull();
            productSku!.Weight.Should().BeNull();
            productSku.Length.Should().BeNull();
            productSku.Width.Should().BeNull();
            productSku.Height.Should().BeNull();
        }

        [Theory]
        [InlineData(SMARTPHONE_SKU_128GB_ID, SMARTPHONE_GALAXY_X_ID, "Smartphone Galaxy X")]
        [InlineData(IPHONE_SKU_256GB_ID, IPHONE_PRO_MAX_ID, "iPhone Pro Max")]
        [InlineData(LAPTOP_SKU_I7_ID, LAPTOP_ULTRABOOK_PRO_ID, "Laptop Ultrabook Pro")]
        public async Task Should_ReturnProductSkuWithCorrectProductAssociation(
            long productSkuId,
            long expectedProductId,
            string expectedProductName)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            ProductSkuResponse? productSku = await HttpClient.GetFromJsonAsync<ProductSkuResponse>(
                $"{ApiRoutes.ProductSkus.Base}/{productSkuId}");

            // Assert
            productSku.Should().NotBeNull();
            productSku!.Product!.Id.Should().Be(expectedProductId);
            productSku.Product.Should().NotBeNull();
            productSku.Product!.Id.Should().Be(expectedProductId);
            productSku.Product.Name.Should().Be(expectedProductName);
        }

        [Fact]
        public async Task Should_ReturnProductSkuWithLowPrice()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded SKU: MOUSE-ERGO (lowest price)
            ProductSkuResponse? productSku = await HttpClient.GetFromJsonAsync<ProductSkuResponse>(
                $"{ApiRoutes.ProductSkus.Base}/{MOUSE_SKU_ERGO_ID}");

            // Assert
            productSku.Should().NotBeNull();
            productSku!.Price.Should().Be(29990,
                because: "mouse has the lowest price in seeded data");
        }

        [Fact]
        public async Task Should_ReturnProductSkuWithHighPrice()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded SKU: ULTRA-I7-16GB (high price)
            ProductSkuResponse? productSku = await HttpClient.GetFromJsonAsync<ProductSkuResponse>(
                $"{ApiRoutes.ProductSkus.Base}/{LAPTOP_SKU_I7_ID}");

            // Assert
            productSku.Should().NotBeNull();
            productSku!.Price.Should().Be(1499990,
                because: "laptop has one of the highest prices");
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
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.ProductSkus.Base}/1");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserDoesNotHavePermission()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.ProductSkus.Base}/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        #region Helper Methods

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
                DisplayOrder: 10);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long productSkuId = await response.Content.ReadFromJsonAsync<long>();
            return productSkuId;
        }

        private async Task<long> CreateProductSkuWithNullBarCode()
        {
            CreateProductSkuRequest createRequest = new(
                ProductId: SMARTPHONE_GALAXY_X_ID,
                BarCode: null,
                Price: 799990m,
                Cost: 550000m,
                Weight: 0.180m,
                Length: 15.8m,
                Width: 7.4m,
                Height: 0.8m,
                IsActive: true,
                DisplayOrder: 10);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long productSkuId = await response.Content.ReadFromJsonAsync<long>();
            return productSkuId;
        }

        private async Task<long> CreateProductSkuWithNullDimensions()
        {
            CreateProductSkuRequest createRequest = new(
                ProductId: SMARTPHONE_GALAXY_X_ID,
                BarCode: $"780{Guid.NewGuid().ToString()[..10]}",
                Price: 799990m,
                Cost: 550000m,
                Weight: null,
                Length: null,
                Width: null,
                Height: null,
                IsActive: true,
                DisplayOrder: 10);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long productSkuId = await response.Content.ReadFromJsonAsync<long>();
            return productSkuId;
        }

        #endregion
    }
}

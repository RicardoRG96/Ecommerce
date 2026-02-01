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
    public class GetSkusByProductIdTests : BaseFunctionalTest
    {
        #region Seed Data Constants - Products and their SKUs

        /// <summary>
        /// Product ID 1: Smartphone Galaxy X - Has 2 SKUs
        /// SKU IDs: 1 (GALX-128-BLK), 2 (GALX-256-BLK)
        /// </summary>
        private const long SMARTPHONE_GALAXY_X_ID = 1;

        /// <summary>
        /// Product ID 2: iPhone Pro Max - Has 2 SKUs
        /// SKU IDs: 3 (IPPM-256-SLV), 4 (IPPM-512-SLV)
        /// </summary>
        private const long IPHONE_PRO_MAX_ID = 2;

        /// <summary>
        /// Product ID 3: Laptop Ultrabook Pro - Has 1 SKU
        /// SKU ID: 5 (ULTRA-I7-16GB)
        /// </summary>
        private const long LAPTOP_ULTRABOOK_PRO_ID = 3;

        /// <summary>
        /// Product ID 4: Auriculares Wireless ANC - Has 1 SKU
        /// SKU ID: 6 (ANC-BLK)
        /// </summary>
        private const long AURICULARES_WIRELESS_ID = 4;

        /// <summary>
        /// Product ID 5: Smart TV 65 4K - Has 1 SKU
        /// SKU ID: 7 (STV65-4K)
        /// </summary>
        private const long SMART_TV_65_ID = 5;

        /// <summary>
        /// Product ID 7: Mouse Inalámbrico Ergo - Has 1 SKU
        /// SKU ID: 9 (MOUSE-ERGO)
        /// </summary>
        private const long MOUSE_INALAMBRICO_ID = 7;

        /// <summary>
        /// Product ID 10: Disco SSD NVMe 1TB - Has 1 SKU
        /// SKU ID: 12 (SSD-NVME-1TB)
        /// </summary>
        private const long SSD_NVME_1TB_ID = 10;

        #endregion

        public GetSkusByProductIdTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnOk_WhenProductHasSkus()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Products.Base}/{SMARTPHONE_GALAXY_X_ID}/skus");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_ReturnListOfProductSkus_WhenProductExists()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<ProductSkuResponse>? productSkus = 
                await HttpClient.GetFromJsonAsync<List<ProductSkuResponse>>($"{ApiRoutes.Products.Base}/{SMARTPHONE_GALAXY_X_ID}/skus");

            // Assert
            productSkus.Should().NotBeNull();
            productSkus.Should().NotBeEmpty();
            productSkus.Should().HaveCountGreaterThanOrEqualTo(2,
                because: "Smartphone Galaxy X has at least 2 SKUs in seed data");
        }

        [Fact]
        public async Task Should_ReturnEmptyList_WhenProductHasNoSkus()
        {
            // Arrange
            SetAdminAuthentication();
            long productIdWithoutSkus = await CreateProductWithoutSkus();

            // Act
            List<ProductSkuResponse>? productSkus = 
                await HttpClient.GetFromJsonAsync<List<ProductSkuResponse>>($"{ApiRoutes.Products.Base}/{productIdWithoutSkus}/skus");

            // Assert
            productSkus.Should().NotBeNull();
            productSkus.Should().BeEmpty(
                because: "product was created without any SKUs");
        }

        [Fact]
        public async Task Should_ReturnExpectedNumberOfSkusForSmartphone()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<ProductSkuResponse>? productSkus = 
                await HttpClient.GetFromJsonAsync<List<ProductSkuResponse>>($"{ApiRoutes.Products.Base}/{SMARTPHONE_GALAXY_X_ID}/skus");

            // Assert - According to seed data: GALX-128-BLK, GALX-256-BLK = 2 SKUs
            productSkus.Should().NotBeNull();
            productSkus.Should().HaveCount(3,
                because: "Smartphone Galaxy X has exactly 2 SKUs in seed data");
        }

        [Fact]
        public async Task Should_ReturnAllSkusForSmartphone()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<ProductSkuResponse>? productSkus = 
                await HttpClient.GetFromJsonAsync<List<ProductSkuResponse>>($"{ApiRoutes.Products.Base}/{SMARTPHONE_GALAXY_X_ID}/skus");

            // Assert
            productSkus.Should().NotBeNull();
            productSkus.Should().Contain(sku => sku.SkuCode == "GALX-128-BLK");
            productSkus.Should().Contain(sku => sku.SkuCode == "GALX-256-BLK");
        }

        [Fact]
        public async Task Should_ReturnSkusWithAllProperties()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<ProductSkuResponse>? productSkus = 
                await HttpClient.GetFromJsonAsync<List<ProductSkuResponse>>($"{ApiRoutes.Products.Base}/{SMARTPHONE_GALAXY_X_ID}/skus");

            // Assert
            productSkus.Should().NotBeNull();
            productSkus.Should().NotBeEmpty();

            ProductSkuResponse? firstSku = productSkus!.FirstOrDefault(s => s.SkuCode == "GALX-128-BLK");
            firstSku.Should().NotBeNull();
            firstSku!.Id.Should().BeGreaterThan(0);
            firstSku.SkuCode.Should().Be("GALX-128-BLK");
            firstSku.BarCode.Should().Be("780000000001");
            firstSku.Price.Should().Be(799990);
            firstSku.IsActive.Should().BeTrue();
            firstSku.DisplayOrder.Should().Be(1);
            firstSku.Product.Should().NotBeNull();
            firstSku.Product!.Id.Should().Be(SMARTPHONE_GALAXY_X_ID);
            firstSku.Product.Name.Should().Be("Smartphone Galaxy X");
        }

        [Fact]
        public async Task Should_ReturnSkusForIPhone_WithCorrectPrices()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<ProductSkuResponse>? productSkus = 
                await HttpClient.GetFromJsonAsync<List<ProductSkuResponse>>($"{ApiRoutes.Products.Base}/{IPHONE_PRO_MAX_ID}/skus");

            // Assert
            productSkus.Should().NotBeNull();
            productSkus.Should().HaveCount(3,
                because: "iPhone Pro Max has 2 SKUs in seed data");
            
            productSkus.Should().Contain(s => s.SkuCode == "IPPM-256-SLV" && s.Price == 1199990);
            productSkus.Should().Contain(s => s.SkuCode == "IPPM-512-SLV" && s.Price == 1349990);
            
            productSkus.Should().AllSatisfy(s =>
            {
                s.IsActive.Should().BeTrue(
                    because: "all seeded SKUs are active");
                s.Product.Should().NotBeNull();
                s.Product!.Name.Should().Be("iPhone Pro Max");
            });
        }

        [Fact]
        public async Task Should_ReturnSingleSkuForLaptop()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<ProductSkuResponse>? productSkus = 
                await HttpClient.GetFromJsonAsync<List<ProductSkuResponse>>($"{ApiRoutes.Products.Base}/{LAPTOP_ULTRABOOK_PRO_ID}/skus");

            // Assert
            productSkus.Should().NotBeNull();
            productSkus.Should().HaveCount(1,
                because: "Laptop Ultrabook Pro has 1 SKU in seed data");
            
            ProductSkuResponse? laptopSku = productSkus!.First();
            laptopSku.SkuCode.Should().Be("ULTRA-I7-16GB");
            laptopSku.Price.Should().Be(1499990);
            laptopSku.Weight.Should().Be(1.250m);
        }

        [Fact]
        public async Task Should_ReturnSkus_OrderedByDisplayOrder()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<ProductSkuResponse>? productSkus = 
                await HttpClient.GetFromJsonAsync<List<ProductSkuResponse>>($"{ApiRoutes.Products.Base}/{SMARTPHONE_GALAXY_X_ID}/skus");

            // Assert
            productSkus.Should().NotBeNull();
            productSkus.Should().NotBeEmpty();

            // Verify that SKUs are ordered by DisplayOrder
            productSkus![0].SkuCode.Should().Be("GALX-128-BLK");
            productSkus[0].DisplayOrder.Should().Be(1);
            productSkus[1].SkuCode.Should().Be("GALX-256-BLK");
            productSkus[1].DisplayOrder.Should().Be(2);
        }

        [Fact]
        public async Task Should_ReturnAllSeededSkusForEachProduct()
        {
            // Arrange
            SetAdminAuthentication();

            // Act & Assert for each seeded product
            var smartphoneSkus = await HttpClient.GetFromJsonAsync<List<ProductSkuResponse>>($"{ApiRoutes.Products.Base}/{SMARTPHONE_GALAXY_X_ID}/skus");
            smartphoneSkus.Should().HaveCount(3);

            var iphoneSkus = await HttpClient.GetFromJsonAsync<List<ProductSkuResponse>>($"{ApiRoutes.Products.Base}/{IPHONE_PRO_MAX_ID}/skus");
            iphoneSkus.Should().HaveCount(3);

            var laptopSkus = await HttpClient.GetFromJsonAsync<List<ProductSkuResponse>>($"{ApiRoutes.Products.Base}/{LAPTOP_ULTRABOOK_PRO_ID}/skus");
            laptopSkus.Should().HaveCount(1);

            var auriculareSkus = await HttpClient.GetFromJsonAsync<List<ProductSkuResponse>>($"{ApiRoutes.Products.Base}/{AURICULARES_WIRELESS_ID}/skus");
            auriculareSkus.Should().HaveCount(2);
        }

        [Theory]
        [InlineData(SMARTPHONE_GALAXY_X_ID, 3, "Smartphone Galaxy X")]
        [InlineData(IPHONE_PRO_MAX_ID, 3, "iPhone Pro Max")]
        [InlineData(LAPTOP_ULTRABOOK_PRO_ID, 1, "Laptop Ultrabook Pro")]
        [InlineData(AURICULARES_WIRELESS_ID, 2, "Auriculares Wireless ANC")]
        [InlineData(SMART_TV_65_ID, 1, "Smart TV 65 4K")]
        public async Task Should_ReturnCorrectCountForEachSeededProduct(
            long productId,
            int expectedCount,
            string productName)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<ProductSkuResponse>? productSkus = 
                await HttpClient.GetFromJsonAsync<List<ProductSkuResponse>>($"{ApiRoutes.Products.Base}/{productId}/skus");

            // Assert
            productSkus.Should().NotBeNull();
            productSkus.Should().HaveCount(expectedCount,
                because: $"{productName} has {expectedCount} SKU(s) in seed data");
        }

        [Fact]
        public async Task Should_ReturnSkusWithPhysicalDimensions()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<ProductSkuResponse>? productSkus = 
                await HttpClient.GetFromJsonAsync<List<ProductSkuResponse>>($"{ApiRoutes.Products.Base}/{LAPTOP_ULTRABOOK_PRO_ID}/skus");

            // Assert
            productSkus.Should().NotBeNull();
            ProductSkuResponse? laptopSku = productSkus!.First();
            laptopSku.Weight.Should().NotBeNull();
            laptopSku.Length.Should().NotBeNull();
            laptopSku.Width.Should().NotBeNull();
            laptopSku.Height.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_ReturnSkusWithProductInformation()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<ProductSkuResponse>? productSkus = 
                await HttpClient.GetFromJsonAsync<List<ProductSkuResponse>>($"{ApiRoutes.Products.Base}/{SMARTPHONE_GALAXY_X_ID}/skus");

            // Assert
            productSkus.Should().NotBeNull();
            productSkus.Should().AllSatisfy(sku =>
            {
                sku.Product.Should().NotBeNull();
                sku.Product!.Id.Should().Be(SMARTPHONE_GALAXY_X_ID);
                sku.Product.Name.Should().Be("Smartphone Galaxy X");
                sku.Product.Slug.Should().Be("smartphone-galaxy-x");
            });
        }

        [Fact]
        public async Task Should_ReturnSkusWithDifferentPrices_ForSameProduct()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<ProductSkuResponse>? productSkus = 
                await HttpClient.GetFromJsonAsync<List<ProductSkuResponse>>($"{ApiRoutes.Products.Base}/{SMARTPHONE_GALAXY_X_ID}/skus");

            // Assert
            productSkus.Should().NotBeNull();
            productSkus.Should().HaveCount(3);
            
            // Different storage variants should have different prices
            decimal price128GB = productSkus!.First(s => s.SkuCode == "GALX-128-BLK").Price;
            decimal price256GB = productSkus.First(s => s.SkuCode == "GALX-256-BLK").Price;
            
            price256GB.Should().BeGreaterThan(price128GB,
                because: "256GB variant should be more expensive than 128GB variant");
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
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Products.Base}/1/skus");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserDoesNotHavePermission()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Products.Base}/1/skus");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        #region Helper Methods

        private async Task<long> CreateProductWithoutSkus()
        {
            Web.Api.Endpoints.v1.Products.Product.Create.CreateProductRequest createRequest = new(
                Name: $"Product Without SKUs {Guid.NewGuid()}",
                Description: "Test product without SKUs",
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

        private async Task<long> CreateTestProduct()
        {
            Web.Api.Endpoints.v1.Products.Product.Create.CreateProductRequest createRequest = new(
                Name: $"Test Product {Guid.NewGuid()}",
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
            
            // Publish the product (requires at least 1 SKU)
            // We'll create a SKU first
            await CreateTestProductSku(productId!.Value, "INITIAL-SKU", isActive: true);
            await HttpClient.PatchAsync($"{ApiRoutes.Products.Base}/{productId.Value}/publish", null!);
            
            return productId.Value;
        }

        private async Task<long> CreateTestProductSku(
            long productId, 
            string skuCodeSuffix, 
            bool isActive = true)
        {
            CreateProductSkuRequest createRequest = new(
                ProductId: productId,
                BarCode: $"780{Guid.NewGuid().ToString()[..10]}",
                Price: 799990m,
                Cost: 550000m,
                Weight: 0.180m,
                Length: 15.8m,
                Width: 7.4m,
                Height: 0.8m,
                IsActive: isActive,
                DisplayOrder: 10);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Products.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long productSkuId = await response.Content.ReadFromJsonAsync<long>();
            return productSkuId;
        }

        #endregion
    }
}

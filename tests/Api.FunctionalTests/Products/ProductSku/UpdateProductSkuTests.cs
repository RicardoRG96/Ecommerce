using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.Product.Create;
using Web.Api.Endpoints.v1.Products.ProductSku.Create;
using Web.Api.Endpoints.v1.Products.ProductSku.Update;

namespace Api.FunctionalTests.Products.ProductSku
{
    public class UpdateProductSkuTests : BaseFunctionalTest
    {
        #region Seed Data Constants - Published Products with SKUs

        /// <summary>
        /// Product ID 1: Smartphone Galaxy X - Published with 2 SKUs
        /// SKU IDs: 1 (GALX-128-BLK), 2 (GALX-256-BLK)
        /// </summary>
        private const long SMARTPHONE_GALAXY_X_ID = 1;

        /// <summary>
        /// Product ID 2: iPhone Pro Max - Published with 2 SKUs
        /// SKU IDs: 3 (IPPM-256-SLV), 4 (IPPM-512-SLV)
        /// </summary>
        private const long IPHONE_PRO_MAX_ID = 2;

        /// <summary>
        /// Product ID 3: Laptop Ultrabook Pro - Published with 1 SKU
        /// SKU ID: 5 (ULTRA-I7-16GB)
        /// </summary>
        private const long LAPTOP_ULTRABOOK_PRO_ID = 3;

        /// <summary>
        /// ProductSku ID 1: GALX-128-BLK from Smartphone Galaxy X
        /// BarCode: 780000000001, Price: 799990
        /// </summary>
        private const long SMARTPHONE_SKU_1_ID = 1;

        /// <summary>
        /// ProductSku ID 2: GALX-256-BLK from Smartphone Galaxy X
        /// BarCode: 780000000002, Price: 849990
        /// </summary>
        private const long SMARTPHONE_SKU_2_ID = 2;

        #endregion

        private static readonly UpdateProductSkuRequest _request = new(
            ProductId: SMARTPHONE_GALAXY_X_ID,
            BarCode: "780000000999",
            Price: 899990m,
            Cost: 650000m,
            Weight: 0.185m,
            Length: 15.8m,
            Width: 7.4m,
            Height: 0.8m,
            DisplayOrder: 3);

        public UpdateProductSkuTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        #region Validation Tests - FluentValidation

        [Theory]
        [MemberData(nameof(GetInvalidMaxLengthRequests))]
        public async Task Should_ReturnBadRequest_WhenFieldExceedsMaximumLength(
            UpdateProductSkuRequest invalidRequest, 
            string fieldName)
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateTestProductSku();

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, 
                because: $"{fieldName} exceeds maximum length");
        }

        public static IEnumerable<object[]> GetInvalidMaxLengthRequests()
        {
            yield return new object[] 
            { 
                _request with { BarCode = Constants.ExceededMaximumLengthField }, 
                nameof(UpdateProductSkuRequest.BarCode) 
            };
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
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{invalidProductSkuId}", _request);

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
            long productSkuId = await CreateTestProductSku();
            UpdateProductSkuRequest invalidRequest = _request with { ProductId = invalidProductId };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, 
                because: $"{fieldName} {reason}");
        }

        [Theory]
        [InlineData(-1, "Price", "is negative")]
        [InlineData(-100, "Price", "is negative")]
        public async Task Should_ReturnBadRequest_WhenPriceIsNegative(
            decimal invalidPrice, 
            string fieldName, 
            string reason)
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateTestProductSku();
            UpdateProductSkuRequest invalidRequest = _request with { Price = invalidPrice };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, 
                because: $"{fieldName} {reason}");
            content.Should().Contain("Price must be greater than or equal to zero");
        }

        [Theory]
        [InlineData(-1, "Cost")]
        [InlineData(-100, "Cost")]
        public async Task Should_ReturnBadRequest_WhenCostIsNegative(
            decimal invalidCost, 
            string fieldName)
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateTestProductSku();
            UpdateProductSkuRequest invalidRequest = _request with { Cost = invalidCost };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain($"{fieldName} must be greater than or equal to zero");
        }

        [Theory]
        [InlineData(-1, "Weight")]
        [InlineData(-10, "Weight")]
        public async Task Should_ReturnBadRequest_WhenWeightIsNegative(
            decimal invalidWeight, 
            string fieldName)
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateTestProductSku();
            UpdateProductSkuRequest invalidRequest = _request with { Weight = invalidWeight };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain($"{fieldName} must be greater than or equal to zero");
        }

        [Theory]
        [InlineData(-1, "Length")]
        [InlineData(-10, "Length")]
        public async Task Should_ReturnBadRequest_WhenLengthIsNegative(
            decimal invalidLength, 
            string fieldName)
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateTestProductSku();
            UpdateProductSkuRequest invalidRequest = _request with { Length = invalidLength };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain($"{fieldName} must be greater than or equal to zero");
        }

        [Theory]
        [InlineData(-1, "Width")]
        [InlineData(-10, "Width")]
        public async Task Should_ReturnBadRequest_WhenWidthIsNegative(
            decimal invalidWidth, 
            string fieldName)
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateTestProductSku();
            UpdateProductSkuRequest invalidRequest = _request with { Width = invalidWidth };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain($"{fieldName} must be greater than or equal to zero");
        }

        [Theory]
        [InlineData(-1, "Height")]
        [InlineData(-10, "Height")]
        public async Task Should_ReturnBadRequest_WhenHeightIsNegative(
            decimal invalidHeight, 
            string fieldName)
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateTestProductSku();
            UpdateProductSkuRequest invalidRequest = _request with { Height = invalidHeight };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain($"{fieldName} must be greater than or equal to zero");
        }

        [Theory]
        [InlineData(-1, "DisplayOrder", "is negative")]
        [InlineData(-10, "DisplayOrder", "is negative")]
        public async Task Should_ReturnBadRequest_WhenDisplayOrderIsNegative(
            int invalidDisplayOrder, 
            string fieldName, 
            string reason)
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateTestProductSku();
            UpdateProductSkuRequest invalidRequest = _request with { DisplayOrder = invalidDisplayOrder };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, 
                because: $"{fieldName} {reason}");
            content.Should().Contain("DisplayOrder must be greater than or equal to zero");
        }

        #endregion

        #region Validation Tests - ProductSkuValidator

        [Fact]
        public async Task Should_ReturnNotFound_WhenProductSkuIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{Constants.NotExistingId}", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenProductIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateTestProductSku();
            UpdateProductSkuRequest invalidRequest = _request with { ProductId = Constants.NotExistingId };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().Contain("Product", 
                because: "error should indicate which Product was not found");
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenProductIsNotPublished()
        {
            // Arrange
            SetAdminAuthentication();
            long unpublishedProductId = await CreateUnpublishedProduct();
            long productSkuId = await CreateTestProductSku();
            
            UpdateProductSkuRequest invalidRequest = _request with 
            { 
                ProductId = unpublishedProductId,
                BarCode = $"780{Guid.NewGuid().ToString()[..10]}"
            };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("not published", 
                because: "error should indicate that the product is not published");
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenBarCodeAlreadyExistsInAnotherSku()
        {
            // Arrange
            SetAdminAuthentication();
            string uniqueBarCode1 = $"780{Guid.NewGuid().ToString()[..10]}";
            string uniqueBarCode2 = $"780{Guid.NewGuid().ToString()[..10]}";
            
            long productSkuId1 = await CreateTestProductSku(barCode: uniqueBarCode1);
            long productSkuId2 = await CreateTestProductSku(barCode: uniqueBarCode2);

            // Act - Try to update productSkuId2 with productSkuId1's BarCode
            UpdateProductSkuRequest invalidRequest = _request with { BarCode = uniqueBarCode1 };
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId2}", invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("BarCode", 
                because: "duplicate BarCode should be clearly indicated");
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenKeepingSameBarCode()
        {
            // Arrange
            SetAdminAuthentication();
            string uniqueBarCode = $"780{Guid.NewGuid().ToString()[..10]}";
            long productSkuId = await CreateTestProductSku(barCode: uniqueBarCode);

            // Act - Update with same BarCode
            UpdateProductSkuRequest request = _request with { BarCode = uniqueBarCode };
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "keeping the same BarCode should be allowed");
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenBarCodeIsNull()
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateTestProductSku();
            
            UpdateProductSkuRequest request = _request with { BarCode = null };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "BarCode is optional and null should be accepted");
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenBarCodeIsEmpty()
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateTestProductSku();
            
            UpdateProductSkuRequest request = _request with { BarCode = "" };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "empty BarCode should be accepted");
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenBarCodeIsWhiteSpace()
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateTestProductSku();
            
            UpdateProductSkuRequest request = _request with { BarCode = "   " };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "whitespace BarCode should be accepted");
        }

        #endregion

        #region Success Tests

        [Fact]
        public async Task Should_ReturnNoContent_WhenRequestIsValid()
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateTestProductSku();

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [MemberData(nameof(GetValidProductSkuUpdateConfigurations))]
        public async Task Should_ReturnNoContent_WhenRequestIsValidWithDifferentConfigurations(
            UpdateProductSkuRequest validRequest,
            string scenario)
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateTestProductSku();
            
            UpdateProductSkuRequest uniqueRequest = validRequest with 
            { 
                BarCode = validRequest.BarCode is not null ? $"780{Guid.NewGuid().ToString()[..10]}" : null
            };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", uniqueRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent, because: scenario);
        }

        public static IEnumerable<object[]> GetValidProductSkuUpdateConfigurations()
        {
            yield return new object[]
            {
                new UpdateProductSkuRequest(
                    ProductId: SMARTPHONE_GALAXY_X_ID,
                    BarCode: "780000000100",
                    Price: 999990m,
                    Cost: 700000m,
                    Weight: 0.200m,
                    Length: 16.0m,
                    Width: 7.5m,
                    Height: 0.9m,
                    DisplayOrder: 4),
                "product sku with all physical dimensions should be updated successfully"
            };

            yield return new object[]
            {
                new UpdateProductSkuRequest(
                    ProductId: IPHONE_PRO_MAX_ID,
                    BarCode: null,
                    Price: 1500000m,
                    Cost: null,
                    Weight: null,
                    Length: null,
                    Width: null,
                    Height: null,
                    DisplayOrder: 3),
                "product sku with minimal fields (null optionals) should be updated successfully"
            };

            yield return new object[]
            {
                new UpdateProductSkuRequest(
                    ProductId: LAPTOP_ULTRABOOK_PRO_ID,
                    BarCode: "780000000300",
                    Price: 1299990m,
                    Cost: 950000m,
                    Weight: 0.250m,
                    Length: 17.0m,
                    Width: 8.0m,
                    Height: 1.0m,
                    DisplayOrder: 2),
                "product sku should be updated successfully with new product"
            };

            yield return new object[]
            {
                new UpdateProductSkuRequest(
                    ProductId: SMARTPHONE_GALAXY_X_ID,
                    BarCode: "780000000400",
                    Price: 999.99m,
                    Cost: 500.50m,
                    Weight: 0.001m,
                    Length: 0.1m,
                    Width: 0.1m,
                    Height: 0.1m,
                    DisplayOrder: 10),
                "product sku with decimal values and high display order should be updated successfully"
            };
        }

        [Theory]
        [InlineData(99.99, "low price")]
        [InlineData(999.99, "medium price")]
        [InlineData(9999.99, "high price")]
        [InlineData(99999.99, "very high price")]
        public async Task Should_ReturnNoContent_WhenUpdatingProductSkuWithDifferentPrices(
            decimal price,
            string scenario)
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateTestProductSku();
            
            UpdateProductSkuRequest request = new(
                ProductId: SMARTPHONE_GALAXY_X_ID,
                BarCode: $"780{Guid.NewGuid().ToString()[..10]}",
                Price: price,
                Cost: null,
                Weight: null,
                Length: null,
                Width: null,
                Height: null,
                DisplayOrder: 5);

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent, because: scenario);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenUpdatingProductSkuWithPhysicalDimensions()
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateTestProductSku();
            
            UpdateProductSkuRequest request = new(
                ProductId: LAPTOP_ULTRABOOK_PRO_ID,
                BarCode: $"780{Guid.NewGuid().ToString()[..10]}",
                Price: 1799990m,
                Cost: 1400000m,
                Weight: 1.350m, // kg
                Length: 35.0m,   // cm
                Width: 25.0m,    // cm
                Height: 2.0m,    // cm
                DisplayOrder: 2);

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenOptionalFieldsAreNull()
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateTestProductSku();
            
            UpdateProductSkuRequest request = new(
                ProductId: IPHONE_PRO_MAX_ID,
                BarCode: null,
                Price: 799990m,
                Cost: null,
                Weight: null,
                Length: null,
                Width: null,
                Height: null,
                DisplayOrder: 3);

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenChangingProductId()
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateTestProductSku();
            
            UpdateProductSkuRequest request = new(
                ProductId: IPHONE_PRO_MAX_ID, // Changing from Smartphone to iPhone
                BarCode: $"780{Guid.NewGuid().ToString()[..10]}",
                Price: 1199990m,
                Cost: 900000m,
                Weight: 0.220m,
                Length: 16.0m,
                Width: 7.8m,
                Height: 0.8m,
                DisplayOrder: 3);

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "changing the ProductId should be allowed");
        }

        [Theory]
        [InlineData(1, "low display order")]
        [InlineData(100, "medium display order")]
        [InlineData(999, "high display order")]
        public async Task Should_ReturnNoContent_WhenUpdatingWithDifferentDisplayOrders(
            int displayOrder,
            string scenario)
        {
            // Arrange
            SetAdminAuthentication();
            long productSkuId = await CreateTestProductSku();
            
            UpdateProductSkuRequest request = new(
                ProductId: SMARTPHONE_GALAXY_X_ID,
                BarCode: $"780{Guid.NewGuid().ToString()[..10]}",
                Price: 899990m,
                Cost: 650000m,
                Weight: 0.185m,
                Length: 15.8m,
                Width: 7.4m,
                Height: 0.8m,
                DisplayOrder: displayOrder);

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent, because: scenario);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenUpdatingSeededProductSku()
        {
            // Arrange
            SetAdminAuthentication();
            
            UpdateProductSkuRequest request = new(
                ProductId: SMARTPHONE_GALAXY_X_ID,
                BarCode: $"780{Guid.NewGuid().ToString()[..10]}",
                Price: 999990m,
                Cost: 750000m,
                Weight: 0.190m,
                Length: 16.0m,
                Width: 7.5m,
                Height: 0.85m,
                DisplayOrder: 1);

            // Act - Updating seeded SKU
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{SMARTPHONE_SKU_1_ID}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        #endregion

        #region Authorization Tests

        [Theory]
        [InlineData(HttpStatusCode.Unauthorized, "no authentication token")]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotAuthenticated(
            HttpStatusCode expectedStatusCode,
            string reason)
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = null;
            long productSkuId = 1; // Using seeded SKU

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", _request);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserIsNotAdmin()
        {
            // Arrange
            SetCustomerUserAuthentication();
            long productSkuId = 1; // Using seeded SKU

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{productSkuId}", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        #endregion

        #region Helper Methods

        private async Task<long> CreateTestProductSku(string? barCode = null)
        {
            CreateProductSkuRequest createRequest = new(
                ProductId: SMARTPHONE_GALAXY_X_ID,
                BarCode: barCode ?? $"780{Guid.NewGuid().ToString()[..10]}",
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

        private async Task<long> CreateUnpublishedProduct()
        {
            CreateProductRequest createRequest = new(
                Name: $"Unpublished Product {Guid.NewGuid()}",
                Description: "Test unpublished product description",
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

        #endregion
    }
}

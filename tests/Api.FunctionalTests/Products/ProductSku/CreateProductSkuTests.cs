using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.Product.Create;
using Web.Api.Endpoints.v1.Products.ProductSku.Create;

namespace Api.FunctionalTests.Products.ProductSku
{
    public class CreateProductSkuTests : BaseFunctionalTest
    {
        #region Seed Data Constants - Published Products with SKUs

        /// <summary>
        /// Product ID 1: Smartphone Galaxy X - Published with 2 SKUs
        /// IsPublished: true, IsActive: true
        /// </summary>
        private const long SMARTPHONE_GALAXY_X_ID = 1;

        /// <summary>
        /// Product ID 2: iPhone Pro Max - Published with 2 SKUs
        /// IsPublished: true, IsActive: true
        /// </summary>
        private const long IPHONE_PRO_MAX_ID = 2;

        /// <summary>
        /// Product ID 3: Laptop Ultrabook Pro - Published with 1 SKU
        /// IsPublished: true, IsActive: true
        /// </summary>
        private const long LAPTOP_ULTRABOOK_PRO_ID = 3;

        #endregion

        private static readonly CreateProductSkuRequest _request = new(
            ProductId: SMARTPHONE_GALAXY_X_ID, // Using seeded published product
            BarCode: "780000000999",
            Price: 899990m,
            Cost: 650000m,
            Weight: 0.185m,
            Length: 15.8m,
            Width: 7.4m,
            Height: 0.8m,
            IsActive: true,
            DisplayOrder: 3);

        public CreateProductSkuTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        #region Validation Tests - FluentValidation

        [Theory]
        [MemberData(nameof(GetInvalidMaxLengthRequests))]
        public async Task Should_ReturnBadRequest_WhenFieldExceedsMaximumLength(
            CreateProductSkuRequest invalidRequest, 
            string fieldName)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, 
                because: $"{fieldName} exceeds maximum length");
        }

        public static IEnumerable<object[]> GetInvalidMaxLengthRequests()
        {
            yield return new object[] 
            { 
                _request with { BarCode = Constants.ExceededMaximumLengthField }, 
                nameof(CreateProductSkuRequest.BarCode) 
            };
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
            CreateProductSkuRequest invalidRequest = _request with { ProductId = invalidProductId };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, invalidRequest);

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
            CreateProductSkuRequest invalidRequest = _request with { Price = invalidPrice };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, invalidRequest);
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
            CreateProductSkuRequest invalidRequest = _request with { Cost = invalidCost };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, invalidRequest);
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
            CreateProductSkuRequest invalidRequest = _request with { Weight = invalidWeight };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, invalidRequest);
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
            CreateProductSkuRequest invalidRequest = _request with { Length = invalidLength };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, invalidRequest);
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
            CreateProductSkuRequest invalidRequest = _request with { Width = invalidWidth };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, invalidRequest);
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
            CreateProductSkuRequest invalidRequest = _request with { Height = invalidHeight };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, invalidRequest);
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
            CreateProductSkuRequest invalidRequest = _request with { DisplayOrder = invalidDisplayOrder };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, 
                because: $"{fieldName} {reason}");
            content.Should().Contain("DisplayOrder must be greater than or equal to zero");
        }

        #endregion

        #region Validation Tests - ProductSkuValidator

        [Fact]
        public async Task Should_ReturnNotFound_WhenProductIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();
            CreateProductSkuRequest invalidRequest = _request with { ProductId = Constants.NotExistingId };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, invalidRequest);
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
            
            // Create an unpublished product (IsPublished = false)
            long unpublishedProductId = await CreateUnpublishedProduct();
            
            CreateProductSkuRequest invalidRequest = _request with 
            { 
                ProductId = unpublishedProductId,
                BarCode = $"780{Guid.NewGuid().ToString()[..10]}"
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("not published", 
                because: "error should indicate that the product is not published");
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenBarCodeAlreadyExists()
        {
            // Arrange
            SetAdminAuthentication();
            string duplicateBarCode = $"780{Guid.NewGuid().ToString()[..10]}";
            
            CreateProductSkuRequest firstRequest = _request with 
            { 
                ProductId = SMARTPHONE_GALAXY_X_ID,
                BarCode = duplicateBarCode 
            };

            // Act - Create product SKU first time
            HttpResponseMessage firstResponse = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, firstRequest);
            firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Act - Try to create product SKU with same BarCode
            CreateProductSkuRequest duplicateRequest = _request with 
            { 
                ProductId= SMARTPHONE_GALAXY_X_ID,
                BarCode = duplicateBarCode 
            };
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, duplicateRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("BarCode", 
                because: "duplicate BarCode should be clearly indicated");
        }

        [Fact]
        public async Task Should_ReturnOk_WhenBarCodeIsNull()
        {
            // Arrange
            SetAdminAuthentication();
            
            CreateProductSkuRequest request = _request with 
            { 
                ProductId = SMARTPHONE_GALAXY_X_ID,
                BarCode = null 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, request);
            long productSkuId = await response.Content.ReadFromJsonAsync<long>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "BarCode is optional and null should be accepted");
            productSkuId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Should_ReturnOk_WhenBarCodeIsEmpty()
        {
            // Arrange
            SetAdminAuthentication();
            
            CreateProductSkuRequest request = _request with 
            { 
                ProductId = SMARTPHONE_GALAXY_X_ID,
                BarCode = "" 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, request);
            long productSkuId = await response.Content.ReadFromJsonAsync<long>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "empty BarCode should be accepted");
            productSkuId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Should_ReturnOk_WhenBarCodeIsWhiteSpace()
        {
            // Arrange
            SetAdminAuthentication();
            
            CreateProductSkuRequest request = _request with 
            { 
                ProductId = SMARTPHONE_GALAXY_X_ID,
                BarCode = "   " 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, request);
            long productSkuId = await response.Content.ReadFromJsonAsync<long>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "whitespace BarCode should be accepted");
            productSkuId.Should().BeGreaterThan(0);
        }

        #endregion

        #region Success Tests

        [Fact]
        public async Task Should_ReturnOk_WhenRequestIsValidWithAllFields()
        {
            // Arrange
            SetAdminAuthentication();
            
            CreateProductSkuRequest request = _request with 
            { 
                ProductId = SMARTPHONE_GALAXY_X_ID,
                BarCode = $"780{Guid.NewGuid().ToString()[..10]}"
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, request);
            long productSkuId = await response.Content.ReadFromJsonAsync<long>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            productSkuId.Should().BeGreaterThan(0, 
                because: "a valid product SKU ID should be returned");
        }

        [Theory]
        [MemberData(nameof(GetValidProductSkuConfigurations))]
        public async Task Should_ReturnOk_WhenRequestIsValidWithDifferentConfigurations(
            CreateProductSkuRequest validRequest,
            string scenario)
        {
            // Arrange
            SetAdminAuthentication();
            
            CreateProductSkuRequest uniqueRequest = validRequest with 
            { 
                BarCode = validRequest.BarCode is not null ? $"780{Guid.NewGuid().ToString()[..10]}" : null
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, uniqueRequest);
            long productSkuId = await response.Content.ReadFromJsonAsync<long>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK, because: scenario);
            productSkuId.Should().BeGreaterThan(0);
        }

        public static IEnumerable<object[]> GetValidProductSkuConfigurations()
        {
            yield return new object[]
            {
                new CreateProductSkuRequest(
                    ProductId: SMARTPHONE_GALAXY_X_ID,
                    BarCode: "780000000100",
                    Price: 999990m,
                    Cost: 700000m,
                    Weight: 0.200m,
                    Length: 16.0m,
                    Width: 7.5m,
                    Height: 0.9m,
                    IsActive: true,
                    DisplayOrder: 4),
                "product sku with all physical dimensions should be created successfully"
            };

            yield return new object[]
            {
                new CreateProductSkuRequest(
                    ProductId: IPHONE_PRO_MAX_ID,
                    BarCode: null,
                    Price: 1500000m,
                    Cost: null,
                    Weight: null,
                    Length: null,
                    Width: null,
                    Height: null,
                    IsActive: true,
                    DisplayOrder: 3),
                "product sku with minimal fields (null optionals) should be created successfully"
            };

            yield return new object[]
            {
                new CreateProductSkuRequest(
                    ProductId: LAPTOP_ULTRABOOK_PRO_ID,
                    BarCode: "780000000300",
                    Price: 1299990m,
                    Cost: 950000m,
                    Weight: 0.250m,
                    Length: 17.0m,
                    Width: 8.0m,
                    Height: 1.0m,
                    IsActive: false,
                    DisplayOrder: 2),
                "inactive product sku should be created successfully"
            };

            yield return new object[]
            {
                new CreateProductSkuRequest(
                    ProductId: SMARTPHONE_GALAXY_X_ID,
                    BarCode: "780000000400",
                    Price: 999.99m,
                    Cost: 500.50m,
                    Weight: 0.001m,
                    Length: 0.1m,
                    Width: 0.1m,
                    Height: 0.1m,
                    IsActive: true,
                    DisplayOrder: 10),
                "product sku with decimal values and high display order should be created successfully"
            };
        }

        [Theory]
        [InlineData(99.99, "low price")]
        [InlineData(999.99, "medium price")]
        [InlineData(9999.99, "high price")]
        [InlineData(99999.99, "very high price")]
        public async Task Should_ReturnOk_WhenCreatingProductSkuWithDifferentPrices(
            decimal price,
            string scenario)
        {
            // Arrange
            SetAdminAuthentication();
            
            CreateProductSkuRequest request = new(
                ProductId: SMARTPHONE_GALAXY_X_ID,
                BarCode: $"780{Guid.NewGuid().ToString()[..10]}",
                Price: price,
                Cost: null,
                Weight: null,
                Length: null,
                Width: null,
                Height: null,
                IsActive: true,
                DisplayOrder: 5);

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, request);
            long productSkuId = await response.Content.ReadFromJsonAsync<long>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK, because: scenario);
            productSkuId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Should_ReturnOk_WhenCreatingProductSkuWithPhysicalDimensions()
        {
            // Arrange
            SetAdminAuthentication();
            
            CreateProductSkuRequest request = new(
                ProductId: LAPTOP_ULTRABOOK_PRO_ID,
                BarCode: $"780{Guid.NewGuid().ToString()[..10]}",
                Price: 1799990m,
                Cost: 1400000m,
                Weight: 1.350m, // kg
                Length: 35.0m,   // cm
                Width: 25.0m,    // cm
                Height: 2.0m,    // cm
                IsActive: true,
                DisplayOrder: 2);

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, request);
            long productSkuId = await response.Content.ReadFromJsonAsync<long>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            productSkuId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Should_ReturnOk_WhenOptionalFieldsAreNull()
        {
            // Arrange
            SetAdminAuthentication();
            
            CreateProductSkuRequest request = new(
                ProductId: IPHONE_PRO_MAX_ID,
                BarCode: null,
                Price: 799990m,
                Cost: null,
                Weight: null,
                Length: null,
                Width: null,
                Height: null,
                IsActive: true,
                DisplayOrder: 3);

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, request);
            long productSkuId = await response.Content.ReadFromJsonAsync<long>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            productSkuId.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData(SMARTPHONE_GALAXY_X_ID, 4, "Smartphone Galaxy X")]
        [InlineData(IPHONE_PRO_MAX_ID, 3, "iPhone Pro Max")]
        [InlineData(LAPTOP_ULTRABOOK_PRO_ID, 2, "Laptop Ultrabook Pro")]
        public async Task Should_ReturnOk_WhenCreatingSkusForDifferentSeededProducts(
            long productId,
            int displayOrder,
            string productName)
        {
            // Arrange
            SetAdminAuthentication();
            
            CreateProductSkuRequest request = new(
                ProductId: productId,
                BarCode: $"780{Guid.NewGuid().ToString()[..10]}",
                Price: 899990m,
                Cost: 650000m,
                Weight: 0.185m,
                Length: 15.8m,
                Width: 7.4m,
                Height: 0.8m,
                IsActive: true,
                DisplayOrder: displayOrder);

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, request);
            long productSkuId = await response.Content.ReadFromJsonAsync<long>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK, because: $"creating SKU for {productName}");
            productSkuId.Should().BeGreaterThan(0);
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
            CreateProductSkuRequest request = _request with 
            { 
                BarCode = $"780{Guid.NewGuid().ToString()[..10]}" 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, request);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserIsNotAdmin()
        {
            // Arrange
            SetCustomerUserAuthentication();
            CreateProductSkuRequest request = _request with 
            { 
                BarCode = $"780{Guid.NewGuid().ToString()[..10]}" 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.ProductSkus.Base, request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        #endregion

        #region Helper Methods

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

using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.ProductAttributeValue.Assign;
using Web.Api.Endpoints.v1.Products.ProductSku.Create;

namespace Api.FunctionalTests.Products.ProductAttributeValue
{
    public class AssignAttributeValueToSkuTests : BaseFunctionalTest
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

        #region Seed Data Constants - AttributeValues

        /// <summary>
        /// AttributeValue ID 1: Negro - Color attribute (Active)
        /// </summary>
        private const long NEGRO_ATTRIBUTE_VALUE_ID = 1;

        /// <summary>
        /// AttributeValue ID 2: Blanco - Color attribute (Active)
        /// </summary>
        private const long BLANCO_ATTRIBUTE_VALUE_ID = 2;

        /// <summary>
        /// AttributeValue ID 3: Azul - Color attribute (Active)
        /// </summary>
        private const long AZUL_ATTRIBUTE_VALUE_ID = 3;

        /// <summary>
        /// AttributeValue ID 4: 128 GB - Storage attribute (Active)
        /// </summary>
        private const long STORAGE_128GB_ATTRIBUTE_VALUE_ID = 4;

        /// <summary>
        /// AttributeValue ID 5: 256 GB - Storage attribute (Active)
        /// </summary>
        private const long STORAGE_256GB_ATTRIBUTE_VALUE_ID = 5;

        /// <summary>
        /// AttributeValue ID 6: 512 GB - Storage attribute (Active)
        /// </summary>
        private const long STORAGE_512GB_ATTRIBUTE_VALUE_ID = 6;

        /// <summary>
        /// AttributeValue ID 7: 8 GB - RAM attribute (Active)
        /// </summary>
        private const long RAM_8GB_ATTRIBUTE_VALUE_ID = 7;

        /// <summary>
        /// AttributeValue ID 10: S - Size attribute (Active)
        /// </summary>
        private const long SIZE_S_ATTRIBUTE_VALUE_ID = 10;

        /// <summary>
        /// AttributeValue ID 11: M - Size attribute (Active)
        /// </summary>
        private const long SIZE_M_ATTRIBUTE_VALUE_ID = 11;

        /// <summary>
        /// AttributeValue ID 13: Aluminio - Material attribute (Active)
        /// </summary>
        private const long MATERIAL_ALUMINIO_ATTRIBUTE_VALUE_ID = 13;

        #endregion

        private static readonly AssignRequest _request = new(
            AttributeValueId: NEGRO_ATTRIBUTE_VALUE_ID);

        public AssignAttributeValueToSkuTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        #region Validation Tests - FluentValidation

        [Theory]
        [InlineData(0, "SkuId", "is missing")]
        [InlineData(-1, "SkuId", "is negative")]
        public async Task Should_ReturnBadRequest_WhenSkuIdIsInvalid(
            long invalidSkuId,
            string fieldName,
            string reason)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{invalidSkuId}/attributes", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                because: $"{fieldName} {reason}");
        }

        [Theory]
        [InlineData(0, "AttributeValueId", "is missing")]
        [InlineData(-1, "AttributeValueId", "is negative")]
        public async Task Should_ReturnBadRequest_WhenAttributeValueIdIsInvalid(
            long invalidAttributeValueId,
            string fieldName,
            string reason)
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            AssignRequest invalidRequest = _request with { AttributeValueId = invalidAttributeValueId };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                because: $"{fieldName} {reason}");
        }

        #endregion

        #region Validation Tests - ProductAttributeValueValidator

        [Fact]
        public async Task Should_ReturnNotFound_WhenSkuIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{Constants.NotExistingId}/attributes", _request);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().Contain("SKU",
                because: "error should indicate which SKU was not found");
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenAttributeValueIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            AssignRequest invalidRequest = _request with { AttributeValueId = Constants.NotExistingId };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes", invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().Contain("Attribute",
                because: "error should indicate which AttributeValue was not found");
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAttributeValueIsInactive()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            long inactiveAttributeValueId = await CreateInactiveAttributeValue();

            AssignRequest invalidRequest = _request with { AttributeValueId = inactiveAttributeValueId };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes", invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("not active",
                because: "error should indicate that the attribute value is inactive");
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenSkuIsInactive()
        {
            // Arrange
            SetAdminAuthentication();
            long inactiveSkuId = await CreateInactiveProductSku();

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{inactiveSkuId}/attributes", _request);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("not active",
                because: "error should indicate that the SKU is inactive");
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAttributeValueIsAlreadyAssignedToSku()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();

            // Act - First assignment (should succeed)
            AssignRequest firstRequest = _request with { AttributeValueId = NEGRO_ATTRIBUTE_VALUE_ID };
            HttpResponseMessage firstResponse = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes", firstRequest);
            firstResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Act - Second assignment with same AttributeValue (should fail)
            AssignRequest duplicateRequest = _request with { AttributeValueId = NEGRO_ATTRIBUTE_VALUE_ID };
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes", duplicateRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("already assigned",
                because: "duplicate assignment should be clearly indicated");
        }

        #endregion

        #region Success Tests

        [Fact]
        public async Task Should_ReturnNoContent_WhenRequestIsValid()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_AssignColorAttributeValue_ToProductSku()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();

            AssignRequest request = new(AttributeValueId: NEGRO_ATTRIBUTE_VALUE_ID);

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_AssignStorageAttributeValue_ToProductSku()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();

            AssignRequest request = new(AttributeValueId: STORAGE_128GB_ATTRIBUTE_VALUE_ID);

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_AssignRamAttributeValue_ToProductSku()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();

            AssignRequest request = new(AttributeValueId: RAM_8GB_ATTRIBUTE_VALUE_ID);

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_AssignMultipleAttributeValues_ToSameSku()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();

            // Act - Assign Color
            AssignRequest colorRequest = new(AttributeValueId: NEGRO_ATTRIBUTE_VALUE_ID);
            HttpResponseMessage colorResponse = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes", colorRequest);
            colorResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Act - Assign Storage
            AssignRequest storageRequest = new(AttributeValueId: STORAGE_256GB_ATTRIBUTE_VALUE_ID);
            HttpResponseMessage storageResponse = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes", storageRequest);
            storageResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Act - Assign RAM
            AssignRequest ramRequest = new(AttributeValueId: RAM_8GB_ATTRIBUTE_VALUE_ID);
            HttpResponseMessage ramResponse = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes", ramRequest);

            // Assert
            ramResponse.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "multiple different attribute values can be assigned to the same SKU");
        }

        [Theory]
        [InlineData(NEGRO_ATTRIBUTE_VALUE_ID, "Color: Negro")]
        [InlineData(BLANCO_ATTRIBUTE_VALUE_ID, "Color: Blanco")]
        [InlineData(AZUL_ATTRIBUTE_VALUE_ID, "Color: Azul")]
        [InlineData(STORAGE_128GB_ATTRIBUTE_VALUE_ID, "Storage: 128 GB")]
        [InlineData(STORAGE_256GB_ATTRIBUTE_VALUE_ID, "Storage: 256 GB")]
        [InlineData(RAM_8GB_ATTRIBUTE_VALUE_ID, "RAM: 8 GB")]
        [InlineData(SIZE_S_ATTRIBUTE_VALUE_ID, "Size: S")]
        [InlineData(SIZE_M_ATTRIBUTE_VALUE_ID, "Size: M")]
        [InlineData(MATERIAL_ALUMINIO_ATTRIBUTE_VALUE_ID, "Material: Aluminio")]
        public async Task Should_AssignDifferentAttributeValues_FromSeedData(
            long attributeValueId,
            string description)
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();

            AssignRequest request = new(AttributeValueId: attributeValueId);

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent, because: $"assigning {description}");
        }

        [Fact]
        public async Task Should_AssignAttributeValue_ToSkuFromSmartphoneProduct()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku(SMARTPHONE_GALAXY_X_ID);

            AssignRequest request = new(AttributeValueId: NEGRO_ATTRIBUTE_VALUE_ID);

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_AssignAttributeValue_ToSkuFromIPhoneProduct()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku(IPHONE_PRO_MAX_ID);

            AssignRequest request = new(AttributeValueId: BLANCO_ATTRIBUTE_VALUE_ID);

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_AssignAttributeValue_ToSkuFromLaptopProduct()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku(LAPTOP_ULTRABOOK_PRO_ID);

            AssignRequest request = new(AttributeValueId: STORAGE_512GB_ATTRIBUTE_VALUE_ID);

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_AssignSameAttributeValue_ToDifferentSkus()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId1 = await CreateActiveProductSku(SMARTPHONE_GALAXY_X_ID);
            long skuId2 = await CreateActiveProductSku(IPHONE_PRO_MAX_ID);

            AssignRequest request = new(AttributeValueId: NEGRO_ATTRIBUTE_VALUE_ID);

            // Act - Assign to first SKU
            HttpResponseMessage response1 = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{skuId1}/attributes", request);
            response1.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Act - Assign same AttributeValue to second SKU
            HttpResponseMessage response2 = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{skuId2}/attributes", request);

            // Assert
            response2.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "same attribute value can be assigned to different SKUs");
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

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/1/attributes", _request);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserIsNotAdmin()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/1/attributes", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        #endregion

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

        private async Task<long> CreateInactiveAttributeValue()
        {
            Web.Api.Endpoints.v1.Products.AttributeValue.Create.CreateAttributeValueRequest createRequest = new(
                AttributeId: 1, // Color attribute
                Value: $"Inactive Color {Guid.NewGuid()}",
                NumericValue: null,
                BooleanValue: null,
                DisplayOrder: 100,
                IsActive: false);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long? attributeValueId = await response.Content.ReadFromJsonAsync<long?>();
            return attributeValueId!.Value;
        }

        #endregion
    }
}

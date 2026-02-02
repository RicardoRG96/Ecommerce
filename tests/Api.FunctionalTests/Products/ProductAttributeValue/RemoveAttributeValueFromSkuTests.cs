using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.ProductAttributeValue.Assign;
using Web.Api.Endpoints.v1.Products.ProductSku.Create;

namespace Api.FunctionalTests.Products.ProductAttributeValue
{
    public class RemoveAttributeValueFromSkuTests : BaseFunctionalTest
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

        public RemoveAttributeValueFromSkuTests(FunctionalTestWebAppFactory factory) 
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
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{invalidSkuId}/attributes/{NEGRO_ATTRIBUTE_VALUE_ID}");

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

            // Act
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes/{invalidAttributeValueId}");

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
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{Constants.NotExistingId}/attributes/{NEGRO_ATTRIBUTE_VALUE_ID}");
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
            long skuId = await CreateActiveProductSkuWithAssignedAttribute();

            // Act
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes/{Constants.NotExistingId}");
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().Contain("Attribute",
                because: "error should indicate which AttributeValue was not found");
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAttributeValueIsNotAssignedToSku()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku(); // SKU without any assigned attributes

            // Act
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes/{NEGRO_ATTRIBUTE_VALUE_ID}");
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("not assigned",
                because: "error should indicate that the attribute value is not assigned to the SKU");
        }

        #endregion

        #region Success Tests

        [Fact]
        public async Task Should_ReturnNoContent_WhenRequestIsValid()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSkuWithAssignedAttribute();

            // Act
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes/{NEGRO_ATTRIBUTE_VALUE_ID}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_RemoveColorAttributeValue_FromProductSku()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            await AssignAttributeValueToSku(skuId, NEGRO_ATTRIBUTE_VALUE_ID);

            // Act
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes/{NEGRO_ATTRIBUTE_VALUE_ID}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_RemoveStorageAttributeValue_FromProductSku()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            await AssignAttributeValueToSku(skuId, STORAGE_128GB_ATTRIBUTE_VALUE_ID);

            // Act
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes/{STORAGE_128GB_ATTRIBUTE_VALUE_ID}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_RemoveRamAttributeValue_FromProductSku()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            await AssignAttributeValueToSku(skuId, RAM_8GB_ATTRIBUTE_VALUE_ID);

            // Act
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes/{RAM_8GB_ATTRIBUTE_VALUE_ID}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_RemoveOneAttributeValue_WhileKeepingOthers()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            
            // Assign multiple attributes
            await AssignAttributeValueToSku(skuId, NEGRO_ATTRIBUTE_VALUE_ID);
            await AssignAttributeValueToSku(skuId, STORAGE_256GB_ATTRIBUTE_VALUE_ID);
            await AssignAttributeValueToSku(skuId, RAM_8GB_ATTRIBUTE_VALUE_ID);

            // Act - Remove only one attribute
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes/{NEGRO_ATTRIBUTE_VALUE_ID}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "removing one attribute should succeed while keeping others");
        }

        [Fact]
        public async Task Should_RemoveAllAttributeValues_FromSku_Sequentially()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            
            // Assign multiple attributes
            await AssignAttributeValueToSku(skuId, NEGRO_ATTRIBUTE_VALUE_ID);
            await AssignAttributeValueToSku(skuId, STORAGE_256GB_ATTRIBUTE_VALUE_ID);
            await AssignAttributeValueToSku(skuId, RAM_8GB_ATTRIBUTE_VALUE_ID);

            // Act - Remove all attributes one by one
            HttpResponseMessage response1 = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes/{NEGRO_ATTRIBUTE_VALUE_ID}");
            response1.StatusCode.Should().Be(HttpStatusCode.NoContent);

            HttpResponseMessage response2 = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes/{STORAGE_256GB_ATTRIBUTE_VALUE_ID}");
            response2.StatusCode.Should().Be(HttpStatusCode.NoContent);

            HttpResponseMessage response3 = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes/{RAM_8GB_ATTRIBUTE_VALUE_ID}");

            // Assert
            response3.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "all attributes can be removed from SKU");
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
        public async Task Should_RemoveDifferentAttributeValues_FromSeedData(
            long attributeValueId,
            string description)
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            await AssignAttributeValueToSku(skuId, attributeValueId);

            // Act
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes/{attributeValueId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent, because: $"removing {description}");
        }

        [Fact]
        public async Task Should_RemoveAttributeValue_FromSkuOfSmartphoneProduct()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku(SMARTPHONE_GALAXY_X_ID);
            await AssignAttributeValueToSku(skuId, NEGRO_ATTRIBUTE_VALUE_ID);

            // Act
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes/{NEGRO_ATTRIBUTE_VALUE_ID}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_RemoveAttributeValue_FromSkuOfIPhoneProduct()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku(IPHONE_PRO_MAX_ID);
            await AssignAttributeValueToSku(skuId, BLANCO_ATTRIBUTE_VALUE_ID);

            // Act
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes/{BLANCO_ATTRIBUTE_VALUE_ID}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_RemoveAttributeValue_FromSkuOfLaptopProduct()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku(LAPTOP_ULTRABOOK_PRO_ID);
            await AssignAttributeValueToSku(skuId, STORAGE_512GB_ATTRIBUTE_VALUE_ID);

            // Act
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes/{STORAGE_512GB_ATTRIBUTE_VALUE_ID}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_AllowReassigningAttributeValue_AfterRemoval()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            
            // Assign attribute
            await AssignAttributeValueToSku(skuId, NEGRO_ATTRIBUTE_VALUE_ID);

            // Act - Remove attribute
            HttpResponseMessage removeResponse = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes/{NEGRO_ATTRIBUTE_VALUE_ID}");
            removeResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Act - Reassign the same attribute
            HttpResponseMessage reassignResponse = await AssignAttributeValueToSku(skuId, NEGRO_ATTRIBUTE_VALUE_ID);

            // Assert
            reassignResponse.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "attribute can be reassigned after removal");
        }

        [Fact]
        public async Task Should_RemoveSameAttributeValue_FromDifferentSkus()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId1 = await CreateActiveProductSku(SMARTPHONE_GALAXY_X_ID);
            long skuId2 = await CreateActiveProductSku(IPHONE_PRO_MAX_ID);
            
            await AssignAttributeValueToSku(skuId1, NEGRO_ATTRIBUTE_VALUE_ID);
            await AssignAttributeValueToSku(skuId2, NEGRO_ATTRIBUTE_VALUE_ID);

            // Act - Remove from first SKU
            HttpResponseMessage response1 = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId1}/attributes/{NEGRO_ATTRIBUTE_VALUE_ID}");
            response1.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Act - Remove from second SKU
            HttpResponseMessage response2 = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId2}/attributes/{NEGRO_ATTRIBUTE_VALUE_ID}");

            // Assert
            response2.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "same attribute value can be removed from different SKUs independently");
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenTryingToRemoveAlreadyRemovedAttribute()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            await AssignAttributeValueToSku(skuId, NEGRO_ATTRIBUTE_VALUE_ID);

            // Act - First removal (should succeed)
            HttpResponseMessage firstRemoval = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes/{NEGRO_ATTRIBUTE_VALUE_ID}");
            firstRemoval.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Act - Second removal (should fail)
            HttpResponseMessage secondRemoval = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes/{NEGRO_ATTRIBUTE_VALUE_ID}");
            string content = await secondRemoval.Content.ReadAsStringAsync();

            // Assert
            secondRemoval.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("not assigned",
                because: "cannot remove an attribute that is not assigned");
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
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/1/attributes/{NEGRO_ATTRIBUTE_VALUE_ID}");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserIsNotAdmin()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/1/attributes/{NEGRO_ATTRIBUTE_VALUE_ID}");

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

        private async Task<long> CreateActiveProductSkuWithAssignedAttribute(long? productId = null, long? attributeValueId = null)
        {
            long skuId = await CreateActiveProductSku(productId);
            await AssignAttributeValueToSku(skuId, attributeValueId ?? NEGRO_ATTRIBUTE_VALUE_ID);
            return skuId;
        }

        private async Task<HttpResponseMessage> AssignAttributeValueToSku(long skuId, long attributeValueId)
        {
            AssignRequest assignRequest = new(AttributeValueId: attributeValueId);
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes", assignRequest);
            response.EnsureSuccessStatusCode();
            return response;
        }

        #endregion
    }
}

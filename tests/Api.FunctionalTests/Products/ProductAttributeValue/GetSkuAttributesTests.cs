using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Products.ProductAttributeValues.Common.Mappers;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.ProductAttributeValue.Assign;
using Web.Api.Endpoints.v1.Products.ProductSku.Create;

namespace Api.FunctionalTests.Products.ProductAttributeValue
{
    public class GetSkuAttributesTests : BaseFunctionalTest
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

        #region Seed Data Constants - Attributes

        /// <summary>
        /// Attribute ID 1: Color - String type, variant, required
        /// Code: "color"
        /// </summary>
        private const long COLOR_ATTRIBUTE_ID = 1;

        /// <summary>
        /// Attribute ID 2: Storage - Number type, variant, required
        /// Code: "storage"
        /// </summary>
        private const long STORAGE_ATTRIBUTE_ID = 2;

        /// <summary>
        /// Attribute ID 3: RAM - Number type, variant, required
        /// Code: "ram"
        /// </summary>
        private const long RAM_ATTRIBUTE_ID = 3;

        /// <summary>
        /// Attribute ID 4: Size - String type, variant, required
        /// Code: "size"
        /// </summary>
        private const long SIZE_ATTRIBUTE_ID = 4;

        /// <summary>
        /// Attribute ID 5: Material - String type, non-variant, optional
        /// Code: "material"
        /// </summary>
        private const long MATERIAL_ATTRIBUTE_ID = 5;

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
        /// AttributeValue ID 8: 12 GB - RAM attribute (Active)
        /// </summary>
        private const long RAM_12GB_ATTRIBUTE_VALUE_ID = 8;

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

        /// <summary>
        /// AttributeValue ID 14: Plástico - Material attribute (Active)
        /// </summary>
        private const long MATERIAL_PLASTICO_ATTRIBUTE_VALUE_ID = 14;

        #endregion

        public GetSkuAttributesTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnOk_WhenSkuHasAttributes()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSkuWithAssignedAttributes();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_ReturnListOfProductAttributeValues_WhenSkuExists()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSkuWithAssignedAttributes();

            // Act
            List<ProductAttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<ProductAttributeValueResponse>>($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes");

            // Assert
            attributeValues.Should().NotBeNull();
            attributeValues.Should().NotBeEmpty();
            attributeValues.Should().HaveCountGreaterThanOrEqualTo(1,
                because: "SKU has at least 1 attribute assigned");
        }

        [Fact]
        public async Task Should_ReturnEmptyList_WhenSkuHasNoAttributes()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku(); // SKU without attributes

            // Act
            List<ProductAttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<ProductAttributeValueResponse>>($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes");

            // Assert
            attributeValues.Should().NotBeNull();
            attributeValues.Should().BeEmpty(
                because: "SKU was created without any assigned attributes");
        }

        [Fact]
        public async Task Should_ReturnAttributeValuesWithAllProperties()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            await AssignAttributeValueToSku(skuId, NEGRO_ATTRIBUTE_VALUE_ID);

            // Act
            List<ProductAttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<ProductAttributeValueResponse>>($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes");

            // Assert
            attributeValues.Should().NotBeNull();
            attributeValues.Should().NotBeEmpty();

            ProductAttributeValueResponse? colorAttribute = attributeValues!.FirstOrDefault();
            colorAttribute.Should().NotBeNull();
            colorAttribute!.AttributeCode.Should().Be("color");
            colorAttribute.Value.Should().Be("Negro");
            colorAttribute.Name.Should().Be("Color");
            colorAttribute.DataType.Should().Be("String");
            colorAttribute.IsVariant.Should().BeTrue();
            colorAttribute.ProductSku.Should().NotBeNull();
            colorAttribute.ProductSku!.Id.Should().Be(skuId);
        }

        [Fact]
        public async Task Should_ReturnMultipleAttributeValues_ForSameSku()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            
            // Assign multiple attributes
            await AssignAttributeValueToSku(skuId, NEGRO_ATTRIBUTE_VALUE_ID);
            await AssignAttributeValueToSku(skuId, STORAGE_256GB_ATTRIBUTE_VALUE_ID);
            await AssignAttributeValueToSku(skuId, RAM_8GB_ATTRIBUTE_VALUE_ID);

            // Act
            List<ProductAttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<ProductAttributeValueResponse>>($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes");

            // Assert
            attributeValues.Should().NotBeNull();
            attributeValues.Should().HaveCount(3,
                because: "3 attributes were assigned to the SKU");
            
            attributeValues.Should().Contain(a => a.AttributeCode == "color" && a.Value == "Negro");
            attributeValues.Should().Contain(a => a.AttributeCode == "storage" && a.Value == "256 GB");
            attributeValues.Should().Contain(a => a.AttributeCode == "ram" && a.Value == "8 GB");
        }

        [Fact]
        public async Task Should_ReturnColorAttributeValue_WithCorrectProperties()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            await AssignAttributeValueToSku(skuId, NEGRO_ATTRIBUTE_VALUE_ID);

            // Act
            List<ProductAttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<ProductAttributeValueResponse>>($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes");

            // Assert
            attributeValues.Should().NotBeNull();
            ProductAttributeValueResponse? colorAttribute = attributeValues!.FirstOrDefault(a => a.AttributeCode == "color");
            
            colorAttribute.Should().NotBeNull();
            colorAttribute!.AttributeCode.Should().Be("color");
            colorAttribute.Value.Should().Be("Negro");
            colorAttribute.Name.Should().Be("Color");
            colorAttribute.DataType.Should().Be("String");
            colorAttribute.IsVariant.Should().BeTrue();
        }

        [Fact]
        public async Task Should_ReturnStorageAttributeValue_WithNumericType()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            await AssignAttributeValueToSku(skuId, STORAGE_256GB_ATTRIBUTE_VALUE_ID);

            // Act
            List<ProductAttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<ProductAttributeValueResponse>>($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes");

            // Assert
            attributeValues.Should().NotBeNull();
            ProductAttributeValueResponse? storageAttribute = attributeValues!.FirstOrDefault(a => a.AttributeCode == "storage");
            
            storageAttribute.Should().NotBeNull();
            storageAttribute!.AttributeCode.Should().Be("storage");
            storageAttribute.Value.Should().Be("256 GB");
            storageAttribute.Name.Should().Be("Almacenamiento");
            storageAttribute.DataType.Should().Be("Number");
            storageAttribute.IsVariant.Should().BeTrue();
        }

        [Fact]
        public async Task Should_ReturnRamAttributeValue_WithNumericType()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            await AssignAttributeValueToSku(skuId, RAM_12GB_ATTRIBUTE_VALUE_ID);

            // Act
            List<ProductAttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<ProductAttributeValueResponse>>($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes");

            // Assert
            attributeValues.Should().NotBeNull();
            ProductAttributeValueResponse? ramAttribute = attributeValues!.FirstOrDefault(a => a.AttributeCode == "ram");
            
            ramAttribute.Should().NotBeNull();
            ramAttribute!.AttributeCode.Should().Be("ram");
            ramAttribute.Value.Should().Be("12 GB");
            ramAttribute.Name.Should().Be("Memoria RAM");
            ramAttribute.DataType.Should().Be("Number");
            ramAttribute.IsVariant.Should().BeTrue();
        }

        [Fact]
        public async Task Should_ReturnMaterialAttributeValue_AsNonVariant()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            await AssignAttributeValueToSku(skuId, MATERIAL_ALUMINIO_ATTRIBUTE_VALUE_ID);

            // Act
            List<ProductAttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<ProductAttributeValueResponse>>($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes");

            // Assert
            attributeValues.Should().NotBeNull();
            ProductAttributeValueResponse? materialAttribute = attributeValues!.FirstOrDefault(a => a.AttributeCode == "material");
            
            materialAttribute.Should().NotBeNull();
            materialAttribute!.AttributeCode.Should().Be("material");
            materialAttribute.Value.Should().Be("Aluminio");
            materialAttribute.Name.Should().Be("Material");
            materialAttribute.DataType.Should().Be("String");
            materialAttribute.IsVariant.Should().BeFalse(
                because: "Material is not a variant attribute");
        }

        [Fact]
        public async Task Should_ReturnAttributesWithProductSkuInformation()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku(SMARTPHONE_GALAXY_X_ID);
            await AssignAttributeValueToSku(skuId, NEGRO_ATTRIBUTE_VALUE_ID);

            // Act
            List<ProductAttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<ProductAttributeValueResponse>>($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes");

            // Assert
            attributeValues.Should().NotBeNull();
            attributeValues.Should().AllSatisfy(attr =>
            {
                attr.ProductSku.Should().NotBeNull();
                attr.ProductSku!.Id.Should().Be(skuId);
                attr.ProductSku.IsActive.Should().BeTrue();
                attr.ProductSku.Product.Should().NotBeNull();
                attr.ProductSku.Product!.Id.Should().Be(SMARTPHONE_GALAXY_X_ID);
                attr.ProductSku.Product.Name.Should().Be("Smartphone Galaxy X");
            });
        }

        [Theory]
        [InlineData(NEGRO_ATTRIBUTE_VALUE_ID, "color", "Negro", "String", true)]
        [InlineData(BLANCO_ATTRIBUTE_VALUE_ID, "color", "Blanco", "String", true)]
        [InlineData(AZUL_ATTRIBUTE_VALUE_ID, "color", "Azul", "String", true)]
        [InlineData(STORAGE_128GB_ATTRIBUTE_VALUE_ID, "storage", "128 GB", "Number", true)]
        [InlineData(RAM_8GB_ATTRIBUTE_VALUE_ID, "ram", "8 GB", "Number", true)]
        [InlineData(SIZE_S_ATTRIBUTE_VALUE_ID, "size", "S", "String", true)]
        [InlineData(SIZE_M_ATTRIBUTE_VALUE_ID, "size", "M", "String", true)]
        [InlineData(MATERIAL_ALUMINIO_ATTRIBUTE_VALUE_ID, "material", "Aluminio", "String", false)]
        [InlineData(MATERIAL_PLASTICO_ATTRIBUTE_VALUE_ID, "material", "Plástico", "String", false)]
        public async Task Should_ReturnDifferentAttributeValues_WithCorrectProperties(
            long attributeValueId,
            string expectedAttributeCode,
            string expectedValue,
            string expectedDataType,
            bool expectedIsVariant)
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            await AssignAttributeValueToSku(skuId, attributeValueId);

            // Act
            List<ProductAttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<ProductAttributeValueResponse>>($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes");

            // Assert
            attributeValues.Should().NotBeNull();
            ProductAttributeValueResponse? attribute = attributeValues!.FirstOrDefault();
            
            attribute.Should().NotBeNull();
            attribute!.AttributeCode.Should().Be(expectedAttributeCode);
            attribute.Value.Should().Be(expectedValue);
            attribute.DataType.Should().Be(expectedDataType);
            attribute.IsVariant.Should().Be(expectedIsVariant);
        }

        [Fact]
        public async Task Should_ReturnUpdatedList_AfterAssigningNewAttribute()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            await AssignAttributeValueToSku(skuId, NEGRO_ATTRIBUTE_VALUE_ID);

            // Act - Get initial attributes
            List<ProductAttributeValueResponse>? initialAttributes = 
                await HttpClient.GetFromJsonAsync<List<ProductAttributeValueResponse>>($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes");
            initialAttributes.Should().HaveCount(1);

            // Act - Assign new attribute
            await AssignAttributeValueToSku(skuId, STORAGE_256GB_ATTRIBUTE_VALUE_ID);

            // Act - Get updated attributes
            List<ProductAttributeValueResponse>? updatedAttributes = 
                await HttpClient.GetFromJsonAsync<List<ProductAttributeValueResponse>>($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes");

            // Assert
            updatedAttributes.Should().HaveCount(2,
                because: "a new attribute was assigned");
        }

        [Fact]
        public async Task Should_ReturnUpdatedList_AfterRemovingAttribute()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            await AssignAttributeValueToSku(skuId, NEGRO_ATTRIBUTE_VALUE_ID);
            await AssignAttributeValueToSku(skuId, STORAGE_256GB_ATTRIBUTE_VALUE_ID);

            // Act - Get initial attributes
            List<ProductAttributeValueResponse>? initialAttributes = 
                await HttpClient.GetFromJsonAsync<List<ProductAttributeValueResponse>>($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes");
            initialAttributes.Should().HaveCount(2);

            // Act - Remove one attribute
            await HttpClient.DeleteAsync($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes/{NEGRO_ATTRIBUTE_VALUE_ID}");

            // Act - Get updated attributes
            List<ProductAttributeValueResponse>? updatedAttributes = 
                await HttpClient.GetFromJsonAsync<List<ProductAttributeValueResponse>>($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes");

            // Assert
            updatedAttributes.Should().HaveCount(1,
                because: "one attribute was removed");
            updatedAttributes.Should().Contain(a => a.AttributeCode == "storage");
        }

        [Fact]
        public async Task Should_ReturnVariantAttributesOnly_WhenFiltering()
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku();
            
            // Assign variant and non-variant attributes
            await AssignAttributeValueToSku(skuId, NEGRO_ATTRIBUTE_VALUE_ID); // Variant
            await AssignAttributeValueToSku(skuId, STORAGE_256GB_ATTRIBUTE_VALUE_ID); // Variant
            await AssignAttributeValueToSku(skuId, MATERIAL_ALUMINIO_ATTRIBUTE_VALUE_ID); // Non-variant

            // Act
            List<ProductAttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<ProductAttributeValueResponse>>($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes");

            // Assert
            attributeValues.Should().NotBeNull();
            attributeValues.Should().HaveCount(3);
            
            var variantAttributes = attributeValues!.Where(a => a.IsVariant).ToList();
            var nonVariantAttributes = attributeValues.Where(a => !a.IsVariant).ToList();
            
            variantAttributes.Should().HaveCount(2);
            nonVariantAttributes.Should().HaveCount(1);
        }

        [Theory]
        [InlineData(SMARTPHONE_GALAXY_X_ID, "Smartphone Galaxy X")]
        [InlineData(IPHONE_PRO_MAX_ID, "iPhone Pro Max")]
        [InlineData(LAPTOP_ULTRABOOK_PRO_ID, "Laptop Ultrabook Pro")]
        public async Task Should_ReturnAttributesForSkusOfDifferentProducts(
            long productId,
            string expectedProductName)
        {
            // Arrange
            SetAdminAuthentication();
            long skuId = await CreateActiveProductSku(productId);
            var responseAssign = await AssignAttributeValueToSku(skuId, NEGRO_ATTRIBUTE_VALUE_ID);
            var content = responseAssign.Content.ReadAsStringAsync();
            // Act
            List<ProductAttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<ProductAttributeValueResponse>>($"{ApiRoutes.ProductSkus.Base}/{skuId}/attributes");

            // Assert
            attributeValues.Should().NotBeNull();
            attributeValues.Should().AllSatisfy(attr =>
            {
                attr.ProductSku.Should().NotBeNull();
                attr.ProductSku!.Product.Should().NotBeNull();
                attr.ProductSku.Product!.Name.Should().Be(expectedProductName);
            });
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
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.ProductSkus.Base}/1/attributes");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserDoesNotHavePermission()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.ProductSkus.Base}/1/attributes");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
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

        private async Task<long> CreateActiveProductSkuWithAssignedAttributes(long? productId = null)
        {
            long skuId = await CreateActiveProductSku(productId);
            await AssignAttributeValueToSku(skuId, NEGRO_ATTRIBUTE_VALUE_ID);
            await AssignAttributeValueToSku(skuId, STORAGE_256GB_ATTRIBUTE_VALUE_ID);
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

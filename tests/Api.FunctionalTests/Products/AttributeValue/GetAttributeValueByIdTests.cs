using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Products.AttributeValues.Common.Mappers;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.AttributeValue.Create;

namespace Api.FunctionalTests.Products.AttributeValue
{
    public class GetAttributeValueByIdTests : BaseFunctionalTest
    {
        #region Seed Data Constants - Attributes

        /// <summary>
        /// Attribute ID 1: Color - String type, variant, required
        /// </summary>
        private const long COLOR_ATTRIBUTE_ID = 1;

        /// <summary>
        /// Attribute ID 2: Almacenamiento - Number type, variant, required
        /// </summary>
        private const long STORAGE_ATTRIBUTE_ID = 2;

        /// <summary>
        /// Attribute ID 3: Memoria RAM - Number type, variant, required
        /// </summary>
        private const long RAM_ATTRIBUTE_ID = 3;

        /// <summary>
        /// Attribute ID 4: Tamaño - String type, variant, required
        /// </summary>
        private const long SIZE_ATTRIBUTE_ID = 4;

        /// <summary>
        /// Attribute ID 5: Material - String type, non-variant, optional
        /// </summary>
        private const long MATERIAL_ATTRIBUTE_ID = 5;

        #endregion

        #region Seed Data Constants - AttributeValues

        /// <summary>
        /// AttributeValue ID 1: Negro - Color attribute
        /// Value: "Negro", NormalizedValue: "negro", DisplayOrder: 1, IsActive: true
        /// </summary>
        private const long NEGRO_VALUE_ID = 1;

        /// <summary>
        /// AttributeValue ID 2: Blanco - Color attribute
        /// Value: "Blanco", NormalizedValue: "blanco", DisplayOrder: 2, IsActive: true
        /// </summary>
        private const long BLANCO_VALUE_ID = 2;

        /// <summary>
        /// AttributeValue ID 3: Azul - Color attribute
        /// Value: "Azul", NormalizedValue: "azul", DisplayOrder: 3, IsActive: true
        /// </summary>
        private const long AZUL_VALUE_ID = 3;

        /// <summary>
        /// AttributeValue ID 4: 128 GB - Storage attribute
        /// Value: "128 GB", NormalizedValue: "128", NumericValue: 128, DisplayOrder: 1, IsActive: true
        /// </summary>
        private const long STORAGE_128GB_VALUE_ID = 4;

        /// <summary>
        /// AttributeValue ID 5: 256 GB - Storage attribute
        /// Value: "256 GB", NormalizedValue: "256", NumericValue: 256, DisplayOrder: 2, IsActive: true
        /// </summary>
        private const long STORAGE_256GB_VALUE_ID = 5;

        /// <summary>
        /// AttributeValue ID 6: 512 GB - Storage attribute
        /// Value: "512 GB", NormalizedValue: "512", NumericValue: 512, DisplayOrder: 3, IsActive: true
        /// </summary>
        private const long STORAGE_512GB_VALUE_ID = 6;

        /// <summary>
        /// AttributeValue ID 7: 8 GB - RAM attribute
        /// Value: "8 GB", NormalizedValue: "8", NumericValue: 8, DisplayOrder: 1, IsActive: true
        /// </summary>
        private const long RAM_8GB_VALUE_ID = 7;

        /// <summary>
        /// AttributeValue ID 8: 12 GB - RAM attribute
        /// Value: "12 GB", NormalizedValue: "12", NumericValue: 12, DisplayOrder: 2, IsActive: true
        /// </summary>
        private const long RAM_12GB_VALUE_ID = 8;

        /// <summary>
        /// AttributeValue ID 9: 16 GB - RAM attribute
        /// Value: "16 GB", NormalizedValue: "16", NumericValue: 16, DisplayOrder: 3, IsActive: true
        /// </summary>
        private const long RAM_16GB_VALUE_ID = 9;

        /// <summary>
        /// AttributeValue ID 10: S - Size attribute
        /// Value: "S", NormalizedValue: "s", DisplayOrder: 1, IsActive: true
        /// </summary>
        private const long SIZE_S_VALUE_ID = 10;

        /// <summary>
        /// AttributeValue ID 11: M - Size attribute
        /// Value: "M", NormalizedValue: "m", DisplayOrder: 2, IsActive: true
        /// </summary>
        private const long SIZE_M_VALUE_ID = 11;

        /// <summary>
        /// AttributeValue ID 12: L - Size attribute
        /// Value: "L", NormalizedValue: "l", DisplayOrder: 3, IsActive: true
        /// </summary>
        private const long SIZE_L_VALUE_ID = 12;

        /// <summary>
        /// AttributeValue ID 13: Aluminio - Material attribute
        /// Value: "Aluminio", NormalizedValue: "aluminio", DisplayOrder: 1, IsActive: true
        /// </summary>
        private const long MATERIAL_ALUMINIO_VALUE_ID = 13;

        /// <summary>
        /// AttributeValue ID 14: Plástico - Material attribute
        /// Value: "Plástico", NormalizedValue: "plastico", DisplayOrder: 2, IsActive: true
        /// </summary>
        private const long MATERIAL_PLASTICO_VALUE_ID = 14;

        #endregion

        public GetAttributeValueByIdTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Theory]
        [InlineData(0, "AttributeValueId", "is missing")]
        [InlineData(-1, "AttributeValueId", "is negative")]
        public async Task Should_ReturnNotFound_WhenAttributeValueIdIsInvalid(
            long invalidAttributeValueId,
            string fieldName,
            string reason)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.AttributeValues.Base}/{invalidAttributeValueId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                because: $"{fieldName} {reason}");
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenAttributeValueIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.AttributeValues.Base}/{Constants.NotExistingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_AndAttributeValue_WhenAttributeValueExists()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded attribute value: Negro (Color)
            AttributeValueResponse? attributeValue = await HttpClient.GetFromJsonAsync<AttributeValueResponse>(
                $"{ApiRoutes.AttributeValues.Base}/{NEGRO_VALUE_ID}");

            // Assert
            attributeValue.Should().NotBeNull();
            attributeValue!.Id.Should().Be(NEGRO_VALUE_ID);
            attributeValue.Value.Should().Be("Negro");
            attributeValue.DisplayOrder.Should().Be(1);
            attributeValue.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Should_ReturnAttributeValueWithAllFields()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded attribute value: 128 GB (Storage with numeric value)
            AttributeValueResponse? attributeValue = await HttpClient.GetFromJsonAsync<AttributeValueResponse>(
                $"{ApiRoutes.AttributeValues.Base}/{STORAGE_128GB_VALUE_ID}");

            // Assert
            attributeValue.Should().NotBeNull();
            attributeValue!.Id.Should().Be(STORAGE_128GB_VALUE_ID);
            attributeValue.Value.Should().Be("128 GB");
            attributeValue.NumericValue.Should().Be(128);
            attributeValue.BooleanValue.Should().BeNull();
            attributeValue.DisplayOrder.Should().Be(1);
            attributeValue.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Should_ReturnAttributeValueWithAttributeInformation()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded attribute value: Negro (Color)
            AttributeValueResponse? attributeValue = await HttpClient.GetFromJsonAsync<AttributeValueResponse>(
                $"{ApiRoutes.AttributeValues.Base}/{NEGRO_VALUE_ID}");

            // Assert
            attributeValue.Should().NotBeNull();
            attributeValue!.Attribute.Should().NotBeNull();
            attributeValue.Attribute!.Id.Should().Be(COLOR_ATTRIBUTE_ID);
            attributeValue.Attribute.Code.Should().Be("color");
            attributeValue.Attribute.Name.Should().Be("Color");
            attributeValue.Attribute.Description.Should().Be("Color del producto");
            attributeValue.Attribute.DataType.Should().Be("String");
            attributeValue.Attribute.IsVariant.Should().BeTrue();
            attributeValue.Attribute.IsFilterable.Should().BeTrue();
            attributeValue.Attribute.IsRequired.Should().BeTrue();
            attributeValue.Attribute.IsActive.Should().BeTrue();
        }

        [Theory]
        [InlineData(NEGRO_VALUE_ID, "Negro", COLOR_ATTRIBUTE_ID, "color", "Color")]
        [InlineData(BLANCO_VALUE_ID, "Blanco", COLOR_ATTRIBUTE_ID, "color", "Color")]
        [InlineData(AZUL_VALUE_ID, "Azul", COLOR_ATTRIBUTE_ID, "color", "Color")]
        public async Task Should_ReturnSeededColorAttributeValue_WithCorrectProperties(
            long attributeValueId,
            string expectedValue,
            long expectedAttributeId,
            string expectedAttributeCode,
            string expectedAttributeName)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            AttributeValueResponse? attributeValue = await HttpClient.GetFromJsonAsync<AttributeValueResponse>(
                $"{ApiRoutes.AttributeValues.Base}/{attributeValueId}");

            // Assert
            attributeValue.Should().NotBeNull();
            attributeValue!.Value.Should().Be(expectedValue);
            attributeValue.Attribute.Should().NotBeNull();
            attributeValue.Attribute!.Id.Should().Be(expectedAttributeId);
            attributeValue.Attribute.Code.Should().Be(expectedAttributeCode);
            attributeValue.Attribute.Name.Should().Be(expectedAttributeName);
        }

        [Theory]
        [InlineData(STORAGE_128GB_VALUE_ID, "128 GB", 128, 1)]
        [InlineData(STORAGE_256GB_VALUE_ID, "256 GB", 256, 2)]
        [InlineData(STORAGE_512GB_VALUE_ID, "512 GB", 512, 3)]
        public async Task Should_ReturnSeededStorageAttributeValue_WithNumericValue(
            long attributeValueId,
            string expectedValue,
            decimal expectedNumericValue,
            int expectedDisplayOrder)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            AttributeValueResponse? attributeValue = await HttpClient.GetFromJsonAsync<AttributeValueResponse>(
                $"{ApiRoutes.AttributeValues.Base}/{attributeValueId}");

            // Assert
            attributeValue.Should().NotBeNull();
            attributeValue!.Value.Should().Be(expectedValue);
            attributeValue.NumericValue.Should().Be(expectedNumericValue);
            attributeValue.BooleanValue.Should().BeNull();
            attributeValue.DisplayOrder.Should().Be(expectedDisplayOrder);
            attributeValue.Attribute.Should().NotBeNull();
            attributeValue.Attribute!.Id.Should().Be(STORAGE_ATTRIBUTE_ID);
            attributeValue.Attribute.Code.Should().Be("storage");
            attributeValue.Attribute.DataType.Should().Be("Number");
        }

        [Theory]
        [InlineData(RAM_8GB_VALUE_ID, "8 GB", 8, 1)]
        [InlineData(RAM_12GB_VALUE_ID, "12 GB", 12, 2)]
        [InlineData(RAM_16GB_VALUE_ID, "16 GB", 16, 3)]
        public async Task Should_ReturnSeededRamAttributeValue_WithNumericValue(
            long attributeValueId,
            string expectedValue,
            decimal expectedNumericValue,
            int expectedDisplayOrder)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            AttributeValueResponse? attributeValue = await HttpClient.GetFromJsonAsync<AttributeValueResponse>(
                $"{ApiRoutes.AttributeValues.Base}/{attributeValueId}");

            // Assert
            attributeValue.Should().NotBeNull();
            attributeValue!.Value.Should().Be(expectedValue);
            attributeValue.NumericValue.Should().Be(expectedNumericValue);
            attributeValue.BooleanValue.Should().BeNull();
            attributeValue.DisplayOrder.Should().Be(expectedDisplayOrder);
            attributeValue.Attribute.Should().NotBeNull();
            attributeValue.Attribute!.Id.Should().Be(RAM_ATTRIBUTE_ID);
            attributeValue.Attribute.Code.Should().Be("ram");
            attributeValue.Attribute.DataType.Should().Be("Number");
        }

        [Theory]
        [InlineData(SIZE_S_VALUE_ID, "S", 1)]
        [InlineData(SIZE_M_VALUE_ID, "M", 2)]
        [InlineData(SIZE_L_VALUE_ID, "L", 3)]
        public async Task Should_ReturnSeededSizeAttributeValue_WithCorrectProperties(
            long attributeValueId,
            string expectedValue,
            int expectedDisplayOrder)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            AttributeValueResponse? attributeValue = await HttpClient.GetFromJsonAsync<AttributeValueResponse>(
                $"{ApiRoutes.AttributeValues.Base}/{attributeValueId}");

            // Assert
            attributeValue.Should().NotBeNull();
            attributeValue!.Value.Should().Be(expectedValue);
            attributeValue.NumericValue.Should().BeNull();
            attributeValue.BooleanValue.Should().BeNull();
            attributeValue.DisplayOrder.Should().Be(expectedDisplayOrder);
            attributeValue.Attribute.Should().NotBeNull();
            attributeValue.Attribute!.Id.Should().Be(SIZE_ATTRIBUTE_ID);
            attributeValue.Attribute.Code.Should().Be("size");
            attributeValue.Attribute.Name.Should().Be("Tamaño");
        }

        [Fact]
        public async Task Should_ReturnActiveAttributeValue()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            AttributeValueResponse? attributeValue = await HttpClient.GetFromJsonAsync<AttributeValueResponse>(
                $"{ApiRoutes.AttributeValues.Base}/{NEGRO_VALUE_ID}");

            // Assert
            attributeValue.Should().NotBeNull();
            attributeValue!.IsActive.Should().BeTrue(
                because: "all seeded attribute values are active");
        }

        [Fact]
        public async Task Should_ReturnInactiveAttributeValue()
        {
            // Arrange
            SetAdminAuthentication();
            long inactiveValueId = await CreateInactiveAttributeValue();

            // Act
            AttributeValueResponse? attributeValue = await HttpClient.GetFromJsonAsync<AttributeValueResponse>(
                $"{ApiRoutes.AttributeValues.Base}/{inactiveValueId}");

            // Assert
            attributeValue.Should().NotBeNull();
            attributeValue!.IsActive.Should().BeFalse(
                because: "the attribute value was created as inactive");
        }

        [Fact]
        public async Task Should_ReturnAttributeValue_WithNullNumericAndBooleanValues()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Color values don't have numeric or boolean values
            AttributeValueResponse? attributeValue = await HttpClient.GetFromJsonAsync<AttributeValueResponse>(
                $"{ApiRoutes.AttributeValues.Base}/{NEGRO_VALUE_ID}");

            // Assert
            attributeValue.Should().NotBeNull();
            attributeValue!.NumericValue.Should().BeNull();
            attributeValue.BooleanValue.Should().BeNull();
        }

        [Fact]
        public async Task Should_ReturnAttributeValue_WithNumericValueNotNull()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Storage values have numeric values
            AttributeValueResponse? attributeValue = await HttpClient.GetFromJsonAsync<AttributeValueResponse>(
                $"{ApiRoutes.AttributeValues.Base}/{STORAGE_256GB_VALUE_ID}");

            // Assert
            attributeValue.Should().NotBeNull();
            attributeValue!.NumericValue.Should().NotBeNull();
            attributeValue.NumericValue.Should().Be(256);
        }

        [Fact]
        public async Task Should_ReturnAttributeValue_WithBooleanValueWhenCreated()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateAttributeValueWithBooleanValue(true);

            // Act
            AttributeValueResponse? attributeValue = await HttpClient.GetFromJsonAsync<AttributeValueResponse>(
                $"{ApiRoutes.AttributeValues.Base}/{attributeValueId}");

            // Assert
            attributeValue.Should().NotBeNull();
            attributeValue!.BooleanValue.Should().NotBeNull();
            attributeValue.BooleanValue.Should().BeTrue();
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
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.AttributeValues.Base}/1");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserDoesNotHavePermission()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.AttributeValues.Base}/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        // Helper methods para crear attribute values de prueba
        private async Task<long> CreateInactiveAttributeValue()
        {
            CreateAttributeValueRequest createRequest = new(
                AttributeId: COLOR_ATTRIBUTE_ID,
                Value: $"Inactive Value {Guid.NewGuid()}",
                NumericValue: null,
                BooleanValue: null,
                DisplayOrder: 10,
                IsActive: false);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long? attributeValueId = await response.Content.ReadFromJsonAsync<long?>();
            return attributeValueId!.Value;
        }

        private async Task<long> CreateAttributeValueWithBooleanValue(bool booleanValue)
        {
            CreateAttributeValueRequest createRequest = new(
                AttributeId: COLOR_ATTRIBUTE_ID,
                Value: $"Boolean Value {Guid.NewGuid()}",
                NumericValue: null,
                BooleanValue: booleanValue,
                DisplayOrder: 10,
                IsActive: true);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long? attributeValueId = await response.Content.ReadFromJsonAsync<long?>();
            return attributeValueId!.Value;
        }
    }
}

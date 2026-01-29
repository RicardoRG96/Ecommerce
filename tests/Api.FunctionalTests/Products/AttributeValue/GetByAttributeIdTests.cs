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
    public class GetByAttributeIdTests : BaseFunctionalTest
    {
        #region Seed Data Constants - Attributes

        /// <summary>
        /// Attribute ID 1: Color - String type, variant, required
        /// Has 3 values in seed: Negro (1), Blanco (2), Azul (3)
        /// </summary>
        private const long COLOR_ATTRIBUTE_ID = 1;

        /// <summary>
        /// Attribute ID 2: Almacenamiento - Number type, variant, required
        /// Has 3 values in seed: 128 GB (4), 256 GB (5), 512 GB (6)
        /// </summary>
        private const long STORAGE_ATTRIBUTE_ID = 2;

        /// <summary>
        /// Attribute ID 3: Memoria RAM - Number type, variant, required
        /// Has 3 values in seed: 8 GB (7), 12 GB (8), 16 GB (9)
        /// </summary>
        private const long RAM_ATTRIBUTE_ID = 3;

        /// <summary>
        /// Attribute ID 4: Tamaño - String type, variant, required
        /// Has 3 values in seed: S (10), M (11), L (12)
        /// </summary>
        private const long SIZE_ATTRIBUTE_ID = 4;

        /// <summary>
        /// Attribute ID 5: Material - String type, non-variant, optional
        /// Has 2 values in seed: Aluminio (13), Plástico (14)
        /// </summary>
        private const long MATERIAL_ATTRIBUTE_ID = 5;

        #endregion

        public GetByAttributeIdTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnOk_WhenAttributeHasValues()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.AttributeValues.Base}/attribute/{COLOR_ATTRIBUTE_ID}/values");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_ReturnListOfAttributeValues_WhenAttributeExists()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<AttributeValueResponse>>($"{ApiRoutes.AttributeValues.Base}/attribute/{COLOR_ATTRIBUTE_ID}/values");

            // Assert
            attributeValues.Should().NotBeNull();
            attributeValues.Should().NotBeEmpty();
            attributeValues.Should().HaveCountGreaterThanOrEqualTo(3,
                because: "Color attribute has at least 3 values in seed data");
        }

        [Fact]
        public async Task Should_ReturnEmptyList_WhenAttributeHasNoValues()
        {
            // Arrange
            SetAdminAuthentication();
            long emptyAttributeId = await CreateAttributeWithoutValues();

            // Act
            List<AttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<AttributeValueResponse>>($"{ApiRoutes.AttributeValues.Base}/attribute/{emptyAttributeId}/values");

            // Assert
            attributeValues.Should().NotBeNull();
            attributeValues.Should().BeEmpty(
                because: "attribute was created without any values");
        }

        [Fact]
        public async Task Should_ReturnExpectedNumberOfColorAttributeValues()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<AttributeValueResponse>>($"{ApiRoutes.AttributeValues.Base}/attribute/{COLOR_ATTRIBUTE_ID}/values");

            // Assert - According to seed data: Negro, Blanco, Azul = 3 values
            attributeValues.Should().NotBeNull();
            attributeValues.Should().HaveCount(4,
                because: "Color attribute has exactly 4 values in seed data");
        }

        [Fact]
        public async Task Should_ReturnAllColorAttributeValues()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<AttributeValueResponse>>($"{ApiRoutes.AttributeValues.Base}/attribute/{COLOR_ATTRIBUTE_ID}/values");

            // Assert
            attributeValues.Should().NotBeNull();
            attributeValues.Should().Contain(v => v.Value == "Negro");
            attributeValues.Should().Contain(v => v.Value == "Blanco");
            attributeValues.Should().Contain(v => v.Value == "Azul");
        }

        [Fact]
        public async Task Should_ReturnAttributeValuesWithAllProperties()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<AttributeValueResponse>>($"{ApiRoutes.AttributeValues.Base}/attribute/{COLOR_ATTRIBUTE_ID}/values");

            // Assert
            attributeValues.Should().NotBeNull();
            attributeValues.Should().NotBeEmpty();

            AttributeValueResponse? negroValue = attributeValues!.FirstOrDefault(v => v.Value == "Negro");
            negroValue.Should().NotBeNull();
            negroValue!.Id.Should().BeGreaterThan(0);
            negroValue.Value.Should().Be("Negro");
            negroValue.DisplayOrder.Should().Be(1);
            negroValue.IsActive.Should().BeTrue();
            negroValue.Attribute.Should().NotBeNull();
            negroValue.Attribute!.Id.Should().Be(COLOR_ATTRIBUTE_ID);
            negroValue.Attribute.Code.Should().Be("color");
            negroValue.Attribute.Name.Should().Be("Color");
        }

        [Fact]
        public async Task Should_ReturnStorageAttributeValues_WithNumericValues()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<AttributeValueResponse>>($"{ApiRoutes.AttributeValues.Base}/attribute/{STORAGE_ATTRIBUTE_ID}/values");

            // Assert
            attributeValues.Should().NotBeNull();
            attributeValues.Should().HaveCount(3,
                because: "Storage attribute has 3 values in seed data");
            
            attributeValues.Should().Contain(v => v.Value == "128 GB" && v.NumericValue == 128);
            attributeValues.Should().Contain(v => v.Value == "256 GB" && v.NumericValue == 256);
            attributeValues.Should().Contain(v => v.Value == "512 GB" && v.NumericValue == 512);
            
            attributeValues.Should().AllSatisfy(v =>
            {
                v.NumericValue.Should().NotBeNull(
                    because: "Storage values have numeric values");
                v.Attribute.Should().NotBeNull();
                v.Attribute!.Code.Should().Be("storage");
                v.Attribute.DataType.Should().Be("Number");
            });
        }

        [Fact]
        public async Task Should_ReturnRamAttributeValues_WithNumericValues()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<AttributeValueResponse>>($"{ApiRoutes.AttributeValues.Base}/attribute/{RAM_ATTRIBUTE_ID}/values");

            // Assert
            attributeValues.Should().NotBeNull();
            attributeValues.Should().HaveCount(3,
                because: "RAM attribute has 3 values in seed data");
            
            attributeValues.Should().Contain(v => v.Value == "8 GB" && v.NumericValue == 8);
            attributeValues.Should().Contain(v => v.Value == "12 GB" && v.NumericValue == 12);
            attributeValues.Should().Contain(v => v.Value == "16 GB" && v.NumericValue == 16);
            
            attributeValues.Should().AllSatisfy(v =>
            {
                v.NumericValue.Should().NotBeNull();
                v.Attribute.Should().NotBeNull();
                v.Attribute!.Code.Should().Be("ram");
            });
        }

        [Fact]
        public async Task Should_ReturnSizeAttributeValues()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<AttributeValueResponse>>($"{ApiRoutes.AttributeValues.Base}/attribute/{SIZE_ATTRIBUTE_ID}/values");

            // Assert
            attributeValues.Should().NotBeNull();
            attributeValues.Should().HaveCount(3,
                because: "Size attribute has 3 values in seed data");
            
            attributeValues.Should().Contain(v => v.Value == "S");
            attributeValues.Should().Contain(v => v.Value == "M");
            attributeValues.Should().Contain(v => v.Value == "L");
            
            attributeValues.Should().AllSatisfy(v =>
            {
                v.NumericValue.Should().BeNull(
                    because: "Size values don't have numeric values");
                v.Attribute.Should().NotBeNull();
                v.Attribute!.Code.Should().Be("size");
            });
        }

        [Fact]
        public async Task Should_ReturnAttributeValues_OrderedByDisplayOrder()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<AttributeValueResponse>>($"{ApiRoutes.AttributeValues.Base}/attribute/{COLOR_ATTRIBUTE_ID}/values");

            // Assert
            attributeValues.Should().NotBeNull();
            attributeValues.Should().NotBeEmpty();

            // Verify that values are ordered by DisplayOrder
            attributeValues![0].Value.Should().Be("Negro");
            attributeValues[0].DisplayOrder.Should().Be(1);
            attributeValues[1].Value.Should().Be("Blanco");
            attributeValues[1].DisplayOrder.Should().Be(2);
            attributeValues[2].Value.Should().Be("Azul");
            attributeValues[2].DisplayOrder.Should().Be(3);
        }

        [Fact]
        public async Task Should_ReturnBothActiveAndInactiveAttributeValues()
        {
            // Arrange
            SetAdminAuthentication();
            long testAttributeId = await CreateTestAttribute();
            await CreateTestAttributeValue(testAttributeId, "Active Value", isActive: true);
            await CreateTestAttributeValue(testAttributeId, "Inactive Value", isActive: false);

            // Act
            List<AttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<AttributeValueResponse>>($"{ApiRoutes.AttributeValues.Base}/attribute/{testAttributeId}/values");

            // Assert
            attributeValues.Should().NotBeNull();
            attributeValues.Should().Contain(v => v.Value == "Active Value" && v.IsActive);
            attributeValues.Should().Contain(v => v.Value == "Inactive Value" && !v.IsActive);
        }

        [Fact]
        public async Task Should_IncludeNewlyCreatedAttributeValues()
        {
            // Arrange
            SetAdminAuthentication();
            await CreateTestAttributeValue(COLOR_ATTRIBUTE_ID, "Custom Color Value");

            // Act
            List<AttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<AttributeValueResponse>>($"{ApiRoutes.AttributeValues.Base}/attribute/{COLOR_ATTRIBUTE_ID}/values");

            // Assert
            attributeValues.Should().NotBeNull();
            attributeValues.Should().HaveCountGreaterThanOrEqualTo(4,
                because: "3 seeded values + 1 newly created");
            attributeValues.Should().Contain(v => v.Value == "Custom Color Value");
        }

        [Theory]
        [InlineData(COLOR_ATTRIBUTE_ID, 3, "Color")]
        [InlineData(STORAGE_ATTRIBUTE_ID, 3, "Storage")]
        [InlineData(RAM_ATTRIBUTE_ID, 3, "RAM")]
        [InlineData(SIZE_ATTRIBUTE_ID, 3, "Size")]
        [InlineData(MATERIAL_ATTRIBUTE_ID, 2, "Material")]
        public async Task Should_ReturnCorrectCountForEachSeededAttribute(
            long attributeId,
            int expectedCount,
            string attributeName)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeValueResponse>? attributeValues = 
                await HttpClient.GetFromJsonAsync<List<AttributeValueResponse>>($"{ApiRoutes.AttributeValues.Base}/attribute/{attributeId}/values");

            // Assert
            attributeValues.Should().NotBeNull();
            attributeValues.Should().HaveCount(expectedCount,
                because: $"{attributeName} attribute has {expectedCount} values in seed data");
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
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.AttributeValues.Base}/attribute/1/values");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserDoesNotHavePermission()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.AttributeValues.Base}/attribute/1/values");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        // Helper methods para crear attributes y attribute values de prueba
        private async Task<long> CreateAttributeWithoutValues()
        {
            Web.Api.Endpoints.v1.Products.Attribute.Create.CreateAttributeRequest createRequest = new(
                Code: $"empty-attr-{Guid.NewGuid()}",
                Name: "Empty Attribute",
                Description: "Attribute with no values",
                DataType: "string",
                IsVariant: false,
                IsFilterable: false,
                IsRequired: false,
                DisplayOrder: 100,
                IsActive: true);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(
                ApiRoutes.Attributes.Base, 
                createRequest);
            response.EnsureSuccessStatusCode();

            long? attributeId = await response.Content.ReadFromJsonAsync<long?>();
            return attributeId!.Value;
        }

        private async Task<long> CreateTestAttribute()
        {
            Web.Api.Endpoints.v1.Products.Attribute.Create.CreateAttributeRequest createRequest = new(
                Code: $"test-attr-{Guid.NewGuid()}",
                Name: "Test Attribute",
                Description: "Test attribute for values",
                DataType: "string",
                IsVariant: false,
                IsFilterable: false,
                IsRequired: false,
                DisplayOrder: 100,
                IsActive: true);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(
                ApiRoutes.Attributes.Base, 
                createRequest);
            response.EnsureSuccessStatusCode();

            long? attributeId = await response.Content.ReadFromJsonAsync<long?>();
            return attributeId!.Value;
        }

        private async Task<long> CreateTestAttributeValue(
            long attributeId, 
            string value, 
            bool isActive = true)
        {
            CreateAttributeValueRequest createRequest = new(
                AttributeId: attributeId,
                Value: value,
                NumericValue: null,
                BooleanValue: null,
                DisplayOrder: 10,
                IsActive: isActive);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(
                ApiRoutes.AttributeValues.Base, 
                createRequest);
            response.EnsureSuccessStatusCode();

            long? attributeValueId = await response.Content.ReadFromJsonAsync<long?>();
            return attributeValueId!.Value;
        }
    }
}

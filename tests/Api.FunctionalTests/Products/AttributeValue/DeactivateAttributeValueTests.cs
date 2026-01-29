using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.AttributeValue.Create;

namespace Api.FunctionalTests.Products.AttributeValue
{
    public class DeactivateAttributeValueTests : BaseFunctionalTest
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

        /// <summary>
        /// AttributeValue ID 1: AttributeId = 1 - Negro
        /// </summary>
        private const long COLOR_ATTRIBUTE_VALUE_ID = 1;

        #endregion

        public DeactivateAttributeValueTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
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

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.AttributeValues.Base}/{invalidAttributeValueId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                because: $"{fieldName} {reason}");
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenAttributeValueIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.AttributeValues.Base}/{Constants.NotExistingId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenAttributeValueIdExists()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateActiveAttributeValue();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenAttributeValueIsAlreadyInactive()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateActiveAttributeValue();

            // Act - First deactivation
            await HttpClient.PatchAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}/deactivate", null!);

            // Act - Second deactivation (the attribute value is already inactive)
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "deactivating an already inactive attribute value should be idempotent");
        }

        [Fact]
        public async Task Should_DeactivateActiveAttributeValue_Successfully()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateActiveAttributeValue();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
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
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.AttributeValues.Base}/1/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserIsNotAdmin()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.AttributeValues.Base}/1/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAttributeValueHasActiveSkus()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PatchAsync(
                $"{ApiRoutes.AttributeValues.Base}/{COLOR_ATTRIBUTE_VALUE_ID}/deactivate", 
                null!);

            string content = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("has active SKUs");
        }

        [Theory]
        [MemberData(nameof(GetValidAttributeValueConfigurations))]
        public async Task Should_DeactivateAttributeValue_WithDifferentConfigurations(
            long attributeId,
            string value,
            decimal? numericValue,
            bool? booleanValue,
            string scenario)
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateActiveAttributeValue(
                attributeId: attributeId,
                value: value,
                numericValue: numericValue,
                booleanValue: booleanValue);

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent, because: scenario);
        }

        public static IEnumerable<object[]> GetValidAttributeValueConfigurations()
        {
            yield return new object[] 
            { 
                COLOR_ATTRIBUTE_ID, 
                "Verde", 
                null, 
                null, 
                "string attribute value" 
            };
            
            yield return new object[] 
            { 
                STORAGE_ATTRIBUTE_ID, 
                "1 TB", 
                1024m, 
                null, 
                "numeric attribute value with storage" 
            };
            
            yield return new object[] 
            { 
                RAM_ATTRIBUTE_ID, 
                "32 GB", 
                32m, 
                null, 
                "numeric attribute value with ram" 
            };
            
            yield return new object[] 
            { 
                SIZE_ATTRIBUTE_ID, 
                "XXL", 
                null, 
                null, 
                "size attribute value" 
            };
            
            yield return new object[] 
            { 
                MATERIAL_ATTRIBUTE_ID, 
                "Acero", 
                null, 
                null, 
                "material attribute value (non-variant)" 
            };
            
            yield return new object[] 
            { 
                COLOR_ATTRIBUTE_ID, 
                "Azul Claro", 
                null, 
                true, 
                "attribute value with boolean value" 
            };
        }

        [Fact]
        public async Task Should_DeactivateAttributeValue_WithHighDisplayOrder()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateActiveAttributeValue(
                attributeId: COLOR_ATTRIBUTE_ID,
                value: "Rosa Brillante",
                displayOrder: 999);

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData(COLOR_ATTRIBUTE_ID, "Rojo Carmesí", "color attribute")]
        [InlineData(SIZE_ATTRIBUTE_ID, "XXXL", "size attribute")]
        [InlineData(MATERIAL_ATTRIBUTE_ID, "Vidrio", "material attribute")]
        public async Task Should_DeactivateAttributeValue_ForDifferentSeededAttributes(
            long attributeId,
            string value,
            string scenario)
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateActiveAttributeValue(attributeId, value);

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent, because: scenario);
        }

        [Fact]
        public async Task Should_DeactivateInactiveAttributeValue_Successfully()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateInactiveAttributeValue();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "deactivating an inactive attribute value should be idempotent");
        }

        [Fact]
        public async Task Should_DeactivateAttributeValue_WithNumericValue()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateActiveAttributeValue(
                attributeId: STORAGE_ATTRIBUTE_ID,
                value: "2 TB",
                numericValue: 2048);

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_DeactivateAttributeValue_WithBooleanValue()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateActiveAttributeValue(
                attributeId: COLOR_ATTRIBUTE_ID,
                value: "Featured Color",
                booleanValue: true);

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        // Helper methods para crear attribute values de prueba
        private async Task<long> CreateActiveAttributeValue(
            long? attributeId = null,
            string? value = null,
            decimal? numericValue = null,
            bool? booleanValue = null,
            int displayOrder = 1)
        {
            CreateAttributeValueRequest createRequest = new(
                AttributeId: attributeId ?? COLOR_ATTRIBUTE_ID,
                Value: value ?? $"Active Value {Guid.NewGuid()}",
                NumericValue: numericValue,
                BooleanValue: booleanValue,
                DisplayOrder: displayOrder,
                IsActive: true);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long? attributeValueId = await response.Content.ReadFromJsonAsync<long?>();
            return attributeValueId!.Value;
        }

        private async Task<long> CreateInactiveAttributeValue()
        {
            CreateAttributeValueRequest createRequest = new(
                AttributeId: COLOR_ATTRIBUTE_ID,
                Value: $"Inactive Value {Guid.NewGuid()}",
                NumericValue: null,
                BooleanValue: null,
                DisplayOrder: 1,
                IsActive: false);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long? attributeValueId = await response.Content.ReadFromJsonAsync<long?>();
            return attributeValueId!.Value;
        }
    }
}

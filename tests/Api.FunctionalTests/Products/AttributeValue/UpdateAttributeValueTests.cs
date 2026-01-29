using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.AttributeValue.Create;
using Web.Api.Endpoints.v1.Products.AttributeValue.Update;

namespace Api.FunctionalTests.Products.AttributeValue
{
    public class UpdateAttributeValueTests : BaseFunctionalTest
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

        private static readonly UpdateAttributeValueRequest _request = new(
            AttributeId: COLOR_ATTRIBUTE_ID,
            Value: "Rojo Actualizado",
            NumericValue: null,
            BooleanValue: null,
            DisplayOrder: 5);

        public UpdateAttributeValueTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        // Consolida tests similares usando Theory
        [Theory]
        [MemberData(nameof(GetInvalidMaxLengthRequests))]
        public async Task Should_ReturnBadRequest_WhenFieldExceedsMaximumLength(
            UpdateAttributeValueRequest invalidRequest, 
            string fieldName)
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateTestAttributeValue();

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, 
                because: $"{fieldName} exceeds maximum length");
        }

        public static IEnumerable<object[]> GetInvalidMaxLengthRequests()
        {
            yield return new object[] 
            { 
                _request with { Value = Constants.ExceededMaximumLengthField }, 
                nameof(UpdateAttributeValueRequest.Value) 
            };
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
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.AttributeValues.Base}/{invalidAttributeValueId}", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                because: $"{fieldName} {reason}");
        }

        [Theory]
        [InlineData(0, "AttributeId", "is missing")]
        [InlineData(-1, "AttributeId", "is negative")]
        public async Task Should_ReturnBadRequest_WhenAttributeIdIsInvalid(
            long invalidAttributeId,
            string fieldName,
            string reason)
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateTestAttributeValue();
            UpdateAttributeValueRequest invalidRequest = _request with { AttributeId = invalidAttributeId };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                because: $"{fieldName} {reason}");
        }

        [Theory]
        [InlineData(-1, "DisplayOrder", "is negative")]
        [InlineData(-10, "DisplayOrder", "is negative")]
        public async Task Should_ReturnBadRequest_WhenDisplayOrderIsInvalid(
            int invalidDisplayOrder, 
            string fieldName, 
            string reason)
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateTestAttributeValue();
            UpdateAttributeValueRequest invalidRequest = _request with { DisplayOrder = invalidDisplayOrder };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}", invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                because: $"{fieldName} {reason}");
        }

        // Verifica mensajes de error específicos
        [Fact]
        public async Task Should_ReturnBadRequest_WhenValueIsMissing()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateTestAttributeValue();
            UpdateAttributeValueRequest invalidRequest = _request with { Value = "" };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}", invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("Value is required",
                because: "validation message should be clear for debugging");
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenAttributeValueIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.AttributeValues.Base}/{Constants.NotExistingId}", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenAttributeIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateTestAttributeValue();
            UpdateAttributeValueRequest invalidRequest = _request with { AttributeId = Constants.NotExistingId };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAttributeIsInactive()
        {
            // Arrange
            SetAdminAuthentication();
            
            // Create an inactive attribute
            long inactiveAttributeId = await CreateInactiveAttribute();
            
            // Create an attribute value with an active attribute
            long attributeValueId = await CreateTestAttributeValue(COLOR_ATTRIBUTE_ID, "Original Value");

            // Try to update the attribute value to use the inactive attribute
            UpdateAttributeValueRequest invalidRequest = _request with 
            { 
                AttributeId = inactiveAttributeId,
                Value = $"New Value-{Guid.NewGuid()}"
            };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync(
                $"{ApiRoutes.AttributeValues.Base}/{attributeValueId}", 
                invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenValueAlreadyExistsForAttribute()
        {
            // Arrange
            SetAdminAuthentication();
            string uniqueValue1 = $"Verde-{Guid.NewGuid()}";
            string uniqueValue2 = $"Amarillo-{Guid.NewGuid()}";
            
            long attributeValueId1 = await CreateTestAttributeValue(COLOR_ATTRIBUTE_ID, uniqueValue1);
            long attributeValueId2 = await CreateTestAttributeValue(COLOR_ATTRIBUTE_ID, uniqueValue2);

            // Act - Try to change attributeValue2's value to attributeValue1's value
            UpdateAttributeValueRequest invalidRequest = _request with { Value = uniqueValue1 };
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId2}", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenRequestIsValid()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateTestAttributeValue();

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_AllowKeepingSameValue_WhenUpdatingSameAttributeValue()
        {
            // Arrange
            SetAdminAuthentication();
            string uniqueValue = $"Morado-{Guid.NewGuid()}";
            long attributeValueId = await CreateTestAttributeValue(COLOR_ATTRIBUTE_ID, uniqueValue);

            // Act - Update attribute value keeping the same value
            UpdateAttributeValueRequest sameValueRequest = _request with { Value = uniqueValue };
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}", sameValueRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        // Consolida casos exitosos con diferentes configuraciones
        [Theory]
        [MemberData(nameof(GetValidAttributeValueUpdateConfigurations))]
        public async Task Should_ReturnNoContent_WhenRequestIsValidWithDifferentConfigurations(
            UpdateAttributeValueRequest validRequest,
            long attributeId,
            string scenario)
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateTestAttributeValue(attributeId);

            UpdateAttributeValueRequest uniqueRequest = validRequest with
            {
                AttributeId = attributeId,
                Value = $"{validRequest.Value}-{Guid.NewGuid()}"
            };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}", uniqueRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent, because: scenario);
        }

        public static IEnumerable<object[]> GetValidAttributeValueUpdateConfigurations()
        {
            yield return new object[]
            {
                new UpdateAttributeValueRequest(
                    AttributeId: STORAGE_ATTRIBUTE_ID,
                    Value: "2 TB",
                    NumericValue: 2048,
                    BooleanValue: null,
                    DisplayOrder: 10),
                STORAGE_ATTRIBUTE_ID,
                "storage with numeric value should be updated successfully"
            };

            yield return new object[]
            {
                new UpdateAttributeValueRequest(
                    AttributeId: RAM_ATTRIBUTE_ID,
                    Value: "64 GB",
                    NumericValue: 64,
                    BooleanValue: null,
                    DisplayOrder: 5),
                RAM_ATTRIBUTE_ID,
                "ram with numeric value should be updated successfully"
            };

            yield return new object[]
            {
                new UpdateAttributeValueRequest(
                    AttributeId: SIZE_ATTRIBUTE_ID,
                    Value: "XXL",
                    NumericValue: null,
                    BooleanValue: null,
                    DisplayOrder: 6),
                SIZE_ATTRIBUTE_ID,
                "size value should be updated successfully"
            };

            yield return new object[]
            {
                new UpdateAttributeValueRequest(
                    AttributeId: MATERIAL_ATTRIBUTE_ID,
                    Value: "Titanio",
                    NumericValue: null,
                    BooleanValue: null,
                    DisplayOrder: 3),
                MATERIAL_ATTRIBUTE_ID,
                "material value should be updated successfully"
            };

            yield return new object[]
            {
                new UpdateAttributeValueRequest(
                    AttributeId: COLOR_ATTRIBUTE_ID,
                    Value: "Plateado",
                    NumericValue: null,
                    BooleanValue: true,
                    DisplayOrder: 999),
                COLOR_ATTRIBUTE_ID,
                "high display order with boolean value should be updated successfully"
            };
        }

        // Tests específicos para actualización de valores numéricos
        [Theory]
        [InlineData(128, "128 GB Updated")]
        [InlineData(256, "256 GB Updated")]
        [InlineData(512, "512 GB Updated")]
        [InlineData(1024, "1 TB Updated")]
        [InlineData(2048, "2 TB Updated")]
        public async Task Should_ReturnNoContent_WhenUpdatingStorageValueWithNumericValue(
            decimal numericValue,
            string displayValue)
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateTestAttributeValue(STORAGE_ATTRIBUTE_ID, "Old Storage");

            UpdateAttributeValueRequest request = new(
                AttributeId: STORAGE_ATTRIBUTE_ID,
                Value: displayValue,
                NumericValue: numericValue,
                BooleanValue: null,
                DisplayOrder: 10);

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData(4, "4 GB Updated")]
        [InlineData(8, "8 GB Updated")]
        [InlineData(16, "16 GB Updated")]
        [InlineData(32, "32 GB Updated")]
        [InlineData(64, "64 GB Updated")]
        public async Task Should_ReturnNoContent_WhenUpdatingRamValueWithNumericValue(
            decimal numericValue,
            string displayValue)
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateTestAttributeValue(RAM_ATTRIBUTE_ID, "Old RAM");

            UpdateAttributeValueRequest request = new(
                AttributeId: RAM_ATTRIBUTE_ID,
                Value: displayValue,
                NumericValue: numericValue,
                BooleanValue: null,
                DisplayOrder: 10);

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        // Tests para actualización de valores booleanos
        [Theory]
        [InlineData(true, "Yes Updated")]
        [InlineData(false, "No Updated")]
        public async Task Should_ReturnNoContent_WhenUpdatingValueWithBooleanValue(
            bool booleanValue,
            string displayValue)
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateTestAttributeValue(COLOR_ATTRIBUTE_ID, "Old Value");

            UpdateAttributeValueRequest request = new(
                AttributeId: COLOR_ATTRIBUTE_ID,
                Value: displayValue,
                NumericValue: null,
                BooleanValue: booleanValue,
                DisplayOrder: 10);

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        // Tests para cambiar de atributo
        [Fact]
        public async Task Should_ReturnNoContent_WhenChangingAttributeId()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateTestAttributeValue(COLOR_ATTRIBUTE_ID, "Original Color");

            UpdateAttributeValueRequest request = new(
                AttributeId: SIZE_ATTRIBUTE_ID, // Changing from Color to Size
                Value: $"XXL-{Guid.NewGuid()}",
                NumericValue: null,
                BooleanValue: null,
                DisplayOrder: 10);

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        // Tests con valores opcionales null
        [Fact]
        public async Task Should_ReturnNoContent_WhenNumericAndBooleanValuesAreNull()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeValueId = await CreateTestAttributeValue(COLOR_ATTRIBUTE_ID, "Original");

            UpdateAttributeValueRequest request = new(
                AttributeId: COLOR_ATTRIBUTE_ID,
                Value: $"Updated-{Guid.NewGuid()}",
                NumericValue: null,
                BooleanValue: null,
                DisplayOrder: 10);

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        // Tests para diferentes DisplayOrders
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
            long attributeValueId = await CreateTestAttributeValue(COLOR_ATTRIBUTE_ID, "Original");

            UpdateAttributeValueRequest request = new(
                AttributeId: COLOR_ATTRIBUTE_ID,
                Value: $"Updated-{Guid.NewGuid()}",
                NumericValue: null,
                BooleanValue: null,
                DisplayOrder: displayOrder);

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.AttributeValues.Base}/{attributeValueId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent, because: scenario);
        }

        // Usa Theory para tests de autorización también
        [Theory]
        [InlineData(HttpStatusCode.Unauthorized, "no authentication token")]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotAuthenticated(
            HttpStatusCode expectedStatusCode,
            string reason)
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = null;

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.AttributeValues.Base}/1", _request);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserIsNotAdmin()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.AttributeValues.Base}/1", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        // Helper methods para crear attribute values y attributes de prueba
        private async Task<long> CreateTestAttributeValue(long? attributeId = null, string? value = null)
        {
            CreateAttributeValueRequest createRequest = new(
                AttributeId: attributeId ?? COLOR_ATTRIBUTE_ID,
                Value: value ?? $"Test Value {Guid.NewGuid()}",
                NumericValue: null,
                BooleanValue: null,
                DisplayOrder: 1,
                IsActive: true);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long? attributeValueId = await response.Content.ReadFromJsonAsync<long?>();
            return attributeValueId!.Value;
        }

        private async Task<long> CreateInactiveAttribute()
        {
            Web.Api.Endpoints.v1.Products.Attribute.Create.CreateAttributeRequest createRequest = new(
                Code: $"inactive-attr-{Guid.NewGuid()}",
                Name: "Inactive Test Attribute",
                Description: "This attribute is inactive",
                DataType: "string",
                IsVariant: false,
                IsFilterable: false,
                IsRequired: false,
                DisplayOrder: 100,
                IsActive: false);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(
                ApiRoutes.Attributes.Base, 
                createRequest);
            response.EnsureSuccessStatusCode();

            long? attributeId = await response.Content.ReadFromJsonAsync<long?>();
            return attributeId!.Value;
        }
    }
}

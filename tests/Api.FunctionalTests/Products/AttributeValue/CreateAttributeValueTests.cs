using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.AttributeValue.Create;

namespace Api.FunctionalTests.Products.AttributeValue
{
    public class CreateAttributeValueTests : BaseFunctionalTest
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

        private static readonly CreateAttributeValueRequest _request = new(
            AttributeId: COLOR_ATTRIBUTE_ID,
            Value: "Rojo",
            NumericValue: null,
            BooleanValue: null,
            DisplayOrder: 4,
            IsActive: true);

        public CreateAttributeValueTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        // Consolida tests similares usando Theory
        [Theory]
        [MemberData(nameof(GetInvalidMaxLengthRequests))]
        public async Task Should_ReturnBadRequest_WhenFieldExceedsMaximumLength(
            CreateAttributeValueRequest invalidRequest, 
            string fieldName)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, 
                because: $"{fieldName} exceeds maximum length");
        }

        public static IEnumerable<object[]> GetInvalidMaxLengthRequests()
        {
            yield return new object[] 
            { 
                _request with { Value = Constants.ExceededMaximumLengthField }, 
                nameof(CreateAttributeValueRequest.Value) 
            };
        }

        // Consolida validaciones de IDs
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
            CreateAttributeValueRequest invalidRequest = _request with { AttributeId = invalidAttributeId };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, invalidRequest);

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
            CreateAttributeValueRequest invalidRequest = _request with { DisplayOrder = invalidDisplayOrder };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, 
                because: $"{fieldName} {reason}");
            content.Should().Contain("DisplayOrder must be zero or greater");
        }

        // Verifica mensajes de error específicos
        [Fact]
        public async Task Should_ReturnBadRequest_WhenValueIsMissing()
        {
            // Arrange
            SetAdminAuthentication();
            CreateAttributeValueRequest invalidRequest = _request with { Value = "" };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("Value is required", 
                because: "validation message should be clear for debugging");
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenAttributeIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();
            CreateAttributeValueRequest invalidRequest = _request with { AttributeId = Constants.NotExistingId };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().Contain("Attribute", 
                because: "error should indicate which Attribute was not found");
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenValueAlreadyExistsForAttribute()
        {
            // Arrange
            SetAdminAuthentication();
            string uniqueValue = $"Verde-{Guid.NewGuid()}";
            CreateAttributeValueRequest firstRequest = _request with { Value = uniqueValue };

            // Act - Create attribute value first time
            HttpResponseMessage firstResponse = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, firstRequest);
            firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Act - Try to create attribute value with same value for same attribute
            CreateAttributeValueRequest duplicateRequest = _request with { Value = uniqueValue };
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, duplicateRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("already exists", 
                because: "duplicate value for same attribute should be clearly indicated");
        }

        // Verifica el contenido de la respuesta exitosa
        [Fact]
        public async Task Should_ReturnOk_WhenRequestIsValidWithAllFields()
        {
            // Arrange
            SetAdminAuthentication();
            CreateAttributeValueRequest request = _request with 
            { 
                Value = $"Amarillo-{Guid.NewGuid()}"
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, request);
            long? attributeValueId = await response.Content.ReadFromJsonAsync<long?>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            attributeValueId.Should().BeGreaterThan(0, 
                because: "a valid attribute value ID should be returned");
        }

        // Tests con diferentes tipos de atributos del seed
        [Theory]
        [MemberData(nameof(GetValidAttributeValueConfigurations))]
        public async Task Should_ReturnOk_WhenRequestIsValidWithDifferentConfigurations(
            CreateAttributeValueRequest validRequest,
            string scenario)
        {
            // Arrange
            SetAdminAuthentication();
            CreateAttributeValueRequest uniqueRequest = validRequest with 
            { 
                Value = $"{validRequest.Value}-{Guid.NewGuid()}" 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, uniqueRequest);
            long? attributeValueId = await response.Content.ReadFromJsonAsync<long?>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK, because: scenario);
            attributeValueId.Should().BeGreaterThan(0);
        }

        public static IEnumerable<object[]> GetValidAttributeValueConfigurations()
        {
            // String attribute - Color
            yield return new object[]
            {
                new CreateAttributeValueRequest(
                    AttributeId: COLOR_ATTRIBUTE_ID,
                    Value: "Verde",
                    NumericValue: null,
                    BooleanValue: null,
                    DisplayOrder: 4,
                    IsActive: true),
                "string attribute value should be created successfully"
            };

            // Number attribute - Storage with numeric value
            yield return new object[]
            {
                new CreateAttributeValueRequest(
                    AttributeId: STORAGE_ATTRIBUTE_ID,
                    Value: "1 TB",
                    NumericValue: 1024,
                    BooleanValue: null,
                    DisplayOrder: 4,
                    IsActive: true),
                "numeric attribute value should be created successfully"
            };

            // Number attribute - RAM with numeric value
            yield return new object[]
            {
                new CreateAttributeValueRequest(
                    AttributeId: RAM_ATTRIBUTE_ID,
                    Value: "32 GB",
                    NumericValue: 32,
                    BooleanValue: null,
                    DisplayOrder: 4,
                    IsActive: true),
                "ram numeric value should be created successfully"
            };

            // String attribute - Size
            yield return new object[]
            {
                new CreateAttributeValueRequest(
                    AttributeId: SIZE_ATTRIBUTE_ID,
                    Value: "XL",
                    NumericValue: null,
                    BooleanValue: null,
                    DisplayOrder: 4,
                    IsActive: true),
                "size value should be created successfully"
            };

            // String attribute - Material (non-variant, optional)
            yield return new object[]
            {
                new CreateAttributeValueRequest(
                    AttributeId: MATERIAL_ATTRIBUTE_ID,
                    Value: "Acero",
                    NumericValue: null,
                    BooleanValue: null,
                    DisplayOrder: 3,
                    IsActive: true),
                "material value should be created successfully"
            };

            // Inactive attribute value
            yield return new object[]
            {
                new CreateAttributeValueRequest(
                    AttributeId: COLOR_ATTRIBUTE_ID,
                    Value: "Morado",
                    NumericValue: null,
                    BooleanValue: null,
                    DisplayOrder: 5,
                    IsActive: false),
                "inactive attribute values should be created successfully"
            };

            // Zero display order
            yield return new object[]
            {
                new CreateAttributeValueRequest(
                    AttributeId: COLOR_ATTRIBUTE_ID,
                    Value: "Rosa",
                    NumericValue: null,
                    BooleanValue: null,
                    DisplayOrder: 0,
                    IsActive: true),
                "zero display order should be accepted"
            };

            // High display order
            yield return new object[]
            {
                new CreateAttributeValueRequest(
                    AttributeId: SIZE_ATTRIBUTE_ID,
                    Value: "XXL",
                    NumericValue: null,
                    BooleanValue: null,
                    DisplayOrder: 999,
                    IsActive: true),
                "high display order should be created successfully"
            };
        }

        // Tests específicos para valores numéricos
        [Theory]
        [InlineData(128, "128 GB")]
        [InlineData(256, "256 GB")]
        [InlineData(512, "512 GB")]
        [InlineData(1024, "1 TB")]
        public async Task Should_ReturnOk_WhenCreatingStorageValueWithNumericValue(
            decimal numericValue,
            string displayValue)
        {
            // Arrange
            SetAdminAuthentication();
            CreateAttributeValueRequest request = new(
                AttributeId: STORAGE_ATTRIBUTE_ID,
                Value: $"{displayValue}-{Guid.NewGuid()}",
                NumericValue: numericValue,
                BooleanValue: null,
                DisplayOrder: 10,
                IsActive: true);

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, request);
            long? attributeValueId = await response.Content.ReadFromJsonAsync<long?>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            attributeValueId.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData(4, "4 GB")]
        [InlineData(8, "8 GB")]
        [InlineData(16, "16 GB")]
        [InlineData(32, "32 GB")]
        public async Task Should_ReturnOk_WhenCreatingRamValueWithNumericValue(
            decimal numericValue,
            string displayValue)
        {
            // Arrange
            SetAdminAuthentication();
            CreateAttributeValueRequest request = new(
                AttributeId: RAM_ATTRIBUTE_ID,
                Value: $"{displayValue}-{Guid.NewGuid()}",
                NumericValue: numericValue,
                BooleanValue: null,
                DisplayOrder: 10,
                IsActive: true);

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, request);
            long? attributeValueId = await response.Content.ReadFromJsonAsync<long?>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            attributeValueId.Should().BeGreaterThan(0);
        }

        // Tests para valores booleanos
        [Theory]
        [InlineData(true, "Yes")]
        [InlineData(false, "No")]
        public async Task Should_ReturnOk_WhenCreatingValueWithBooleanValue(
            bool booleanValue,
            string displayValue)
        {
            // Arrange
            SetAdminAuthentication();
            CreateAttributeValueRequest request = new(
                AttributeId: COLOR_ATTRIBUTE_ID, // Using any attribute for this test
                Value: $"{displayValue}-{Guid.NewGuid()}",
                NumericValue: null,
                BooleanValue: booleanValue,
                DisplayOrder: 10,
                IsActive: true);

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, request);
            long? attributeValueId = await response.Content.ReadFromJsonAsync<long?>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            attributeValueId.Should().BeGreaterThan(0);
        }

        // Tests con valores opcionales null
        [Fact]
        public async Task Should_ReturnOk_WhenNumericAndBooleanValuesAreNull()
        {
            // Arrange
            SetAdminAuthentication();
            CreateAttributeValueRequest request = new(
                AttributeId: COLOR_ATTRIBUTE_ID,
                Value: $"Naranja-{Guid.NewGuid()}",
                NumericValue: null,
                BooleanValue: null,
                DisplayOrder: 10,
                IsActive: true);

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, request);
            long? attributeValueId = await response.Content.ReadFromJsonAsync<long?>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            attributeValueId.Should().BeGreaterThan(0);
        }

        // Tests para diferentes atributos del seed
        [Theory]
        [InlineData(COLOR_ATTRIBUTE_ID, "Turquesa", "color attribute")]
        [InlineData(SIZE_ATTRIBUTE_ID, "XS", "size attribute")]
        [InlineData(MATERIAL_ATTRIBUTE_ID, "Vidrio", "material attribute")]
        public async Task Should_ReturnOk_WhenCreatingValuesForDifferentSeededAttributes(
            long attributeId,
            string value,
            string scenario)
        {
            // Arrange
            SetAdminAuthentication();
            CreateAttributeValueRequest request = new(
                AttributeId: attributeId,
                Value: $"{value}-{Guid.NewGuid()}",
                NumericValue: null,
                BooleanValue: null,
                DisplayOrder: 10,
                IsActive: true);

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, request);
            long? attributeValueId = await response.Content.ReadFromJsonAsync<long?>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK, because: scenario);
            attributeValueId.Should().BeGreaterThan(0);
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
            CreateAttributeValueRequest request = _request with 
            { 
                Value = $"Test-{Guid.NewGuid()}" 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, request);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserIsNotAdmin()
        {
            // Arrange
            SetCustomerUserAuthentication();
            CreateAttributeValueRequest request = _request with 
            { 
                Value = $"Test-{Guid.NewGuid()}" 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.AttributeValues.Base, request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

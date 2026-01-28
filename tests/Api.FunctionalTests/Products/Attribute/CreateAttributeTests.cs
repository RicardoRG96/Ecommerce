using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.Attribute.Create;

namespace Api.FunctionalTests.Products.Attribute
{
    public class CreateAttributeTests : BaseFunctionalTest
    {
        private static readonly CreateAttributeRequest _request = new(
            Code: "color",
            Name: "Color",
            Description: "Product color attribute for customization",
            DataType: "string",
            IsVariant: true,
            IsFilterable: true,
            IsRequired: false,
            DisplayOrder: 1,
            IsActive: true);

        public CreateAttributeTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        // Consolida tests similares usando Theory
        [Theory]
        [MemberData(nameof(GetInvalidMaxLengthRequests))]
        public async Task Should_ReturnBadRequest_WhenFieldExceedsMaximumLength(
            CreateAttributeRequest invalidRequest, 
            string fieldName)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Attributes.Base, invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, 
                because: $"{fieldName} exceeds maximum length");
        }

        public static IEnumerable<object[]> GetInvalidMaxLengthRequests()
        {
            yield return new object[] 
            { 
                _request with { Code = Constants.ExceededMaximumLengthField }, 
                nameof(CreateAttributeRequest.Code) 
            };
            yield return new object[] 
            { 
                _request with { Name = Constants.ExceededMaximumLengthField }, 
                nameof(CreateAttributeRequest.Name) 
            };
            yield return new object[] 
            { 
                _request with { Description = Constants.ExceededMaximumLengthField }, 
                nameof(CreateAttributeRequest.Description) 
            };
            yield return new object[] 
            { 
                _request with { DataType = Constants.ExceededMaximumLengthField }, 
                nameof(CreateAttributeRequest.DataType) 
            };
        }

        // Verifica mensajes de error específicos para campos requeridos
        [Fact]
        public async Task Should_ReturnBadRequest_WhenCodeIsMissing()
        {
            // Arrange
            SetAdminAuthentication();
            CreateAttributeRequest invalidRequest = _request with { Code = "" };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Attributes.Base, invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("Code is required", 
                because: "validation message should be clear for debugging");
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenNameIsMissing()
        {
            // Arrange
            SetAdminAuthentication();
            CreateAttributeRequest invalidRequest = _request with { Name = "" };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Attributes.Base, invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("Name is required", 
                because: "validation message should be clear for debugging");
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenDataTypeIsMissing()
        {
            // Arrange
            SetAdminAuthentication();
            CreateAttributeRequest invalidRequest = _request with { DataType = "" };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Attributes.Base, invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("DataType is required", 
                because: "validation message should be clear for debugging");
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
            CreateAttributeRequest invalidRequest = _request with { DisplayOrder = invalidDisplayOrder };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Attributes.Base, invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, 
                because: $"{fieldName} {reason}");

            content.Should().Contain("DisplayOrder must be greater than or equal to 0");
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAttributeCodeAlreadyExists()
        {
            // Arrange
            SetAdminAuthentication();
            string uniqueCode = $"size-{Guid.NewGuid()}"; // Usa GUID para evitar colisiones
            CreateAttributeRequest firstRequest = _request with { Code = uniqueCode };

            // Act - Create attribute first time
            HttpResponseMessage firstResponse = await HttpClient.PostAsJsonAsync(ApiRoutes.Attributes.Base, firstRequest);
            firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Act - Try to create attribute with same code
            CreateAttributeRequest duplicateRequest = _request with { Code = uniqueCode };
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Attributes.Base, duplicateRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("already exists", 
                because: "duplicate code should be clearly indicated");
        }

        // Verifica el contenido de la respuesta exitosa
        [Fact]
        public async Task Should_ReturnOk_WhenRequestIsValidWithAllFields()
        {
            // Arrange
            SetAdminAuthentication();
            CreateAttributeRequest request = _request with 
            { 
                Code = $"material-{Guid.NewGuid()}"
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Attributes.Base, request);
            long? attributeId = await response.Content.ReadFromJsonAsync<long?>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            attributeId.Should().BeGreaterThan(0, 
                because: "a valid attribute ID should be returned");
        }

        // Consolida casos exitosos con diferentes configuraciones
        [Theory]
        [MemberData(nameof(GetValidAttributeConfigurations))]
        public async Task Should_ReturnOk_WhenRequestIsValidWithDifferentConfigurations(
            CreateAttributeRequest validRequest,
            string scenario)
        {
            // Arrange
            SetAdminAuthentication();
            CreateAttributeRequest uniqueRequest = validRequest with 
            { 
                Code = $"{validRequest.Code}-{Guid.NewGuid()}" 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Attributes.Base, uniqueRequest);
            long? attributeId = await response.Content.ReadFromJsonAsync<long?>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK, because: scenario);
            attributeId.Should().BeGreaterThan(0);
        }

        public static IEnumerable<object[]> GetValidAttributeConfigurations()
        {
            yield return new object[]
            {
                new CreateAttributeRequest(
                    Code: "weight",
                    Name: "Weight",
                    Description: "",
                    DataType: "decimal",
                    IsVariant: false,
                    IsFilterable: true,
                    IsRequired: false,
                    DisplayOrder: 0,
                    IsActive: true),
                "minimal description should be accepted"
            };

            yield return new object[]
            {
                _request with 
                { 
                    Code = "brand-attr",
                    IsVariant = false 
                },
                "non-variant attributes should be created successfully"
            };

            yield return new object[]
            {
                _request with 
                { 
                    Code = "custom-field",
                    IsFilterable = false 
                },
                "non-filterable attributes should be created successfully"
            };

            yield return new object[]
            {
                _request with 
                { 
                    Code = "required-attr",
                    IsRequired = true 
                },
                "required attributes should be created successfully"
            };

            yield return new object[]
            {
                _request with 
                { 
                    Code = "inactive-attr",
                    IsActive = false 
                },
                "inactive attributes should be created successfully"
            };

            yield return new object[]
            {
                new CreateAttributeRequest(
                    Code: "integer-attr",
                    Name: "Integer Attribute",
                    Description: "An integer type attribute",
                    DataType: "integer",
                    IsVariant: false,
                    IsFilterable: true,
                    IsRequired: false,
                    DisplayOrder: 5,
                    IsActive: true),
                "integer data type attributes should be created successfully"
            };

            yield return new object[]
            {
                new CreateAttributeRequest(
                    Code: "boolean-attr",
                    Name: "Boolean Attribute",
                    Description: "A boolean type attribute",
                    DataType: "boolean",
                    IsVariant: false,
                    IsFilterable: true,
                    IsRequired: false,
                    DisplayOrder: 10,
                    IsActive: true),
                "boolean data type attributes should be created successfully"
            };

            yield return new object[]
            {
                new CreateAttributeRequest(
                    Code: "high-order",
                    Name: "High Display Order",
                    Description: "Attribute with high display order",
                    DataType: "string",
                    IsVariant: true,
                    IsFilterable: true,
                    IsRequired: false,
                    DisplayOrder: 999,
                    IsActive: true),
                "attributes with high display order should be created successfully"
            };
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
            CreateAttributeRequest request = _request with 
            { 
                Code = $"test-attr-{Guid.NewGuid()}" 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Attributes.Base, request);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserIsNotAdmin()
        {
            // Arrange
            SetCustomerUserAuthentication();
            CreateAttributeRequest request = _request with 
            { 
                Code = $"test-attr-{Guid.NewGuid()}" 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Attributes.Base, request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        // Tests adicionales para validar la combinación de flags booleanos
        [Theory]
        [MemberData(nameof(GetValidBooleanCombinations))]
        public async Task Should_ReturnOk_WhenBooleanFlagsCombinationsAreValid(
            bool isVariant,
            bool isFilterable,
            bool isRequired,
            bool isActive,
            string scenario)
        {
            // Arrange
            SetAdminAuthentication();
            CreateAttributeRequest request = _request with 
            { 
                Code = $"combo-{Guid.NewGuid()}",
                IsVariant = isVariant,
                IsFilterable = isFilterable,
                IsRequired = isRequired,
                IsActive = isActive
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Attributes.Base, request);
            long? attributeId = await response.Content.ReadFromJsonAsync<long?>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK, because: scenario);
            attributeId.Should().BeGreaterThan(0);
        }

        public static IEnumerable<object[]> GetValidBooleanCombinations()
        {
            yield return new object[] { true, true, true, true, "all flags true" };
            yield return new object[] { false, false, false, false, "all flags false" };
            yield return new object[] { true, false, true, false, "variant and required, but not filterable or active" };
            yield return new object[] { false, true, false, true, "filterable and active, but not variant or required" };
        }
    }
}

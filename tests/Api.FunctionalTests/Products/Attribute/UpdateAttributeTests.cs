using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.Attribute.Create;
using Web.Api.Endpoints.v1.Products.Attribute.Update;

namespace Api.FunctionalTests.Products.Attribute
{
    public class UpdateAttributeTests : BaseFunctionalTest
    {
        private static readonly UpdateAttributeRequest _request = new(
            Code: "color-updated",
            Name: "Color - Updated",
            Description: "Product color attribute for customization - Updated version",
            DataType: "string",
            IsVariant: true,
            IsFilterable: true,
            IsRequired: false,
            DisplayOrder: 2);

        public UpdateAttributeTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        // Consolida tests similares usando Theory
        [Theory]
        [MemberData(nameof(GetInvalidMaxLengthRequests))]
        public async Task Should_ReturnBadRequest_WhenFieldExceedsMaximumLength(
            UpdateAttributeRequest invalidRequest, 
            string fieldName)
        {
            // Arrange
            SetAdminAuthentication();
            long attributeId = await CreateTestAttribute();

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Attributes.Base}/{attributeId}", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, 
                because: $"{fieldName} exceeds maximum length");
        }

        public static IEnumerable<object[]> GetInvalidMaxLengthRequests()
        {
            yield return new object[] 
            { 
                _request with { Code = Constants.ExceededMaximumLengthField }, 
                nameof(UpdateAttributeRequest.Code) 
            };
            yield return new object[] 
            { 
                _request with { Name = Constants.ExceededMaximumLengthField }, 
                nameof(UpdateAttributeRequest.Name) 
            };
            yield return new object[] 
            { 
                _request with { Description = Constants.ExceededMaximumLengthField }, 
                nameof(UpdateAttributeRequest.Description) 
            };
            yield return new object[] 
            { 
                _request with { DataType = Constants.ExceededMaximumLengthField }, 
                nameof(UpdateAttributeRequest.DataType) 
            };
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

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Attributes.Base}/{invalidAttributeId}", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                because: $"{fieldName} {reason}");
        }

        // Verifica mensajes de error específicos
        [Fact]
        public async Task Should_ReturnBadRequest_WhenCodeIsMissing()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeId = await CreateTestAttribute();
            UpdateAttributeRequest invalidRequest = _request with { Code = "" };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Attributes.Base}/{attributeId}", invalidRequest);
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
            long attributeId = await CreateTestAttribute();
            UpdateAttributeRequest invalidRequest = _request with { Name = "" };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Attributes.Base}/{attributeId}", invalidRequest);
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
            long attributeId = await CreateTestAttribute();
            UpdateAttributeRequest invalidRequest = _request with { DataType = "" };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Attributes.Base}/{attributeId}", invalidRequest);
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
            long attributeId = await CreateTestAttribute();
            UpdateAttributeRequest invalidRequest = _request with { DisplayOrder = invalidDisplayOrder };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Attributes.Base}/{attributeId}", invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, 
                because: $"{fieldName} {reason}");
            content.Should().Contain("DisplayOrder must be greater than or equal to 0");
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenAttributeIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Attributes.Base}/{Constants.NotExistingId}", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAttributeCodeAlreadyExists()
        {
            // Arrange
            SetAdminAuthentication();
            string uniqueCode1 = $"size-{Guid.NewGuid()}";
            string uniqueCode2 = $"weight-{Guid.NewGuid()}";
            
            long attributeId1 = await CreateTestAttribute(uniqueCode1);
            long attributeId2 = await CreateTestAttribute(uniqueCode2);

            // Act - Try to change attribute2 code to attribute1's code
            UpdateAttributeRequest invalidRequest = _request with { Code = uniqueCode1 };
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Attributes.Base}/{attributeId2}", invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("already exists", 
                because: "duplicate code should be clearly indicated");
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenRequestIsValid()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeId = await CreateTestAttribute();

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Attributes.Base}/{attributeId}", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_AllowKeepingSameCode_WhenUpdatingSameAttribute()
        {
            // Arrange
            SetAdminAuthentication();
            string attributeCode = $"unique-code-{Guid.NewGuid()}";
            long attributeId = await CreateTestAttribute(attributeCode);

            // Act - Update attribute keeping the same code
            UpdateAttributeRequest sameCodeRequest = _request with { Code = attributeCode };
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Attributes.Base}/{attributeId}", sameCodeRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        // Consolida casos exitosos con diferentes configuraciones
        [Theory]
        [MemberData(nameof(GetValidAttributeUpdateConfigurations))]
        public async Task Should_ReturnNoContent_WhenRequestIsValidWithDifferentConfigurations(
            UpdateAttributeRequest validRequest,
            string scenario)
        {
            // Arrange
            SetAdminAuthentication();
            long attributeId = await CreateTestAttribute();

            UpdateAttributeRequest uniqueRequest = validRequest with
            {
                Code = $"unique-{Guid.NewGuid()}"
            };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Attributes.Base}/{attributeId}", uniqueRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent, because: scenario);
        }

        public static IEnumerable<object[]> GetValidAttributeUpdateConfigurations()
        {
            yield return new object[]
            {
                new UpdateAttributeRequest(
                    Code: "updated-code",
                    Name: "Updated Name",
                    Description: "",
                    DataType: "string",
                    IsVariant: false,
                    IsFilterable: false,
                    IsRequired: false,
                    DisplayOrder: 0),
                "minimal description should be accepted"
            };

            yield return new object[]
            {
                _request with 
                { 
                    IsVariant = false 
                },
                "changing to non-variant should be successful"
            };

            yield return new object[]
            {
                _request with 
                { 
                    IsFilterable = false 
                },
                "changing to non-filterable should be successful"
            };

            yield return new object[]
            {
                _request with 
                { 
                    IsRequired = true 
                },
                "changing to required should be successful"
            };

            yield return new object[]
            {
                _request with 
                { 
                    DataType = "integer" 
                },
                "changing data type should be successful"
            };

            yield return new object[]
            {
                _request with 
                { 
                    DisplayOrder = 999 
                },
                "changing display order to high value should be successful"
            };

            yield return new object[]
            {
                new UpdateAttributeRequest(
                    Code: "decimal-type",
                    Name: "Decimal Attribute",
                    Description: "A decimal type attribute updated",
                    DataType: "decimal",
                    IsVariant: false,
                    IsFilterable: true,
                    IsRequired: false,
                    DisplayOrder: 5),
                "decimal data type should be updated successfully"
            };

            yield return new object[]
            {
                new UpdateAttributeRequest(
                    Code: "boolean-type",
                    Name: "Boolean Attribute",
                    Description: "A boolean type attribute updated",
                    DataType: "boolean",
                    IsVariant: false,
                    IsFilterable: true,
                    IsRequired: true,
                    DisplayOrder: 10),
                "boolean data type with required flag should be updated successfully"
            };
        }

        // Tests adicionales para validar cambios en combinaciones de flags booleanos
        [Theory]
        [MemberData(nameof(GetValidBooleanCombinations))]
        public async Task Should_ReturnNoContent_WhenBooleanFlagsCombinationsAreValid(
            bool isVariant,
            bool isFilterable,
            bool isRequired,
            string scenario)
        {
            // Arrange
            SetAdminAuthentication();
            long attributeId = await CreateTestAttribute();
            
            UpdateAttributeRequest request = _request with 
            { 
                Code = $"combo-{Guid.NewGuid()}",
                IsVariant = isVariant,
                IsFilterable = isFilterable,
                IsRequired = isRequired
            };

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Attributes.Base}/{attributeId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent, because: scenario);
        }

        public static IEnumerable<object[]> GetValidBooleanCombinations()
        {
            yield return new object[] { true, true, true, "all flags true" };
            yield return new object[] { false, false, false, "all flags false" };
            yield return new object[] { true, false, true, "variant and required, but not filterable" };
            yield return new object[] { false, true, false, "filterable only" };
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
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Attributes.Base}/1", _request);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserIsNotAdmin()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Attributes.Base}/1", _request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        // Helper privado para crear atributos de prueba
        private async Task<long> CreateTestAttribute(string? code = null)
        {
            CreateAttributeRequest createRequest = new(
                Code: code ?? $"test-attr-{Guid.NewGuid()}",
                Name: "Test Attribute",
                Description: "Test attribute description",
                DataType: "string",
                IsVariant: true,
                IsFilterable: true,
                IsRequired: false,
                DisplayOrder: 1,
                IsActive: true);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Attributes.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long? attributeId = await response.Content.ReadFromJsonAsync<long?>();
            return attributeId!.Value;
        }
    }
}

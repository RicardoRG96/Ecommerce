using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Products.Attributes.GetById;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.Attribute.Create;

namespace Api.FunctionalTests.Products.Attribute
{
    public class GetAttributeByIdTests : BaseFunctionalTest
    {
        public GetAttributeByIdTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Theory]
        [InlineData(0, "AttributeId", "is missing")]
        [InlineData(-1, "AttributeId", "is negative")]
        public async Task Should_ReturnNotFound_WhenAttributeIdIsInvalid(
            long invalidAttributeId,
            string fieldName,
            string reason)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Attributes.Base}/{invalidAttributeId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                because: $"{fieldName} {reason}");
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenAttributeIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Attributes.Base}/{Constants.NotExistingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_AndAttribute_WhenAttributeExists()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeId = await CreateTestAttribute("test-color", "Color Test", "string");

            // Act
            AttributeResponse? attribute = await HttpClient.GetFromJsonAsync<AttributeResponse>(
                $"{ApiRoutes.Attributes.Base}/{attributeId}");

            // Assert
            attribute.Should().NotBeNull();
            attribute!.Id.Should().Be(attributeId);
            attribute.Code.Should().Contain("test-color");
            attribute.Name.Should().Be("Color Test");
            attribute.DataType.Should().Be("string");
        }

        [Fact]
        public async Task Should_ReturnAttributeWithAllFields()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeId = await CreateTestAttribute(
                code: "size-attr",
                name: "Size Attribute",
                dataType: "string",
                description: "Product size attribute for variants",
                isVariant: true,
                isFilterable: true,
                isRequired: false,
                displayOrder: 5,
                isActive: true);

            // Act
            AttributeResponse? attribute = await HttpClient.GetFromJsonAsync<AttributeResponse>(
                $"{ApiRoutes.Attributes.Base}/{attributeId}");

            // Assert
            attribute.Should().NotBeNull();
            attribute!.Id.Should().Be(attributeId);
            attribute.Code.Should().Contain("size-attr");
            attribute.Name.Should().Be("Size Attribute");
            attribute.Description.Should().Be("Product size attribute for variants");
            attribute.DataType.Should().Be("string");
            attribute.IsVariant.Should().BeTrue();
            attribute.IsFilterable.Should().BeTrue();
            attribute.IsRequired.Should().BeFalse();
            attribute.DisplayOrder.Should().Be(5);
            attribute.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Should_ReturnActiveAttribute()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeId = await CreateTestAttribute(
                code: "active-attr",
                name: "Active Attribute",
                dataType: "string",
                isActive: true);

            // Act
            AttributeResponse? attribute = await HttpClient.GetFromJsonAsync<AttributeResponse>(
                $"{ApiRoutes.Attributes.Base}/{attributeId}");

            // Assert
            attribute.Should().NotBeNull();
            attribute!.IsActive.Should().BeTrue(
                because: "the attribute was created as active");
        }

        [Fact]
        public async Task Should_ReturnInactiveAttribute()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeId = await CreateTestAttribute(
                code: "inactive-attr",
                name: "Inactive Attribute",
                dataType: "string",
                isActive: false);

            // Act
            AttributeResponse? attribute = await HttpClient.GetFromJsonAsync<AttributeResponse>(
                $"{ApiRoutes.Attributes.Base}/{attributeId}");

            // Assert
            attribute.Should().NotBeNull();
            attribute!.IsActive.Should().BeFalse(
                because: "the attribute was created as inactive");
        }

        [Theory]
        [MemberData(nameof(GetDifferentDataTypes))]
        public async Task Should_ReturnAttribute_WithDifferentDataTypes(
            string dataType,
            string scenario)
        {
            // Arrange
            SetAdminAuthentication();
            long attributeId = await CreateTestAttribute(
                code: $"attr-{dataType}",
                name: $"Attribute {dataType}",
                dataType: dataType);

            // Act
            AttributeResponse? attribute = await HttpClient.GetFromJsonAsync<AttributeResponse>(
                $"{ApiRoutes.Attributes.Base}/{attributeId}");

            // Assert
            attribute.Should().NotBeNull();
            attribute!.DataType.Should().Be(dataType, because: scenario);
        }

        public static IEnumerable<object[]> GetDifferentDataTypes()
        {
            yield return new object[] { "string", "string data type should be returned correctly" };
            yield return new object[] { "integer", "integer data type should be returned correctly" };
            yield return new object[] { "decimal", "decimal data type should be returned correctly" };
            yield return new object[] { "boolean", "boolean data type should be returned correctly" };
        }

        [Theory]
        [MemberData(nameof(GetDifferentBooleanFlags))]
        public async Task Should_ReturnAttribute_WithDifferentBooleanFlags(
            bool isVariant,
            bool isFilterable,
            bool isRequired,
            string scenario)
        {
            // Arrange
            SetAdminAuthentication();
            long attributeId = await CreateTestAttribute(
                code: "flags",
                name: "Flags Test Attribute",
                dataType: "string",
                isVariant: isVariant,
                isFilterable: isFilterable,
                isRequired: isRequired);

            // Act
            AttributeResponse? attribute = await HttpClient.GetFromJsonAsync<AttributeResponse>(
                $"{ApiRoutes.Attributes.Base}/{attributeId}");

            // Assert
            attribute.Should().NotBeNull();
            attribute!.IsVariant.Should().Be(isVariant);
            attribute.IsFilterable.Should().Be(isFilterable);
            attribute.IsRequired.Should().Be(isRequired, because: scenario);
        }

        public static IEnumerable<object[]> GetDifferentBooleanFlags()
        {
            yield return new object[] { true, true, true, "all flags true" };
            yield return new object[] { false, false, false, "all flags false" };
            yield return new object[] { true, false, true, "variant and required only" };
            yield return new object[] { false, true, false, "filterable only" };
        }

        [Theory]
        [InlineData(0, "zero display order")]
        [InlineData(1, "low display order")]
        [InlineData(100, "medium display order")]
        [InlineData(999, "high display order")]
        public async Task Should_ReturnAttribute_WithDifferentDisplayOrders(
            int displayOrder,
            string scenario)
        {
            // Arrange
            SetAdminAuthentication();
            long attributeId = await CreateTestAttribute(
                code: $"order-{displayOrder}",
                name: $"Order {displayOrder}",
                dataType: "string",
                displayOrder: displayOrder);

            // Act
            AttributeResponse? attribute = await HttpClient.GetFromJsonAsync<AttributeResponse>(
                $"{ApiRoutes.Attributes.Base}/{attributeId}");

            // Assert
            attribute.Should().NotBeNull();
            attribute!.DisplayOrder.Should().Be(displayOrder, because: scenario);
        }

        [Fact]
        public async Task Should_ReturnAttribute_WithEmptyDescription()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeId = await CreateTestAttribute(
                code: "no-desc",
                name: "No Description",
                dataType: "string",
                description: "");

            // Act
            AttributeResponse? attribute = await HttpClient.GetFromJsonAsync<AttributeResponse>(
                $"{ApiRoutes.Attributes.Base}/{attributeId}");

            // Assert
            attribute.Should().NotBeNull();
            attribute!.Description.Should().BeEmpty(
                because: "attribute was created without description");
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
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Attributes.Base}/1");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserDoesNotHavePermission()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Attributes.Base}/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        // Helper method para crear atributos de prueba
        private async Task<long> CreateTestAttribute(
            string code,
            string name,
            string dataType,
            string description = "Test description",
            bool isVariant = false,
            bool isFilterable = false,
            bool isRequired = false,
            int displayOrder = 1,
            bool isActive = true)
        {
            CreateAttributeRequest createRequest = new(
                Code: $"{code}-{Guid.NewGuid()}",
                Name: name,
                Description: description,
                DataType: dataType,
                IsVariant: isVariant,
                IsFilterable: isFilterable,
                IsRequired: isRequired,
                DisplayOrder: displayOrder,
                IsActive: isActive);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Attributes.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long? attributeId = await response.Content.ReadFromJsonAsync<long?>();
            return attributeId!.Value;
        }
    }
}

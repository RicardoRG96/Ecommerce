using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.Attribute.Create;

namespace Api.FunctionalTests.Products.Attribute
{
    public class DeactivateAttributeTests : BaseFunctionalTest
    {
        public DeactivateAttributeTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
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
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Attributes.Base}/{invalidAttributeId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                because: $"{fieldName} {reason}");
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenAttributeIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Attributes.Base}/{Constants.NotExistingId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenAttributeIdExists()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeId = await CreateActiveAttribute();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Attributes.Base}/{attributeId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenAttributeIsAlreadyInactive()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeId = await CreateActiveAttribute();

            // Act - First deactivation
            await HttpClient.PatchAsync($"{ApiRoutes.Attributes.Base}/{attributeId}/deactivate", null!);

            // Act - Second deactivation (the attribute is already inactive)
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Attributes.Base}/{attributeId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "deactivating an already inactive attribute should be idempotent");
        }

        [Fact]
        public async Task Should_DeactivateActiveAttribute_Successfully()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeId = await CreateActiveAttribute();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Attributes.Base}/{attributeId}/deactivate", null!);

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
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Attributes.Base}/1/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserIsNotAdmin()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Attributes.Base}/1/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Theory]
        [MemberData(nameof(GetValidAttributeConfigurations))]
        public async Task Should_DeactivateAttribute_WithDifferentConfigurations(
            bool isVariant,
            bool isFilterable,
            bool isRequired,
            string dataType,
            string scenario)
        {
            // Arrange
            SetAdminAuthentication();
            long attributeId = await CreateActiveAttribute(
                isVariant: isVariant, 
                isFilterable: isFilterable, 
                isRequired: isRequired, 
                dataType: dataType);

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Attributes.Base}/{attributeId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent, because: scenario);
        }

        public static IEnumerable<object[]> GetValidAttributeConfigurations()
        {
            yield return new object[] { true, true, true, "string", "variant, filterable and required attribute" };
            yield return new object[] { false, false, false, "string", "non-variant, non-filterable and optional attribute" };
            yield return new object[] { true, false, false, "integer", "variant integer attribute" };
            yield return new object[] { false, true, true, "decimal", "filterable and required decimal attribute" };
            yield return new object[] { true, true, false, "boolean", "variant and filterable boolean attribute" };
        }

        [Fact]
        public async Task Should_DeactivateInactiveAttribute_Successfully()
        {
            // Arrange
            SetAdminAuthentication();
            long attributeId = await CreateInactiveAttribute();

            // Act
            HttpResponseMessage response = await HttpClient.PatchAsync($"{ApiRoutes.Attributes.Base}/{attributeId}/deactivate", null!);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "deactivating an inactive attribute should be idempotent");
        }

        // Helper methods para crear atributos de prueba
        private async Task<long> CreateActiveAttribute(
            bool isVariant = true, 
            bool isFilterable = true, 
            bool isRequired = false, 
            string dataType = "string")
        {
            CreateAttributeRequest createRequest = new(
                Code: $"active-attr-{Guid.NewGuid()}",
                Name: "Active Test Attribute",
                Description: "Active test attribute description",
                DataType: dataType,
                IsVariant: isVariant,
                IsFilterable: isFilterable,
                IsRequired: isRequired,
                DisplayOrder: 1,
                IsActive: true);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Attributes.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long? attributeId = await response.Content.ReadFromJsonAsync<long?>();
            return attributeId!.Value;
        }

        private async Task<long> CreateInactiveAttribute()
        {
            CreateAttributeRequest createRequest = new(
                Code: $"inactive-attr-{Guid.NewGuid()}",
                Name: "Inactive Test Attribute",
                Description: "Inactive test attribute description",
                DataType: "string",
                IsVariant: false,
                IsFilterable: false,
                IsRequired: false,
                DisplayOrder: 1,
                IsActive: false);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Attributes.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long? attributeId = await response.Content.ReadFromJsonAsync<long?>();
            return attributeId!.Value;
        }
    }
}

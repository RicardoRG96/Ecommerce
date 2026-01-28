using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Products.Attributes.GetAll;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.Attribute.Create;

namespace Api.FunctionalTests.Products.Attribute
{
    public class GetAllAttributesTests : BaseFunctionalTest
    {
        #region Seed Data Constants

        /// <summary>
        /// Attribute ID 1: Color
        /// - Code: color
        /// - DataType: String
        /// - IsVariant: true, IsFilterable: true, IsRequired: true
        /// - DisplayOrder: 1, IsActive: true
        /// </summary>
        private const long COLOR_ATTRIBUTE_ID = 1;

        /// <summary>
        /// Attribute ID 2: Almacenamiento (Storage)
        /// - Code: storage
        /// - DataType: Number
        /// - IsVariant: true, IsFilterable: true, IsRequired: true
        /// - DisplayOrder: 2, IsActive: true
        /// </summary>
        private const long STORAGE_ATTRIBUTE_ID = 2;

        /// <summary>
        /// Attribute ID 3: Memoria RAM
        /// - Code: ram
        /// - DataType: Number
        /// - IsVariant: true, IsFilterable: true, IsRequired: true
        /// - DisplayOrder: 3, IsActive: true
        /// </summary>
        private const long RAM_ATTRIBUTE_ID = 3;

        /// <summary>
        /// Attribute ID 4: Tamaño/Talla (Size)
        /// - Code: size
        /// - DataType: String
        /// - IsVariant: true, IsFilterable: true, IsRequired: true
        /// - DisplayOrder: 4, IsActive: true
        /// </summary>
        private const long SIZE_ATTRIBUTE_ID = 4;

        /// <summary>
        /// Attribute ID 5: Material
        /// - Code: material
        /// - DataType: String
        /// - IsVariant: false, IsFilterable: true, IsRequired: false
        /// - DisplayOrder: 5, IsActive: true
        /// </summary>
        private const long MATERIAL_ATTRIBUTE_ID = 5;

        #endregion

        public GetAllAttributesTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnOk_WhenAttributesExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Attributes.Base}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_ReturnListOfAttributes_WhenAttributesExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeResponse>? attributes = 
                await HttpClient.GetFromJsonAsync<List<AttributeResponse>>($"{ApiRoutes.Attributes.Base}");

            // Assert
            attributes.Should().NotBeNull();
            attributes.Should().NotBeEmpty();
            attributes.Should().HaveCountGreaterThanOrEqualTo(5,
                because: "seed data contains at least 5 attributes");
        }

        [Fact]
        public async Task Should_ReturnExpectedNumberOfSeededAttributes()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeResponse>? attributes = 
                await HttpClient.GetFromJsonAsync<List<AttributeResponse>>($"{ApiRoutes.Attributes.Base}");

            // Assert - According to seed data: color, storage, ram, size, material = 5 attributes
            attributes.Should().NotBeNull();
            attributes.Should().HaveCount(5,
                because: "seed data contains exactly 5 attributes");
        }

        [Fact]
        public async Task Should_ReturnAttributesWithAllProperties()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeResponse>? attributes = 
                await HttpClient.GetFromJsonAsync<List<AttributeResponse>>($"{ApiRoutes.Attributes.Base}");

            // Assert
            attributes.Should().NotBeNull();
            attributes.Should().NotBeEmpty();

            // Verify Color attribute (ID 1)
            AttributeResponse? colorAttribute = attributes!.FirstOrDefault(a => a.Id == COLOR_ATTRIBUTE_ID);
            colorAttribute.Should().NotBeNull();
            colorAttribute!.Id.Should().Be(COLOR_ATTRIBUTE_ID);
            colorAttribute.Code.Should().Be("color");
            colorAttribute.Name.Should().Be("Color");
            colorAttribute.Description.Should().Be("Color del producto");
            colorAttribute.DataType.Should().Be("String");
            colorAttribute.IsVariant.Should().BeTrue();
            colorAttribute.IsFilterable.Should().BeTrue();
            colorAttribute.IsRequired.Should().BeTrue();
            colorAttribute.DisplayOrder.Should().Be(1);
            colorAttribute.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Should_ReturnAllSeededAttributes()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeResponse>? attributes = 
                await HttpClient.GetFromJsonAsync<List<AttributeResponse>>($"{ApiRoutes.Attributes.Base}");

            // Assert
            attributes.Should().NotBeNull();
            attributes.Should().Contain(a => a.Code == "color" && a.Name == "Color");
            attributes.Should().Contain(a => a.Code == "storage" && a.Name == "Almacenamiento");
            attributes.Should().Contain(a => a.Code == "ram" && a.Name == "Memoria RAM");
            attributes.Should().Contain(a => a.Code == "size" && a.Name == "Tamaño");
            attributes.Should().Contain(a => a.Code == "material" && a.Name == "Material");
        }

        [Theory]
        [InlineData(COLOR_ATTRIBUTE_ID, "color", "Color", "String")]
        [InlineData(STORAGE_ATTRIBUTE_ID, "storage", "Almacenamiento", "Number")]
        [InlineData(RAM_ATTRIBUTE_ID, "ram", "Memoria RAM", "Number")]
        [InlineData(SIZE_ATTRIBUTE_ID, "size", "Tamaño", "String")]
        [InlineData(MATERIAL_ATTRIBUTE_ID, "material", "Material", "String")]
        public async Task Should_ReturnSeededAttribute_WithCorrectProperties(
            long attributeId,
            string expectedCode,
            string expectedName,
            string expectedDataType)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeResponse>? attributes = 
                await HttpClient.GetFromJsonAsync<List<AttributeResponse>>($"{ApiRoutes.Attributes.Base}");

            // Assert
            attributes.Should().NotBeNull();
            AttributeResponse? attribute = attributes!.FirstOrDefault(a => a.Id == attributeId);
            attribute.Should().NotBeNull();
            attribute!.Code.Should().Be(expectedCode);
            attribute.Name.Should().Be(expectedName);
            attribute.DataType.Should().Be(expectedDataType);
        }

        [Fact]
        public async Task Should_ReturnAttributes_WithStringDataType()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeResponse>? attributes = 
                await HttpClient.GetFromJsonAsync<List<AttributeResponse>>($"{ApiRoutes.Attributes.Base}");

            // Assert
            attributes.Should().NotBeNull();
            List<AttributeResponse> stringAttributes = attributes!.Where(a => a.DataType == "String").ToList();
            stringAttributes.Should().HaveCount(3,
                because: "color, size, and material are String type");
            stringAttributes.Should().Contain(a => a.Code == "color");
            stringAttributes.Should().Contain(a => a.Code == "size");
            stringAttributes.Should().Contain(a => a.Code == "material");
        }

        [Fact]
        public async Task Should_ReturnAttributes_WithNumberDataType()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeResponse>? attributes = 
                await HttpClient.GetFromJsonAsync<List<AttributeResponse>>($"{ApiRoutes.Attributes.Base}");

            // Assert
            attributes.Should().NotBeNull();
            List<AttributeResponse> numberAttributes = attributes!.Where(a => a.DataType == "Number").ToList();
            numberAttributes.Should().HaveCount(2,
                because: "storage and ram are Number type");
            numberAttributes.Should().Contain(a => a.Code == "storage");
            numberAttributes.Should().Contain(a => a.Code == "ram");
        }

        [Fact]
        public async Task Should_ReturnAttributes_WithVariantFlagTrue()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeResponse>? attributes = 
                await HttpClient.GetFromJsonAsync<List<AttributeResponse>>($"{ApiRoutes.Attributes.Base}");

            // Assert
            attributes.Should().NotBeNull();
            List<AttributeResponse> variantAttributes = attributes!.Where(a => a.IsVariant).ToList();
            variantAttributes.Should().HaveCount(4,
                because: "color, storage, ram, and size are variants");
            variantAttributes.Should().Contain(a => a.Code == "color");
            variantAttributes.Should().Contain(a => a.Code == "storage");
            variantAttributes.Should().Contain(a => a.Code == "ram");
            variantAttributes.Should().Contain(a => a.Code == "size");
        }

        [Fact]
        public async Task Should_ReturnAttributes_WithVariantFlagFalse()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeResponse>? attributes = 
                await HttpClient.GetFromJsonAsync<List<AttributeResponse>>($"{ApiRoutes.Attributes.Base}");

            // Assert
            attributes.Should().NotBeNull();
            List<AttributeResponse> nonVariantAttributes = attributes!.Where(a => !a.IsVariant).ToList();
            nonVariantAttributes.Should().HaveCount(1,
                because: "only material is not a variant");
            nonVariantAttributes.Should().Contain(a => a.Code == "material");
        }

        [Fact]
        public async Task Should_ReturnAttributes_AllAreFilterable()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeResponse>? attributes = 
                await HttpClient.GetFromJsonAsync<List<AttributeResponse>>($"{ApiRoutes.Attributes.Base}");

            // Assert
            attributes.Should().NotBeNull();
            attributes.Should().AllSatisfy(a => a.IsFilterable.Should().BeTrue(),
                because: "all seeded attributes are filterable");
        }

        [Fact]
        public async Task Should_ReturnAttributes_WithRequiredFlagTrue()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeResponse>? attributes = 
                await HttpClient.GetFromJsonAsync<List<AttributeResponse>>($"{ApiRoutes.Attributes.Base}");

            // Assert
            attributes.Should().NotBeNull();
            List<AttributeResponse> requiredAttributes = attributes!.Where(a => a.IsRequired).ToList();
            requiredAttributes.Should().HaveCount(4,
                because: "color, storage, ram, and size are required");
            requiredAttributes.Should().Contain(a => a.Code == "color");
            requiredAttributes.Should().Contain(a => a.Code == "storage");
            requiredAttributes.Should().Contain(a => a.Code == "ram");
            requiredAttributes.Should().Contain(a => a.Code == "size");
        }

        [Fact]
        public async Task Should_ReturnAttributes_WithRequiredFlagFalse()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeResponse>? attributes = 
                await HttpClient.GetFromJsonAsync<List<AttributeResponse>>($"{ApiRoutes.Attributes.Base}");

            // Assert
            attributes.Should().NotBeNull();
            List<AttributeResponse> optionalAttributes = attributes!.Where(a => !a.IsRequired).ToList();
            optionalAttributes.Should().HaveCount(1,
                because: "only material is optional");
            optionalAttributes.Should().Contain(a => a.Code == "material");
        }

        [Fact]
        public async Task Should_ReturnAttributes_AllAreActive()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeResponse>? attributes = 
                await HttpClient.GetFromJsonAsync<List<AttributeResponse>>($"{ApiRoutes.Attributes.Base}");

            // Assert
            attributes.Should().NotBeNull();
            attributes.Should().AllSatisfy(a => a.IsActive.Should().BeTrue(),
                because: "all seeded attributes are active");
        }

        [Fact]
        public async Task Should_ReturnAttributes_OrderedByDisplayOrder()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeResponse>? attributes = 
                await HttpClient.GetFromJsonAsync<List<AttributeResponse>>($"{ApiRoutes.Attributes.Base}");

            // Assert
            attributes.Should().NotBeNull();
            attributes.Should().NotBeEmpty();

            // Verify that attributes are ordered by DisplayOrder (1 to 5)
            attributes![0].Code.Should().Be("color");
            attributes[0].DisplayOrder.Should().Be(1);
            attributes[1].Code.Should().Be("storage");
            attributes[1].DisplayOrder.Should().Be(2);
            attributes[2].Code.Should().Be("ram");
            attributes[2].DisplayOrder.Should().Be(3);
            attributes[3].Code.Should().Be("size");
            attributes[3].DisplayOrder.Should().Be(4);
            attributes[4].Code.Should().Be("material");
            attributes[4].DisplayOrder.Should().Be(5);
        }

        [Fact]
        public async Task Should_ReturnMaterialAttribute_AsNonVariantAndOptional()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<AttributeResponse>? attributes = 
                await HttpClient.GetFromJsonAsync<List<AttributeResponse>>($"{ApiRoutes.Attributes.Base}");

            // Assert
            attributes.Should().NotBeNull();
            AttributeResponse? materialAttribute = attributes!.FirstOrDefault(a => a.Code == "material");
            materialAttribute.Should().NotBeNull();
            materialAttribute!.IsVariant.Should().BeFalse(
                because: "material is not used for product variants");
            materialAttribute.IsRequired.Should().BeFalse(
                because: "material is optional");
            materialAttribute.IsFilterable.Should().BeTrue(
                because: "material can be used for filtering");
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
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Attributes.Base}");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserDoesNotHavePermission()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Attributes.Base}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        // Helper method para crear atributos de prueba adicionales
        private async Task<long> CreateTestAttribute(
            string code,
            string name,
            string dataType,
            string description = "Test description",
            bool isVariant = false,
            bool isFilterable = false,
            bool isRequired = true,
            int displayOrder = 99,
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

using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.Product.Create;

namespace Api.FunctionalTests.Products.Product
{
    public class CreateProductTests : BaseFunctionalTest
    {
        private static readonly CreateProductRequest _request = new(
            Name: "PlayStation 5 Console",
            Description: "The PlayStation 5 console unleashes new gaming possibilities that you never anticipated. Experience lightning-fast loading with an ultra-high speed SSD, deeper immersion with support for haptic feedback, adaptive triggers and 3D Audio, and an all-new generation of incredible PlayStation games.",
            ShortDescription: "Next-gen gaming console with ultra-high speed SSD and stunning visuals",
            BrandId: 1,
            CategoryId: 1,
            ProductTaxCategoryId: 1,
            IsActive: true,
            IsFeatured: true,
            IsDigital: false,
            MetaTitle: "PlayStation 5 Console - Next Gen Gaming",
            MetaDescription: "Buy PlayStation 5 console with ultra-high speed SSD and stunning 4K graphics",
            MetaKeywords: "PS5, PlayStation 5, gaming console, next-gen");

        public CreateProductTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        // Consolida tests similares usando Theory
        [Theory]
        [MemberData(nameof(GetInvalidMaxLengthRequests))]
        public async Task Should_ReturnBadRequest_WhenFieldExceedsMaximumLength(
            CreateProductRequest invalidRequest, 
            string fieldName)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Products.Base, invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, 
                because: $"{fieldName} exceeds maximum length");
        }

        public static IEnumerable<object[]> GetInvalidMaxLengthRequests()
        {
            yield return new object[] 
            { 
                _request with { Name = Constants.ExceededMaximumLengthField }, 
                nameof(CreateProductRequest.Name) 
            };
            yield return new object[] 
            { 
                _request with { ShortDescription = Constants.ExceededMaximumLengthField }, 
                nameof(CreateProductRequest.ShortDescription) 
            };
            yield return new object[] 
            { 
                _request with { MetaTitle = Constants.ExceededMaximumLengthField }, 
                nameof(CreateProductRequest.MetaTitle) 
            };
            yield return new object[] 
            { 
                _request with { MetaDescription = Constants.ExceededMaximumLengthField }, 
                nameof(CreateProductRequest.MetaDescription) 
            };
            yield return new object[] 
            { 
                _request with { MetaKeywords = Constants.ExceededMaximumLengthField }, 
                nameof(CreateProductRequest.MetaKeywords) 
            };
        }

        // Consolida validaciones de IDs
        [Theory]
        [InlineData(0, "BrandId", "is missing")]
        [InlineData(-1, "BrandId", "is negative")]
        public async Task Should_ReturnBadRequest_WhenBrandIdIsInvalid(
            long invalidBrandId, 
            string fieldName, 
            string reason)
        {
            // Arrange
            SetAdminAuthentication();
            CreateProductRequest invalidRequest = _request with { BrandId = invalidBrandId };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Products.Base, invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, 
                because: $"{fieldName} {reason}");
        }

        [Theory]
        [InlineData(0, "CategoryId", "is missing")]
        [InlineData(-1, "CategoryId", "is negative")]
        public async Task Should_ReturnBadRequest_WhenCategoryIdIsInvalid(
            long invalidCategoryId, 
            string fieldName,
            string reason)
        {
            // Arrange
            SetAdminAuthentication();
            CreateProductRequest invalidRequest = _request with { CategoryId = invalidCategoryId };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Products.Base, invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                because: $"{fieldName} {reason}");
        }

        [Theory]
        [InlineData(0, "ProductTaxCategoryId", "is missing")]
        [InlineData(-1, "ProductTaxCategoryId", "is negative")]
        public async Task Should_ReturnBadRequest_WhenProductTaxCategoryIdIsInvalid(
            long invalidId, 
            string fieldName,
            string reason)
        {
            // Arrange
            SetAdminAuthentication();
            CreateProductRequest invalidRequest = _request with { ProductTaxCategoryId = invalidId };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Products.Base, invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                because: $"{fieldName} {reason}");
        }

        // Verifica mensajes de error específicos
        [Fact]
        public async Task Should_ReturnBadRequest_WhenNameIsMissing()
        {
            // Arrange
            SetAdminAuthentication();
            CreateProductRequest invalidRequest = _request with { Name = "" };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Products.Base, invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("Product name is required", 
                because: "validation message should be clear for debugging");
        }

        // Verifica entidades relacionadas inexistentes con Theory
        [Theory]
        [MemberData(nameof(GetNonExistentRelatedEntityRequests))]
        public async Task Should_ReturnNotFound_WhenRelatedEntityDoesNotExist(
            CreateProductRequest invalidRequest,
            string entityName)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Products.Base, invalidRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().Contain(entityName, 
                because: $"error should indicate which {entityName} was not found");
        }

        public static IEnumerable<object[]> GetNonExistentRelatedEntityRequests()
        {
            yield return new object[] { _request with { BrandId = 999999 }, "Brand" };
            yield return new object[] { _request with { CategoryId = 999999 }, "Category" };
            yield return new object[] { _request with { ProductTaxCategoryId = 999999 }, "ProductTaxCategory" };
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenProductNameAlreadyExists()
        {
            // Arrange
            SetAdminAuthentication();
            string uniqueName = $"iPhone 17 Pro {Guid.NewGuid()}"; // ✅ MEJORA: Usa GUID para evitar colisiones
            CreateProductRequest firstRequest = _request with { Name = uniqueName };

            // Act - Create product first time
            HttpResponseMessage firstResponse = await HttpClient.PostAsJsonAsync(ApiRoutes.Products.Base, firstRequest);
            firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Act - Try to create product with same name
            CreateProductRequest duplicateRequest = _request with { Name = uniqueName };
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Products.Base, duplicateRequest);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain("already exists", 
                because: "duplicate name should be clearly indicated");
        }

        // Verifica el contenido de la respuesta exitosa
        [Fact]
        public async Task Should_ReturnOk_WhenRequestIsValidWithAllFields()
        {
            // Arrange
            SetAdminAuthentication();
            CreateProductRequest request = _request with 
            { 
                Name = $"PlayStation 5 Console {Guid.NewGuid()}" // ✅ Único para cada ejecución
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Products.Base, request);
            long? productId = await response.Content.ReadFromJsonAsync<long?>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            productId.Should().BeGreaterThan(0, 
                because: "a valid product ID should be returned");
        }

        // Consolida casos exitosos con diferentes configuraciones
        [Theory]
        [MemberData(nameof(GetValidProductConfigurations))]
        public async Task Should_ReturnOk_WhenRequestIsValidWithDifferentConfigurations(
            CreateProductRequest validRequest,
            string scenario)
        {
            // Arrange
            SetAdminAuthentication();
            CreateProductRequest uniqueRequest = validRequest with 
            { 
                Name = $"{validRequest.Name} {Guid.NewGuid()}" 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Products.Base, uniqueRequest);
            long? productId = await response.Content.ReadFromJsonAsync<long?>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK, because: scenario);
            productId.Should().BeGreaterThan(0);
        }

        public static IEnumerable<object[]> GetValidProductConfigurations()
        {
            yield return new object[]
            {
                new CreateProductRequest(
                    Name: "Xbox Series X",
                    Description: "",
                    ShortDescription: "",
                    BrandId: 1,
                    CategoryId: 1,
                    ProductTaxCategoryId: 1,
                    IsActive: true,
                    IsFeatured: false,
                    IsDigital: false,
                    MetaTitle: "",
                    MetaDescription: "",
                    MetaKeywords: ""),
                "minimal fields should be accepted"
            };

            yield return new object[]
            {
                _request with 
                { 
                    Name = "PlayStation Plus 12 Month Membership",
                    IsDigital = true 
                },
                "digital products should be created successfully"
            };

            yield return new object[]
            {
                _request with 
                { 
                    Name = "Nintendo Switch OLED",
                    IsActive = false 
                },
                "inactive products should be created successfully"
            };

            yield return new object[]
            {
                _request with 
                { 
                    Name = "Gaming Headset Pro",
                    IsFeatured = false 
                },
                "non-featured products should be created successfully"
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
            CreateProductRequest request = _request with 
            { 
                Name = $"Test Product {Guid.NewGuid()}" 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Products.Base, request);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserIsNotAdmin()
        {
            // Arrange
            SetCustomerUserAuthentication();
            CreateProductRequest request = _request with 
            { 
                Name = $"Test Product {Guid.NewGuid()}" 
            };

            // Act
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Products.Base, request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

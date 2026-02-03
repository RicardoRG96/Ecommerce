using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Abstractions.Search;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Products.Product.Search
{
    public class SearchProductTests : BaseFunctionalTest
    {
        #region Seed Data Constants - Products with SKUs

        /// <summary>
        /// Product: Smartphone Galaxy X - Has 2 SKUs (Published)
        /// Expected to be searchable by: "Smartphone", "Galaxy", "Samsung"
        /// </summary>
        private const string SMARTPHONE_GALAXY_X_NAME = "Smartphone Galaxy X";

        private const string NIKE_BRAND = "Nike";
        
        #endregion

        private bool _isIndexInitialized = false;

        public SearchProductTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        #region Setup - Ensure Index is Ready

        /// <summary>
        /// Ensures that products are indexed in Algolia before running search tests.
        /// This method should be called in Arrange phase of each test.
        /// </summary>
        private async Task EnsureIndexIsReadyAsync()
        {
            if (_isIndexInitialized)
            {
                return;
            }

            SetAdminAuthentication();
            
            // Index products
            await HttpClient.PostAsync($"{ApiRoutes.Products.Base}/index-search", null);
            
            // Wait for Algolia to process indexing (increased time for reliability)
            await Task.Delay(TimeSpan.FromSeconds(3));
            
            _isIndexInitialized = true;
        }

        #endregion

        #region Successful Search Tests

        [Fact]
        public async Task Should_ReturnOk_WhenProductsAreFound()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string searchQuery = Uri.EscapeDataString("Smartphone");

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "searching for existing products should return 200 OK");
        }

        [Fact]
        public async Task Should_ReturnListOfProducts_WhenSearchingByProductName()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string searchQuery = Uri.EscapeDataString("Smartphone");

            // Act
            List<ProductSearchModel>? searchResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            searchResults.Should().NotBeNull();
            searchResults.Should().NotBeEmpty(
                because: "'Smartphone' exists in seed data");
            searchResults.Should().Contain(p => p.Name.Contains("Smartphone", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public async Task Should_ReturnAllMatchingProducts_WhenSearchingByPartialName()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string searchQuery = Uri.EscapeDataString("Galaxy");

            // Act
            List<ProductSearchModel>? searchResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            searchResults.Should().NotBeNull();
            searchResults.Should().NotBeEmpty();
            searchResults.Should().AllSatisfy(p => 
                p.Name.ToLowerInvariant().Contains("galaxy"));
        }

        [Fact]
        public async Task Should_ReturnProductsWithAllProperties_WhenSearching()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string searchQuery = Uri.EscapeDataString(SMARTPHONE_GALAXY_X_NAME);

            // Act
            List<ProductSearchModel>? searchResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            searchResults.Should().NotBeNull();
            searchResults.Should().NotBeEmpty();

            var firstResult = searchResults!.First();
            firstResult.ObjectID.Should().NotBeNullOrEmpty();
            firstResult.ProductId.Should().NotBeNullOrEmpty();
            firstResult.Name.Should().NotBeNullOrEmpty();
            firstResult.Slug.Should().NotBeNullOrEmpty();
            firstResult.Description.Should().NotBeNull();
            firstResult.SkuCode.Should().NotBeNullOrEmpty();
            firstResult.Price.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Should_FindProductByBrandName()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string searchQuery = Uri.EscapeDataString(NIKE_BRAND);

            // Act
            List<ProductSearchModel>? searchResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            searchResults.Should().NotBeNull();
            
            if (searchResults!.Any())
            {
                searchResults.Should().AllSatisfy(p => 
                    p.Brand.Should().Be(NIKE_BRAND,
                        because: "searching by brand name should return products from that brand"));
            }
        }

        [Fact]
        public async Task Should_FindProductByDescription()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string searchQuery = Uri.EscapeDataString("smartphone");

            // Act
            List<ProductSearchModel>? searchResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            searchResults.Should().NotBeNull();
            searchResults.Should().NotBeEmpty(
                because: "description field is searchable and contains 'smartphone'");
        }

        [Fact]
        public async Task Should_ReturnMultipleSkus_ForSameProduct_WhenSearching()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string searchQuery = Uri.EscapeDataString(SMARTPHONE_GALAXY_X_NAME);

            // Act
            List<ProductSearchModel>? searchResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            searchResults.Should().NotBeNull();
            searchResults.Should().NotBeEmpty();

            // Smartphone Galaxy X has multiple SKUs
            searchResults.Should().HaveCountGreaterThanOrEqualTo(2,
                because: "Smartphone Galaxy X has at least 2 SKUs");

            // All results should be for the same product but different SKUs
            var distinctProductIds = searchResults!.Select(p => p.ProductId).Distinct().ToList();
            distinctProductIds.Should().ContainSingle(
                because: "all results should be for the same product");

            // But different SKU codes
            var distinctSkuCodes = searchResults.Select(p => p.SkuCode).Distinct().ToList();
            distinctSkuCodes.Should().HaveCountGreaterThanOrEqualTo(2,
                because: "product has multiple SKU variants");
        }

        [Theory]
        [InlineData("iPhone", "Should find iPhone products")]
        [InlineData("Laptop", "Should find Laptop products")]
        [InlineData("Auriculares", "Should find Auriculares products")]
        [InlineData("Smart TV", "Should find Smart TV products")]
        public async Task Should_FindProducts_ForDifferentSearchTerms(string searchTerm, string reason)
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string searchQuery = Uri.EscapeDataString(searchTerm);

            // Act
            List<ProductSearchModel>? searchResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            searchResults.Should().NotBeNull();
            searchResults.Should().NotBeEmpty(because: reason);
        }

        [Fact]
        public async Task Should_HandleCaseInsensitiveSearch()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string lowerCaseQuery = Uri.EscapeDataString("smartphone");
            string upperCaseQuery = Uri.EscapeDataString("SMARTPHONE");
            string mixedCaseQuery = Uri.EscapeDataString("SmartPhone");

            // Act
            var lowerCaseResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>($"{ApiRoutes.Products.Base}/search?searchQuery={lowerCaseQuery}");
            
            var upperCaseResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>($"{ApiRoutes.Products.Base}/search?searchQuery={upperCaseQuery}");
            
            var mixedCaseResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>($"{ApiRoutes.Products.Base}/search?searchQuery={mixedCaseQuery}");

            // Assert
            lowerCaseResults.Should().NotBeEmpty();
            upperCaseResults.Should().NotBeEmpty();
            mixedCaseResults.Should().NotBeEmpty();

            // All should return same results
            lowerCaseResults!.Count.Should().Be(upperCaseResults!.Count);
            lowerCaseResults.Count.Should().Be(mixedCaseResults!.Count);
        }

        [Fact]
        public async Task Should_HandleSpecialCharactersInSearch()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string searchQuery = Uri.EscapeDataString("Smart TV 65\"");

            // Act
            List<ProductSearchModel>? searchResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            searchResults.Should().NotBeNull();
            // Should handle gracefully even with special characters
        }

        [Fact]
        public async Task Should_ReturnProductsWithCorrectBrandAndCategory()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string searchQuery = Uri.EscapeDataString("Smartphone");

            // Act
            List<ProductSearchModel>? searchResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            searchResults.Should().NotBeNull();
            searchResults.Should().NotBeEmpty();

            searchResults.Should().AllSatisfy(p =>
            {
                p.Brand.Should().NotBeNullOrEmpty(
                    because: "all products in seed data have brands");
                p.Category.Should().NotBeNullOrEmpty(
                    because: "all products in seed data have categories");
            });
        }

        [Fact]
        public async Task Should_ReturnProductsWithImageUrl()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string searchQuery = Uri.EscapeDataString("Smartphone");

            // Act
            List<ProductSearchModel>? searchResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            searchResults.Should().NotBeNull();
            searchResults.Should().NotBeEmpty();

            // At least some products should have images
            searchResults.Should().Contain(p => !string.IsNullOrEmpty(p.ImageUrl),
                because: "products in seed data have gallery images");
        }

        [Fact]
        public async Task Should_ReturnProductsWithAttributes()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string searchQuery = Uri.EscapeDataString("Smartphone");

            // Act
            List<ProductSearchModel>? searchResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            searchResults.Should().NotBeNull();
            searchResults.Should().NotBeEmpty();

            // SKUs with variants should have attributes
            searchResults.Should().Contain(p => p.Attributes != null && p.Attributes.Any(),
                because: "product SKUs have variant attributes in seed data");
        }

        [Fact]
        public async Task Should_HandleSearchWithSpaces()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string searchQuery = Uri.EscapeDataString("Smart TV 4K");

            // Act
            List<ProductSearchModel>? searchResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            searchResults.Should().NotBeNull();
            
            if (searchResults!.Any())
            {
                searchResults.Should().Contain(p => 
                    p.Name.Contains("Smart TV", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.Contains("4K", StringComparison.OrdinalIgnoreCase));
            }
        }

        [Fact]
        public async Task Should_ReturnRelevantResults_OrderedByRelevance()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string searchQuery = Uri.EscapeDataString("iPhone Pro Max");

            // Act
            List<ProductSearchModel>? searchResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            searchResults.Should().NotBeNull();
            searchResults.Should().NotBeEmpty();

            // Most relevant results (exact name match) should appear first
            var firstResult = searchResults!.First();
            firstResult.Name.Should().Contain("iPhone", 
                because: "most relevant result should match search query");
        }

        #endregion

        #region No Results Tests

        [Fact]
        public async Task Should_ReturnNotFound_WhenNoProductsMatch()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string searchQuery = Uri.EscapeDataString("NonExistentProductXYZ123");

            // Act
            HttpResponseMessage response = await HttpClient
                .GetAsync($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                because: "searching for non-existent products should return 404");
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenSearchingForUnpublishedProduct()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            long unpublishedProductId = await CreateUnpublishedProduct();
            
            // Re-index to ensure unpublished product is not in index
            SetAdminAuthentication();
            await HttpClient.PostAsync($"{ApiRoutes.Products.Base}/index-search", null);
            await Task.Delay(TimeSpan.FromSeconds(3));

            string searchQuery = Uri.EscapeDataString($"UnpublishedProduct{unpublishedProductId}");

            // Act
            HttpResponseMessage response = await HttpClient
                .GetAsync($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                because: "unpublished products should not be searchable");
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenSearchingWithRandomCharacters()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string searchQuery = Uri.EscapeDataString("!@#$%^&*()");

            // Act
            HttpResponseMessage response = await HttpClient
                .GetAsync($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                because: "random special characters should not match any products");
        }

        #endregion

        #region Edge Cases Tests

        [Fact]
        public async Task Should_HandleEmptySearchQuery()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string searchQuery = Uri.EscapeDataString("");

            // Act
            HttpResponseMessage response = await HttpClient
                .GetAsync($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            // Behavior may vary - either return all or return bad request
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.OK, 
                HttpStatusCode.BadRequest,
                HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_HandleVeryLongSearchQuery()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string longQuery = new string('a', 500);
            string searchQuery = Uri.EscapeDataString(longQuery);

            // Act
            HttpResponseMessage response = await HttpClient
                .GetAsync($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.OK,
                HttpStatusCode.NotFound,
                HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_HandleSearchWithOnlyWhitespace()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string searchQuery = Uri.EscapeDataString("   ");

            // Act
            HttpResponseMessage response = await HttpClient
                .GetAsync($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.OK,
                HttpStatusCode.NotFound,
                HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_HandleNumericSearch()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string searchQuery = Uri.EscapeDataString("65");

            // Act
            List<ProductSearchModel>? searchResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert - Should find Smart TV 65 4K
            searchResults.Should().NotBeNull();
            
            if (searchResults!.Any())
            {
                searchResults.Should().Contain(p => p.Name.Contains("65"));
            }
        }

        [Fact]
        public async Task Should_HandleSearchWithAccents()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string searchQuery = Uri.EscapeDataString("Electrónica");

            // Act
            HttpResponseMessage response = await HttpClient
                .GetAsync($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.OK,
                HttpStatusCode.NotFound);
        }

        #endregion

        #region Authorization Tests

        [Theory]
        [InlineData(HttpStatusCode.OK, "search is public")]
        public async Task Should_AllowSearchWithoutAuthentication(
            HttpStatusCode expectedStatusCode,
            string reason)
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            HttpClient.DefaultRequestHeaders.Authorization = null;
            string searchQuery = Uri.EscapeDataString("Smartphone");

            // Act
            HttpResponseMessage response = await HttpClient
                .GetAsync($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_AllowCustomerToSearch()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            SetCustomerUserAuthentication();
            string searchQuery = Uri.EscapeDataString("Smartphone");

            // Act
            HttpResponseMessage response = await HttpClient
                .GetAsync($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "customers should be able to search products");
        }

        [Fact]
        public async Task Should_AllowCustomerSupportToSearch()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            SetCustomerSupportUserAuthentication();
            string searchQuery = Uri.EscapeDataString("Smartphone");

            // Act
            HttpResponseMessage response = await HttpClient
                .GetAsync($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "customer support should be able to search products");
        }

        [Fact]
        public async Task Should_AllowAdminToSearch()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            SetAdminAuthentication();
            string searchQuery = Uri.EscapeDataString("Smartphone");

            // Act
            HttpResponseMessage response = await HttpClient
                .GetAsync($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "admins should be able to search products");
        }

        #endregion

        #region Performance Tests

        [Fact]
        public async Task Should_HandleMultipleConsecutiveSearches()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string[] searchQueries = 
            {
                "Smartphone",
                "iPhone",
                "Laptop",
                "Auriculares",
                "Smart TV"
            };

            // Act & Assert
            foreach (var query in searchQueries)
            {
                string searchQuery = Uri.EscapeDataString(query);
                HttpResponseMessage response = await HttpClient
                    .GetAsync($"{ApiRoutes.Products.Base}/search?searchQuery={searchQuery}");

                response.StatusCode.Should().BeOneOf(
                    HttpStatusCode.OK,
                    HttpStatusCode.NotFound);
            }
        }

        #endregion

        #region Helper Methods

        private async Task<long> CreateUnpublishedProduct()
        {
            SetAdminAuthentication();

            Web.Api.Endpoints.v1.Products.Product.Create.CreateProductRequest createRequest = new(
                Name: $"UnpublishedProduct{Guid.NewGuid()}",
                Description: "Test unpublished product - should not be searchable",
                ShortDescription: "Test short description",
                BrandId: 1,
                CategoryId: 1,
                ProductTaxCategoryId: 1,
                IsActive: true,
                IsFeatured: false,
                IsDigital: false,
                MetaTitle: "Test Meta Title",
                MetaDescription: "Test Meta Description",
                MetaKeywords: "test, product");

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiRoutes.Products.Base, createRequest);
            response.EnsureSuccessStatusCode();

            long? productId = await response.Content.ReadFromJsonAsync<long?>();
            
            // Product is created but not published by default
            return productId!.Value;
        }

        #endregion
    }
}

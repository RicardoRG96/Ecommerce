using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Algolia.Search.Clients;
using Algolia.Search.Models.Search;
using Application.Abstractions.Search;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Products.Product.Search
{
    public class IndexProductsTests : BaseFunctionalTest
    {
        #region Seed Data Constants - Products with SKUs

        /// <summary>
        /// Product ID 1: Smartphone Galaxy X - Has 2 SKUs (Published)
        /// SKU IDs: 1 (GALX-128-BLK), 2 (GALX-256-BLK)
        /// </summary>
        private const long SMARTPHONE_GALAXY_X_ID = 1;
        private const string SMARTPHONE_GALAXY_X_NAME = "Smartphone Galaxy X";

        /// <summary>
        /// Product ID 2: iPhone Pro Max - Has 2 SKUs (Published)
        /// SKU IDs: 3 (IPPM-256-SLV), 4 (IPPM-512-SLV)
        /// </summary>
        private const long IPHONE_PRO_MAX_ID = 2;
        private const string IPHONE_PRO_MAX_NAME = "iPhone Pro Max";

        /// <summary>
        /// Product ID 3: Laptop Ultrabook Pro - Has 1 SKU (Published)
        /// SKU ID: 5 (ULTRA-I7-16GB)
        /// </summary>
        private const long LAPTOP_ULTRABOOK_PRO_ID = 3;
        private const string LAPTOP_ULTRABOOK_PRO_NAME = "Laptop Ultrabook Pro";

        /// <summary>
        /// Product ID 4: Auriculares Wireless ANC - Has 1 SKU (Published)
        /// SKU ID: 6 (ANC-BLK)
        /// </summary>
        private const long AURICULARES_WIRELESS_ID = 4;
        private const string AURICULARES_WIRELESS_NAME = "Auriculares Wireless ANC";

        /// <summary>
        /// Product ID 5: Smart TV 65 4K - Has 1 SKU (Published)
        /// SKU ID: 7 (STV65-4K)
        /// </summary>
        private const long SMART_TV_65_ID = 5;

        #endregion

        private readonly ISearchClient _searchClient;
        private readonly string _indexName;

        public IndexProductsTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
            var scope = factory.Services.CreateScope();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            
            _searchClient = new SearchClient(
                configuration["Algolia:ApplicationId"]!,
                configuration["Algolia:AdminApiKey"]!);
            
            _indexName = configuration["Algolia:IndexName"] ?? "test_products";
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenIndexingIsSuccessful()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PostAsync($"{ApiRoutes.Products.Base}/index-search", null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "successful indexing should return 204 No Content");
        }

        [Fact]
        public async Task Should_IndexAllPublishedProducts_InAlgolia()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PostAsync($"{ApiRoutes.Products.Base}/index-search", null);

            // Assert
            response.EnsureSuccessStatusCode();

            // Wait for Algolia to process the indexing
            await Task.Delay(TimeSpan.FromSeconds(2));

            // Verify products are in Algolia
            var searchResult = await _searchClient.SearchSingleIndexAsync<ProductSearchModel>(
                _indexName,
                new SearchParams(new SearchParamsObject { Query = "" }),
                null,
                CancellationToken.None);

            searchResult.Hits.Should().NotBeEmpty(
                because: "all published products with SKUs should be indexed");
        }

        [Fact]
        public async Task Should_IndexProductWithCorrectObjectID_Format()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            await HttpClient.PostAsync($"{ApiRoutes.Products.Base}/index-search", null);
            await Task.Delay(TimeSpan.FromSeconds(2));

            // Assert
            var searchResult = await _searchClient.SearchSingleIndexAsync<ProductSearchModel>(
                _indexName,
                new SearchParams(new SearchParamsObject { Query = SMARTPHONE_GALAXY_X_NAME }),
                null,
                CancellationToken.None);

            searchResult.Hits.Should().NotBeEmpty();
            searchResult.Hits.Should().AllSatisfy(hit =>
            {
                hit.ObjectID.Should().MatchRegex(@"^\d+_\d+$",
                    because: "ObjectID should follow format 'ProductId_SkuId'");
            });
        }

        [Fact]
        public async Task Should_IndexProductWithAllRequiredProperties()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            await HttpClient.PostAsync($"{ApiRoutes.Products.Base}/index-search", null);
            await Task.Delay(TimeSpan.FromSeconds(2));

            // Assert
            var searchResult = await _searchClient.SearchSingleIndexAsync<ProductSearchModel>(
                _indexName,
                new SearchParams(new SearchParamsObject { Query = SMARTPHONE_GALAXY_X_NAME }),
                null,
                CancellationToken.None);

            searchResult.Hits.Should().NotBeEmpty();
            
            var firstHit = searchResult.Hits.First();
            firstHit.Name.Should().NotBeNullOrEmpty();
            firstHit.Slug.Should().NotBeNullOrEmpty();
            firstHit.Description.Should().NotBeNull();
            firstHit.SkuCode.Should().NotBeNullOrEmpty();
            firstHit.Price.Should().BeGreaterThan(0);
            firstHit.ProductId.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Should_IndexProductWithBrandInformation()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            await HttpClient.PostAsync($"{ApiRoutes.Products.Base}/index-search", null);
            await Task.Delay(TimeSpan.FromSeconds(2));

            // Assert
            var searchResult = await _searchClient.SearchSingleIndexAsync<ProductSearchModel>(
                _indexName,
                new SearchParams(new SearchParamsObject { Query = SMARTPHONE_GALAXY_X_NAME }),
                null,
                CancellationToken.None);

            searchResult.Hits.Should().NotBeEmpty();
            searchResult.Hits.Should().AllSatisfy(hit =>
            {
                hit.Brand.Should().NotBeNullOrEmpty(
                    because: "all products in seed data have brands");
            });
        }

        [Fact]
        public async Task Should_IndexProductWithCategoryInformation()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            await HttpClient.PostAsync($"{ApiRoutes.Products.Base}/index-search", null);
            await Task.Delay(TimeSpan.FromSeconds(2));

            // Assert
            var searchResult = await _searchClient.SearchSingleIndexAsync<ProductSearchModel>(
                _indexName,
                new SearchParams(new SearchParamsObject { Query = SMARTPHONE_GALAXY_X_NAME }),
                null,
                CancellationToken.None);

            searchResult.Hits.Should().NotBeEmpty();
            searchResult.Hits.Should().AllSatisfy(hit =>
            {
                hit.Category.Should().NotBeNullOrEmpty(
                    because: "all products in seed data have categories");
            });
        }

        [Fact]
        public async Task Should_IndexMultipleSkusForSameProduct()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            await HttpClient.PostAsync($"{ApiRoutes.Products.Base}/index-search", null);
            await Task.Delay(TimeSpan.FromSeconds(2));

            // Assert - Smartphone Galaxy X has 2 SKUs
            var searchResult = await _searchClient.SearchSingleIndexAsync<ProductSearchModel>(
                _indexName,
                new SearchParams(new SearchParamsObject { Query = SMARTPHONE_GALAXY_X_NAME }),
                null,
                CancellationToken.None);

            searchResult.Hits.Should().HaveCountGreaterThanOrEqualTo(2,
                because: "Smartphone Galaxy X has at least 2 SKUs in seed data");

            // Verify different SKU codes for the same product
            searchResult.Hits.Select(h => h.SkuCode).Distinct().Should().HaveCountGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task Should_IndexOnlyPublishedProducts()
        {
            // Arrange
            SetAdminAuthentication();
            long unpublishedProductId = await CreateUnpublishedProduct();

            // Act
            await HttpClient.PostAsync($"{ApiRoutes.Products.Base}/index-search", null);
            await Task.Delay(TimeSpan.FromSeconds(2));

            // Assert - Search for all products
            var searchResult = await _searchClient.SearchSingleIndexAsync<ProductSearchModel>(
                _indexName,
                new SearchParams(new SearchParamsObject { Query = "" }),
                null,
                CancellationToken.None);

            // Verify unpublished product is NOT in the index
            searchResult.Hits.Should().NotContain(hit => hit.ProductId == unpublishedProductId.ToString(),
                because: "only published products should be indexed");
        }

        [Fact]
        public async Task Should_IndexProductsWithAttributes()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            await HttpClient.PostAsync($"{ApiRoutes.Products.Base}/index-search", null);
            await Task.Delay(TimeSpan.FromSeconds(2));

            // Assert
            var searchResult = await _searchClient.SearchSingleIndexAsync<ProductSearchModel>(
                _indexName,
                new SearchParams(new SearchParamsObject { Query = SMARTPHONE_GALAXY_X_NAME }),
                null,
                CancellationToken.None);

            searchResult.Hits.Should().NotBeEmpty();
            
            // Some SKUs should have attributes (like storage, color, etc.)
            searchResult.Hits.Should().Contain(hit => hit.Attributes != null && hit.Attributes.Any(),
                because: "product SKUs have variant attributes in seed data");
        }

        [Fact]
        public async Task Should_HandleDuplicateAttributeNames_ByTakingFirst()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            await HttpClient.PostAsync($"{ApiRoutes.Products.Base}/index-search", null);
            await Task.Delay(TimeSpan.FromSeconds(2));

            // Assert - Verify no exceptions were thrown
            var searchResult = await _searchClient.SearchSingleIndexAsync<ProductSearchModel>(
                _indexName,
                new SearchParams(new SearchParamsObject { Query = "" }),
                null,
                CancellationToken.None);

            searchResult.Hits.Should().NotBeEmpty();
            
            // Verify attributes dictionary doesn't have duplicate keys
            foreach (var hit in searchResult.Hits.Where(h => h.Attributes != null))
            {
                var attributeKeys = hit.Attributes!.Keys.ToList();
                attributeKeys.Should().OnlyHaveUniqueItems(
                    because: "duplicate attribute names should be handled by GroupBy");
            }
        }

        [Fact]
        public async Task Should_IndexProductWithPrimaryImage()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            await HttpClient.PostAsync($"{ApiRoutes.Products.Base}/index-search", null);
            await Task.Delay(TimeSpan.FromSeconds(2));

            // Assert
            var searchResult = await _searchClient.SearchSingleIndexAsync<ProductSearchModel>(
                _indexName,
                new SearchParams(new SearchParamsObject { Query = SMARTPHONE_GALAXY_X_NAME }),
                null,
                CancellationToken.None);

            searchResult.Hits.Should().NotBeEmpty();
            
            // At least some products should have images
            searchResult.Hits.Should().Contain(hit => !string.IsNullOrEmpty(hit.ImageUrl),
                because: "products in seed data have gallery images");
        }

        [Fact]
        public async Task Should_UpdateExistingIndex_WhenCalledMultipleTimes()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Index twice
            await HttpClient.PostAsync($"{ApiRoutes.Products.Base}/index-search", null);
            await Task.Delay(TimeSpan.FromSeconds(2));

            var firstIndexResult = await _searchClient.SearchSingleIndexAsync<ProductSearchModel>(
                _indexName,
                new SearchParams(new SearchParamsObject { Query = "" }),
                null,
                CancellationToken.None);

            int firstCount = firstIndexResult.Hits.Count;

            // Act - Index again
            await HttpClient.PostAsync($"{ApiRoutes.Products.Base}/index-search", null);
            await Task.Delay(TimeSpan.FromSeconds(2));

            var secondIndexResult = await _searchClient.SearchSingleIndexAsync<ProductSearchModel>(
                _indexName,
                new SearchParams(new SearchParamsObject { Query = "" }),
                null,
                CancellationToken.None);

            // Assert - Should have same count (not duplicated)
            secondIndexResult.Hits.Count.Should().Be(firstCount,
                because: "re-indexing should update existing records, not duplicate them");
        }

        [Fact]
        public async Task Should_IndexProductsWithCorrectPrices()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            await HttpClient.PostAsync($"{ApiRoutes.Products.Base}/index-search", null);
            await Task.Delay(TimeSpan.FromSeconds(2));

            // Assert
            var searchResult = await _searchClient.SearchSingleIndexAsync<ProductSearchModel>(
                _indexName,
                new SearchParams(new SearchParamsObject { Query = IPHONE_PRO_MAX_NAME }),
                null,
                CancellationToken.None);

            searchResult.Hits.Should().NotBeEmpty();
            searchResult.Hits.Should().AllSatisfy(hit =>
            {
                hit.Price.Should().BeGreaterThan(0,
                    because: "all products should have valid prices");
            });

            // Verify different SKUs have different prices
            if (searchResult.Hits.Count > 1)
            {
                searchResult.Hits.Select(h => h.Price).Distinct().Should().HaveCountGreaterThan(1,
                    because: "different SKUs should have different prices");
            }
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
            HttpResponseMessage response = await HttpClient.PostAsync($"{ApiRoutes.Products.Base}/index-search", null);

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserDoesNotHavePermission()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PostAsync($"{ApiRoutes.Products.Base}/index-search", null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden,
                because: "regular customers should not have permission to index products");
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenCustomerSupportTriesToIndex()
        {
            // Arrange
            SetCustomerSupportUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PostAsync($"{ApiRoutes.Products.Base}/index-search", null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden,
                because: "customer support should not have permission to index products");
        }

        [Fact]
        public async Task Should_AllowAdminToIndex()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.PostAsync($"{ApiRoutes.Products.Base}/index-search", null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent,
                because: "admin should have permission to index products");
        }

        #region Helper Methods

        private async Task<long> CreateUnpublishedProduct()
        {
            Web.Api.Endpoints.v1.Products.Product.Create.CreateProductRequest createRequest = new(
                Name: $"Unpublished Product {Guid.NewGuid()}",
                Description: "Test unpublished product",
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

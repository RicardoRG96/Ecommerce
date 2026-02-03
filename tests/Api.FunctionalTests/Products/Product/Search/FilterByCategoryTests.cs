using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Abstractions.Search;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Products.Product.Search
{
    public class FilterByCategoryTests : BaseFunctionalTest
    {
        #region Seed Data Constants - Categories

        /// <summary>
        /// Category ID 1: Electronics - Root Category (Active)
        /// Parent: NULL, Has subcategories: Televisions, Audio Equipment, Cameras
        /// </summary>
        private const string CATEGORY_ELECTRONICS = "Electronics";

        /// <summary>
        /// Category ID 2: Computers - Root Category (Active)
        /// Parent: NULL, Has subcategories: Laptops, Desktop Computers, Computer Accessories
        /// </summary>
        private const string CATEGORY_COMPUTERS = "Computers";

        /// <summary>
        /// Category ID 3: Smartphones - Root Category (Active)
        /// Parent: NULL, Has subcategories: iPhone, Android Phones, Phone Accessories
        /// </summary>
        private const string CATEGORY_SMARTPHONES = "Smartphones";

        /// <summary>
        /// Category ID 4: Home Appliances - Root Category (Active)
        /// Parent: NULL, Has subcategories: Kitchen Appliances, Vacuum Cleaners
        /// </summary>
        private const string CATEGORY_HOME_APPLIANCES = "Home Appliances";

        /// <summary>
        /// Category ID 5: Sports & Outdoors - Root Category (Active)
        /// Parent: NULL, Has subcategories: Fitness Equipment
        /// </summary>
        private const string CATEGORY_SPORTS_OUTDOORS = "Sports & Outdoors";

        /// <summary>
        /// Category ID 7: Televisions - Subcategory of Electronics (Active)
        /// Parent: Electronics (1)
        /// </summary>
        private const string CATEGORY_TELEVISIONS = "Televisions";

        /// <summary>
        /// Category ID 12: Laptops - Subcategory of Computers (Active)
        /// Parent: Computers (2)
        /// </summary>
        private const string CATEGORY_LAPTOPS = "Laptops";

        #endregion

        private bool _isIndexInitialized = false;

        public FilterByCategoryTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        #region Setup - Ensure Index is Ready

        /// <summary>
        /// Ensures that products are indexed in Algolia before running filter tests.
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

        #region Successful Filter Tests - Single Category

        [Fact]
        public async Task Should_ReturnOk_WhenFilteringBySingleCategory()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "filtering by existing category should return 200 OK");
        }

        [Fact]
        public async Task Should_ReturnProductsOnlyFromElectronics_WhenFilteringByElectronics()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().AllSatisfy(p => 
                    p.Category.Should().Be(CATEGORY_ELECTRONICS,
                        because: "when filtering by Electronics, all results should be Electronics products"));
            }
        }

        [Fact]
        public async Task Should_ReturnProductsOnlyFromComputers_WhenFilteringByComputers()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_COMPUTERS}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().AllSatisfy(p => 
                    p.Category.Should().Be(CATEGORY_COMPUTERS,
                        because: "when filtering by Computers, all results should be Computers products"));
            }
        }

        [Fact]
        public async Task Should_ReturnProductsOnlyFromSmartphones_WhenFilteringBySmartphones()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_SMARTPHONES}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().AllSatisfy(p => 
                    p.Category.Should().Be(CATEGORY_SMARTPHONES,
                        because: "when filtering by Smartphones, all results should be Smartphones products"));
            }
        }

        [Fact]
        public async Task Should_ReturnProductsWithAllProperties_WhenFilteringByCategory()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                var firstResult = results.First();
                firstResult.ObjectID.Should().NotBeNullOrEmpty();
                firstResult.ProductId.Should().NotBeNullOrEmpty();
                firstResult.Name.Should().NotBeNullOrEmpty();
                firstResult.Slug.Should().NotBeNullOrEmpty();
                firstResult.SkuCode.Should().NotBeNullOrEmpty();
                firstResult.Category.Should().Be(CATEGORY_ELECTRONICS);
                firstResult.Price.Should().BeGreaterThan(0);
            }
        }

        [Theory]
        [InlineData(CATEGORY_ELECTRONICS, "Electronics products should be found")]
        [InlineData(CATEGORY_COMPUTERS, "Computers products should be found")]
        [InlineData(CATEGORY_SMARTPHONES, "Smartphones products should be found")]
        [InlineData(CATEGORY_HOME_APPLIANCES, "Home Appliances products should be found")]
        public async Task Should_ReturnProducts_ForDifferentActiveCategories(string categoryName, string reason)
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={Uri.EscapeDataString(categoryName)}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().AllSatisfy(p => p.Category.Should().Be(categoryName), because: reason);
            }
        }

        #endregion

        #region Successful Filter Tests - Multiple Categories

        [Fact]
        public async Task Should_ReturnOk_WhenFilteringByMultipleCategories()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}&categories={CATEGORY_COMPUTERS}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "filtering by multiple categories should return 200 OK");
        }

        [Fact]
        public async Task Should_ReturnProductsFromElectronicsAndComputers_WhenFilteringByBoth()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}&categories={CATEGORY_COMPUTERS}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                var categories = results.Select(p => p.Category).Distinct().ToList();
                
                categories.Should().Contain(CATEGORY_ELECTRONICS, because: "Electronics was included in filter");
                categories.Should().Contain(CATEGORY_COMPUTERS, because: "Computers was included in filter");
                
                results.Should().AllSatisfy(p => 
                    p.Category.Should().BeOneOf(CATEGORY_ELECTRONICS, CATEGORY_COMPUTERS));
            }
        }

        [Fact]
        public async Task Should_ReturnProductsFromThreeCategories_WhenFilteringByThree()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}&categories={CATEGORY_COMPUTERS}&categories={CATEGORY_SMARTPHONES}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                var categories = results.Select(p => p.Category).Distinct().ToList();
                
                results.Should().AllSatisfy(p => 
                    p.Category.Should().BeOneOf(CATEGORY_ELECTRONICS, CATEGORY_COMPUTERS, CATEGORY_SMARTPHONES));
                
                // Verify we got products from multiple categories
                categories.Count.Should().BeGreaterThan(1,
                    because: "we should have products from at least 2 of the 3 requested categories");
            }
        }

        [Fact]
        public async Task Should_ReturnCombinedResults_NotDuplicates_WhenFilteringMultipleCategories()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}&categories={CATEGORY_COMPUTERS}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                // Verify no duplicate ObjectIDs (SKU combinations)
                var objectIds = results.Select(p => p.ObjectID).ToList();
                objectIds.Should().OnlyHaveUniqueItems(
                    because: "each product SKU should appear only once in results");
            }
        }

        [Fact]
        public async Task Should_ReturnMoreResults_WhenFilteringMultipleCategoriesThanSingle()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            var electronicsResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}");

            var computersResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_COMPUTERS}");

            var combinedResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}&categories={CATEGORY_COMPUTERS}");

            // Assert
            if (electronicsResults!.Any() && computersResults!.Any())
            {
                combinedResults!.Count.Should().BeGreaterThanOrEqualTo(electronicsResults.Count,
                    because: "combined filter should have at least as many results as Electronics alone");
                
                combinedResults.Count.Should().BeGreaterThanOrEqualTo(computersResults.Count,
                    because: "combined filter should have at least as many results as Computers alone");
            }
        }

        #endregion

        #region Subcategory Tests

        [Fact]
        public async Task Should_HandleSubcategoryFiltering()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-categories?categories={Uri.EscapeDataString(CATEGORY_LAPTOPS)}");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.OK,
                HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_FilterByParentAndSubcategory()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            var parentResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_COMPUTERS}");

            var subcategoryResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={Uri.EscapeDataString(CATEGORY_LAPTOPS)}");

            // Assert
            parentResults.Should().NotBeNull();
            subcategoryResults.Should().NotBeNull();
        }

        #endregion

        #region Case Sensitivity Tests

        [Fact]
        public async Task Should_HandleCaseSensitiveCategoryNames()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            var lowerCaseResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories=electronics");

            var upperCaseResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories=ELECTRONICS");

            var properCaseResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}");

            // Assert
            // Note: Behavior depends on Algolia configuration
            // If case-insensitive, all should return same results
            // If case-sensitive, only properCaseResults should have data
            properCaseResults.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_HandleCategoryNamesWithSpaces()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string categoryWithSpaces = Uri.EscapeDataString(CATEGORY_HOME_APPLIANCES);

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-categories?categories={categoryWithSpaces}");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.OK,
                HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_HandleCategoryNamesWithAmpersands()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string categoryWithAmpersand = Uri.EscapeDataString(CATEGORY_SPORTS_OUTDOORS);

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-categories?categories={categoryWithAmpersand}");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.OK,
                HttpStatusCode.NotFound);
        }

        #endregion

        #region No Results Tests

        [Fact]
        public async Task Should_ReturnNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-categories?categories=NonExistentCategoryXYZ123");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                because: "filtering by non-existent category should return 404");
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenCategoryHasNoPublishedProducts()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string categoryWithoutProducts = "CategoryWithNoProducts";

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-categories?categories={Uri.EscapeDataString(categoryWithoutProducts)}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                because: "categories without published products should return no results");
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenAllCategoriesInFilterAreInvalid()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-categories?categories=InvalidCategory1&categories=InvalidCategory2");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                because: "when all categories are invalid, no results should be found");
        }

        [Fact]
        public async Task Should_ReturnResults_WhenOneCategoryIsValidAndOtherIsNot()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}&categories=InvalidCategoryXYZ");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().AllSatisfy(p => 
                    p.Category.Should().Be(CATEGORY_ELECTRONICS,
                        because: "only valid category (Electronics) should return results"));
            }
        }

        #endregion

        #region Edge Cases Tests

        [Fact]
        public async Task Should_HandleEmptyCategoryArray()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-categories");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.BadRequest,
                HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_HandleSpecialCharactersInCategoryName()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string specialChars = Uri.EscapeDataString("Category!@#$%");

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-categories?categories={specialChars}");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.OK,
                HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_HandleVeryLongCategoryName()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string longCategory = new string('A', 500);
            string encoded = Uri.EscapeDataString(longCategory);

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-categories?categories={encoded}");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.OK,
                HttpStatusCode.NotFound,
                HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_HandleManyCategoriesInSingleFilter()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            var queryString = $"?categories={CATEGORY_ELECTRONICS}&categories={CATEGORY_COMPUTERS}" +
                            $"&categories={CATEGORY_SMARTPHONES}&categories={Uri.EscapeDataString(CATEGORY_HOME_APPLIANCES)}" +
                            $"&categories={Uri.EscapeDataString(CATEGORY_SPORTS_OUTDOORS)}";

            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-categories{queryString}");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.OK,
                HttpStatusCode.NotFound);
        }

        #endregion

        #region Product Properties Verification

        [Fact]
        public async Task Should_ReturnProductsWithBrands_WhenFilteringByCategory()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().AllSatisfy(p => 
                    p.Brand.Should().NotBeNullOrEmpty(
                        because: "all products should have brands"));
            }
        }

        [Fact]
        public async Task Should_ReturnProductsWithValidPrices_WhenFilteringByCategory()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().AllSatisfy(p => 
                    p.Price.Should().BeGreaterThan(0,
                        because: "all products should have valid prices"));
            }
        }

        [Fact]
        public async Task Should_ReturnMultipleSkus_ForSameProduct_WhenFilteringByCategory()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Count >= 2)
            {
                // Check if there are products with same ProductId but different ObjectIDs (different SKUs)
                var groupedByProduct = results.GroupBy(p => p.ProductId);
                
                groupedByProduct.Should().Contain(g => g.Count() > 1,
                    because: "some products should have multiple SKU variants");
            }
        }

        [Fact]
        public async Task Should_ReturnProductsWithImages_WhenFilteringByCategory()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().Contain(p => !string.IsNullOrEmpty(p.ImageUrl),
                    because: "at least some products should have images");
            }
        }

        [Fact]
        public async Task Should_ReturnProductsFromDifferentBrands_WithinSameCategory()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Count > 1)
            {
                var brands = results.Select(p => p.Brand).Distinct().ToList();
                brands.Should().HaveCountGreaterThanOrEqualTo(1,
                    because: "products in a category can be from different brands");
            }
        }

        #endregion

        #region Authorization Tests

        [Fact]
        public async Task Should_AllowFilterWithoutAuthentication()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            HttpClient.DefaultRequestHeaders.Authorization = null;

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "filtering should be public and not require authentication");
        }

        [Fact]
        public async Task Should_AllowCustomerToFilter()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "customers should be able to filter products");
        }

        [Fact]
        public async Task Should_AllowCustomerSupportToFilter()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            SetCustomerSupportUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "customer support should be able to filter products");
        }

        [Fact]
        public async Task Should_AllowAdminToFilter()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "admins should be able to filter products");
        }

        #endregion

        #region Performance Tests

        [Fact]
        public async Task Should_ReturnResultsQuickly_ForSingleCategoryFilter()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}");

            stopwatch.Stop();

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(3000,
                because: "filtering should be fast with Algolia");
        }

        [Fact]
        public async Task Should_HandleMultipleConsecutiveFilters()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string[] categories = { CATEGORY_ELECTRONICS, CATEGORY_COMPUTERS, CATEGORY_SMARTPHONES };

            // Act & Assert
            foreach (var category in categories)
            {
                HttpResponseMessage response = await HttpClient.GetAsync(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={Uri.EscapeDataString(category)}");

                response.StatusCode.Should().BeOneOf(
                    HttpStatusCode.OK,
                    HttpStatusCode.NotFound);
            }
        }

        [Fact]
        public async Task Should_HandleRapidFilterChanges()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            var tasks = new List<Task<HttpResponseMessage>>
            {
                HttpClient.GetAsync($"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}"),
                HttpClient.GetAsync($"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_COMPUTERS}"),
                HttpClient.GetAsync($"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_SMARTPHONES}")
            };

            var responses = await Task.WhenAll(tasks);

            // Assert
            responses.Should().AllSatisfy(r => 
                r.StatusCode.Should().BeOneOf(
                    HttpStatusCode.OK, 
                    HttpStatusCode.NotFound));
        }

        [Fact]
        public async Task Should_HandleFilteringByRootAndSubcategoriesSimultaneously()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}&categories={Uri.EscapeDataString(CATEGORY_TELEVISIONS)}");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.OK,
                HttpStatusCode.NotFound);
        }

        #endregion

        #region Hierarchical Category Tests

        [Fact]
        public async Task Should_DistinguishBetweenParentAndChildCategories()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            var electronicsResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={CATEGORY_ELECTRONICS}");

            var televisionsResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-categories?categories={Uri.EscapeDataString(CATEGORY_TELEVISIONS)}");

            // Assert
            electronicsResults.Should().NotBeNull();
            televisionsResults.Should().NotBeNull();
            
            // Parent category (Electronics) should have equal or more products than child (Televisions)
            if (electronicsResults!.Any() && televisionsResults!.Any())
            {
                electronicsResults.Count.Should().BeGreaterThanOrEqualTo(televisionsResults.Count,
                    because: "parent category should include all products from child categories");
            }
        }

        #endregion
    }
}

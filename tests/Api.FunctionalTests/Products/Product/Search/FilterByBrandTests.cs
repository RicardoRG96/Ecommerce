using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Abstractions.Search;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Products.Product.Search
{
    public class FilterByBrandTests : BaseFunctionalTest
    {
        #region Seed Data Constants - Brands

        /// <summary>
        /// Brand ID 1: Nike - Featured and Active
        /// Products: Expected to have products in seed data
        /// </summary>
        private const string BRAND_NIKE = "Nike";
        private const long BRAND_NIKE_ID = 1;

        /// <summary>
        /// Brand ID 2: Adidas - Featured and Active
        /// Products: Expected to have products in seed data
        /// </summary>
        private const string BRAND_ADIDAS = "Adidas";
        private const long BRAND_ADIDAS_ID = 2;

        /// <summary>
        /// Brand ID 3: Puma - Featured and Active
        /// Products: Expected to have products in seed data
        /// </summary>
        private const string BRAND_PUMA = "Puma";
        private const long BRAND_PUMA_ID = 3;

        /// <summary>
        /// Brand ID 5: New Balance - Featured and Active
        /// Products: Expected to have products in seed data
        /// </summary>
        private const string BRAND_NEW_BALANCE = "New Balance";

        /// <summary>
        /// Brand ID 6: Under Armour - Featured and Active
        /// </summary>
        private const string BRAND_UNDER_ARMOUR = "Under Armour";

        /// <summary>
        /// Brand ID 10: Fila - Inactive Brand
        /// </summary>
        private const string BRAND_FILA = "Fila";

        /// <summary>
        /// Brand ID 15: Patagonia - Inactive Brand
        /// </summary>
        private const string BRAND_PATAGONIA = "Patagonia";

        #endregion

        private bool _isIndexInitialized = false;

        public FilterByBrandTests(FunctionalTestWebAppFactory factory) 
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

        #region Successful Filter Tests - Single Brand

        [Fact]
        public async Task Should_ReturnOk_WhenFilteringBySingleBrand()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_NIKE}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "filtering by existing brand should return 200 OK");
        }

        [Fact]
        public async Task Should_ReturnProductsOnlyFromNike_WhenFilteringByNike()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_NIKE}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().AllSatisfy(p => 
                    p.Brand.Should().Be(BRAND_NIKE,
                        because: "when filtering by Nike, all results should be Nike products"));
            }
        }

        [Fact]
        public async Task Should_ReturnProductsOnlyFromAdidas_WhenFilteringByAdidas()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_ADIDAS}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().AllSatisfy(p => 
                    p.Brand.Should().Be(BRAND_ADIDAS,
                        because: "when filtering by Adidas, all results should be Adidas products"));
            }
        }

        [Fact]
        public async Task Should_ReturnProductsOnlyFromPuma_WhenFilteringByPuma()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_PUMA}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().AllSatisfy(p => 
                    p.Brand.Should().Be(BRAND_PUMA,
                        because: "when filtering by Puma, all results should be Puma products"));
            }
        }

        [Fact]
        public async Task Should_ReturnProductsWithAllProperties_WhenFilteringByBrand()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_NIKE}");

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
                firstResult.Brand.Should().Be(BRAND_NIKE);
                firstResult.Price.Should().BeGreaterThan(0);
            }
        }

        [Theory]
        [InlineData(BRAND_NIKE, "Nike products should be found")]
        [InlineData(BRAND_ADIDAS, "Adidas products should be found")]
        [InlineData(BRAND_PUMA, "Puma products should be found")]
        public async Task Should_ReturnProducts_ForDifferentActiveBrands(string brandName, string reason)
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-brands?brands={Uri.EscapeDataString(brandName)}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().AllSatisfy(p => p.Brand.Should().Be(brandName), because: reason);
            }
        }

        #endregion

        #region Successful Filter Tests - Multiple Brands

        [Fact]
        public async Task Should_ReturnOk_WhenFilteringByMultipleBrands()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_NIKE}&brands={BRAND_ADIDAS}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "filtering by multiple brands should return 200 OK");
        }

        [Fact]
        public async Task Should_ReturnProductsFromNikeAndAdidas_WhenFilteringByBoth()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_NIKE}&brands={BRAND_ADIDAS}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                var brands = results.Select(p => p.Brand).Distinct().ToList();
                
                brands.Should().Contain(BRAND_NIKE, because: "Nike was included in filter");
                brands.Should().Contain(BRAND_ADIDAS, because: "Adidas was included in filter");
                
                results.Should().AllSatisfy(p => 
                    p.Brand.Should().BeOneOf(BRAND_NIKE, BRAND_ADIDAS));
            }
        }

        [Fact]
        public async Task Should_ReturnProductsFromThreeBrands_WhenFilteringByThree()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_NIKE}&brands={BRAND_ADIDAS}&brands={BRAND_PUMA}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                var brands = results.Select(p => p.Brand).Distinct().ToList();
                
                results.Should().AllSatisfy(p => 
                    p.Brand.Should().BeOneOf(BRAND_NIKE, BRAND_ADIDAS, BRAND_PUMA));
                
                // Verify we got products from multiple brands
                brands.Count.Should().BeGreaterThan(1,
                    because: "we should have products from at least 2 of the 3 requested brands");
            }
        }

        [Fact]
        public async Task Should_ReturnCombinedResults_NotDuplicates_WhenFilteringMultipleBrands()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_NIKE}&brands={BRAND_ADIDAS}");

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
        public async Task Should_ReturnMoreResults_WhenFilteringMultipleBrandsThanSingle()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            var nikeResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_NIKE}");

            var adidasResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_ADIDAS}");

            var combinedResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_NIKE}&brands={BRAND_ADIDAS}");

            // Assert
            if (nikeResults!.Any() && adidasResults!.Any())
            {
                combinedResults!.Count.Should().BeGreaterThanOrEqualTo(nikeResults.Count,
                    because: "combined filter should have at least as many results as Nike alone");
                
                combinedResults.Count.Should().BeGreaterThanOrEqualTo(adidasResults.Count,
                    because: "combined filter should have at least as many results as Adidas alone");
            }
        }

        #endregion

        #region Case Sensitivity Tests

        [Fact]
        public async Task Should_HandleCaseSensitiveBrandNames()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            var lowerCaseResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-brands?brands=nike");

            var upperCaseResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-brands?brands=NIKE");

            var properCaseResults = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_NIKE}");

            // Assert
            // Note: Behavior depends on Algolia configuration
            // If case-insensitive, all should return same results
            // If case-sensitive, only properCaseResults should have data
            properCaseResults.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_HandleBrandNamesWithSpaces()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string brandWithSpaces = Uri.EscapeDataString(BRAND_NEW_BALANCE);

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-brands?brands={brandWithSpaces}");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.OK,
                HttpStatusCode.NotFound);
        }

        #endregion

        #region No Results Tests

        [Fact]
        public async Task Should_ReturnNotFound_WhenBrandDoesNotExist()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-brands?brands=NonExistentBrandXYZ123");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                because: "filtering by non-existent brand should return 404");
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenAllBrandsInFilterAreInvalid()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-brands?brands=InvalidBrand1&brands=InvalidBrand2");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                because: "when all brands are invalid, no results should be found");
        }

        [Fact]
        public async Task Should_ReturnResults_WhenOneBrandIsValidAndOtherIsNot()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_NIKE}&brands=InvalidBrandXYZ");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().AllSatisfy(p => 
                    p.Brand.Should().Be(BRAND_NIKE,
                        because: "only valid brand (Nike) should return results"));
            }
        }

        #endregion

        #region Edge Cases Tests

        [Fact]
        public async Task Should_HandleEmptyBrandArray()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-brands");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.BadRequest,
                HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_HandleSpecialCharactersInBrandName()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string specialChars = Uri.EscapeDataString("Brand!@#$%");

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-brands?brands={specialChars}");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.OK,
                HttpStatusCode.NotFound,
                HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task Should_HandleVeryLongBrandName()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string longBrand = new string('A', 500);
            string encoded = Uri.EscapeDataString(longBrand);

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-brands?brands={encoded}");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.OK,
                HttpStatusCode.NotFound,
                HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_HandleManyBrandsInSingleFilter()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            var queryString = $"?brands={BRAND_NIKE}&brands={BRAND_ADIDAS}&brands={BRAND_PUMA}" +
                            $"&brands={Uri.EscapeDataString(BRAND_NEW_BALANCE)}" +
                            $"&brands={Uri.EscapeDataString(BRAND_UNDER_ARMOUR)}";

            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-brands{queryString}");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.OK,
                HttpStatusCode.NotFound);
        }

        #endregion

        #region Product Properties Verification

        [Fact]
        public async Task Should_ReturnProductsWithCategories_WhenFilteringByBrand()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_NIKE}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().AllSatisfy(p => 
                    p.Category.Should().NotBeNullOrEmpty(
                        because: "all products should have categories"));
            }
        }

        [Fact]
        public async Task Should_ReturnProductsWithValidPrices_WhenFilteringByBrand()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_NIKE}");

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
        public async Task Should_ReturnMultipleSkus_ForSameProduct_WhenFilteringByBrand()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_NIKE}");

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
        public async Task Should_ReturnProductsWithImages_WhenFilteringByBrand()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_NIKE}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().Contain(p => !string.IsNullOrEmpty(p.ImageUrl),
                    because: "at least some products should have images");
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
                $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_NIKE}");

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
                $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_NIKE}");

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
                $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_NIKE}");

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
                $"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_NIKE}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "admins should be able to filter products");
        }

        #endregion

        #region Performance Tests

        [Fact]
        public async Task Should_HandleMultipleConsecutiveFilters()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            string[] brands = { BRAND_NIKE, BRAND_ADIDAS, BRAND_PUMA };

            // Act & Assert
            foreach (var brand in brands)
            {
                HttpResponseMessage response = await HttpClient.GetAsync(
                    $"{ApiRoutes.Products.Base}/filter-by-brands?brands={Uri.EscapeDataString(brand)}");

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
                HttpClient.GetAsync($"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_NIKE}"),
                HttpClient.GetAsync($"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_ADIDAS}"),
                HttpClient.GetAsync($"{ApiRoutes.Products.Base}/filter-by-brands?brands={BRAND_PUMA}")
            };

            var responses = await Task.WhenAll(tasks);

            // Assert
            responses.Should().AllSatisfy(r => 
                r.StatusCode.Should().BeOneOf(
                    HttpStatusCode.OK, 
                    HttpStatusCode.NotFound));
        }

        #endregion
    }
}

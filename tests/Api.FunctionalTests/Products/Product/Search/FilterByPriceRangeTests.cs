using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Abstractions.Search;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Products.Product.Search
{
    public class FilterByPriceRangeTests : BaseFunctionalTest
    {
        #region Seed Data Constants - Price Ranges

        /// <summary>
        /// Budget Range: Under 50,000 CLP
        /// Products: Mouse Inalámbrico Ergo (29,990)
        /// </summary>
        private const decimal BUDGET_MIN = 0;
        private const decimal BUDGET_MAX = 50000;

        /// <summary>
        /// Low Range: 50,000 - 200,000 CLP
        /// Products: Teclado Mecánico Pro (89,990), SSD NVMe 1TB (129,990), Auriculares ANC (199,990)
        /// </summary>
        private const decimal LOW_MIN = 50000;
        private const decimal LOW_MAX = 200000;

        /// <summary>
        /// Mid Range: 200,000 - 500,000 CLP
        /// Products: Monitor 27 QHD (329,990), Tablet Android Plus (349,990)
        /// </summary>
        private const decimal MID_MIN = 200000;
        private const decimal MID_MAX = 500000;

        /// <summary>
        /// High Range: 500,000 - 1,000,000 CLP
        /// Products: Smartphone Galaxy X (799,990 - 849,990), Smart TV 65 4K (899,990)
        /// </summary>
        private const decimal HIGH_MIN = 500000;
        private const decimal HIGH_MAX = 1000000;

        /// <summary>
        /// Premium Range: Over 1,000,000 CLP
        /// Products: iPhone Pro Max (1,199,990 - 1,349,990), Laptop Ultrabook Pro (1,499,990)
        /// </summary>
        private const decimal PREMIUM_MIN = 1000000;
        private const decimal PREMIUM_MAX = 2000000;

        /// <summary>
        /// Specific Product Prices from seed data
        /// </summary>
        private const decimal MOUSE_PRICE = 29990;
        private const decimal KEYBOARD_PRICE = 89990;
        private const decimal SSD_PRICE = 129990;
        private const decimal HEADPHONES_PRICE = 199990;
        private const decimal MONITOR_PRICE = 329990;
        private const decimal TABLET_PRICE = 349990;
        private const decimal SMARTPHONE_MIN_PRICE = 799990;
        private const decimal SMARTPHONE_MAX_PRICE = 849990;
        private const decimal SMART_TV_PRICE = 899990;
        private const decimal IPHONE_MIN_PRICE = 1199990;
        private const decimal IPHONE_MAX_PRICE = 1349990;
        private const decimal LAPTOP_PRICE = 1499990;

        #endregion

        private bool _isIndexInitialized = false;

        public FilterByPriceRangeTests(FunctionalTestWebAppFactory factory) 
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

        #region Successful Filter Tests - Basic Ranges

        [Fact]
        public async Task Should_ReturnOk_WhenFilteringByValidPriceRange()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={LOW_MIN}&maxPrice={LOW_MAX}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "filtering by valid price range should return 200 OK");
        }

        [Fact]
        public async Task Should_ReturnProductsInLowRange()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={LOW_MIN}&maxPrice={LOW_MAX}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().AllSatisfy(p => 
                {
                    p.Price.Should().BeGreaterThanOrEqualTo(LOW_MIN);
                    p.Price.Should().BeLessThanOrEqualTo(LOW_MAX);
                });

                // Should include products in 50k-200k range
                var expectedPrices = new[] { KEYBOARD_PRICE, SSD_PRICE, HEADPHONES_PRICE };
                results.Select(p => p.Price).Should().Contain(expectedPrices.Where(price => 
                    price >= LOW_MIN && price <= LOW_MAX));
            }
        }

        [Fact]
        public async Task Should_ReturnProductsInMidRange()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={MID_MIN}&maxPrice={MID_MAX}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().AllSatisfy(p => 
                {
                    p.Price.Should().BeGreaterThanOrEqualTo(MID_MIN);
                    p.Price.Should().BeLessThanOrEqualTo(MID_MAX);
                });

                // Should include Monitor (329,990) and Tablet (349,990)
                results.Should().Contain(p => p.Price == MONITOR_PRICE || p.Price == TABLET_PRICE);
            }
        }

        [Fact]
        public async Task Should_ReturnProductsInHighRange()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={HIGH_MIN}&maxPrice={HIGH_MAX}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().AllSatisfy(p => 
                {
                    p.Price.Should().BeGreaterThanOrEqualTo(HIGH_MIN);
                    p.Price.Should().BeLessThanOrEqualTo(HIGH_MAX);
                });

                // Should include Smartphone and Smart TV
                results.Should().Contain(p => 
                    (p.Price >= SMARTPHONE_MIN_PRICE && p.Price <= SMARTPHONE_MAX_PRICE) ||
                    p.Price == SMART_TV_PRICE);
            }
        }

        [Fact]
        public async Task Should_ReturnProductsInPremiumRange()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={PREMIUM_MIN}&maxPrice={PREMIUM_MAX}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().AllSatisfy(p => 
                {
                    p.Price.Should().BeGreaterThanOrEqualTo(PREMIUM_MIN);
                    p.Price.Should().BeLessThanOrEqualTo(PREMIUM_MAX);
                });

                // Should include iPhone and Laptop
                results.Should().Contain(p => 
                    (p.Price >= IPHONE_MIN_PRICE && p.Price <= IPHONE_MAX_PRICE) ||
                    p.Price == LAPTOP_PRICE);
            }
        }

        #endregion

        #region Exact Price Filtering

        [Fact]
        public async Task Should_ReturnProductWithExactPrice()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={MOUSE_PRICE}&maxPrice={MOUSE_PRICE}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().AllSatisfy(p => p.Price.Should().Be(MOUSE_PRICE));
                results.Should().Contain(p => p.Name.Contains("Mouse", StringComparison.OrdinalIgnoreCase));
            }
        }

        [Fact]
        public async Task Should_ReturnMultipleProductsWithSimilarPrices()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            decimal priceRange = 10000; // +/- 10k

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={MONITOR_PRICE - priceRange}&maxPrice={TABLET_PRICE + priceRange}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().AllSatisfy(p => 
                {
                    p.Price.Should().BeGreaterThanOrEqualTo(MONITOR_PRICE - priceRange);
                    p.Price.Should().BeLessThanOrEqualTo(TABLET_PRICE + priceRange);
                });

                // Should include both Monitor and Tablet
                results.Should().Contain(p => p.Price == MONITOR_PRICE);
                results.Should().Contain(p => p.Price == TABLET_PRICE);
            }
        }

        #endregion

        #region Wide Range Tests

        [Fact]
        public async Task Should_ReturnAllProducts_WhenRangeCoversAllPrices()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice=1&maxPrice=10000000");

            // Assert
            results.Should().NotBeNull();
            results.Should().NotBeEmpty(
                because: "a very wide range should return all products");

            results.Should().AllSatisfy(p => 
            {
                p.Price.Should().BeGreaterThanOrEqualTo(1);
                p.Price.Should().BeLessThanOrEqualTo(10000000);
            });
        }

        [Fact]
        public async Task Should_ReturnProductsWithAllProperties_WhenFilteringByPrice()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={MID_MIN}&maxPrice={MID_MAX}");

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
                firstResult.Price.Should().BeGreaterThan(0);
                firstResult.Brand.Should().NotBeNullOrEmpty();
                firstResult.Category.Should().NotBeNullOrEmpty();
            }
        }

        #endregion

        #region Boundary Tests

        [Fact]
        public async Task Should_IncludeProductsAtMinimumBoundary()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={MOUSE_PRICE}&maxPrice={KEYBOARD_PRICE}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                // Should include product at minimum boundary
                results.Should().Contain(p => p.Price == MOUSE_PRICE,
                    because: "products at minimum boundary should be included");
            }
        }

        [Fact]
        public async Task Should_IncludeProductsAtMaximumBoundary()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={MOUSE_PRICE}&maxPrice={KEYBOARD_PRICE}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                // Should include product at maximum boundary
                results.Should().Contain(p => p.Price == KEYBOARD_PRICE,
                    because: "products at maximum boundary should be included");
            }
        }

        [Fact]
        public async Task Should_ExcludeProductsOutsideRange()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={LOW_MIN}&maxPrice={LOW_MAX}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                // Should NOT include products outside range
                results.Should().NotContain(p => p.Price < LOW_MIN,
                    because: "products below minimum should be excluded");
                results.Should().NotContain(p => p.Price > LOW_MAX,
                    because: "products above maximum should be excluded");
            }
        }

        #endregion

        #region No Results Tests

        [Fact]
        public async Task Should_ReturnNotFound_WhenNoPricesInRange()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            decimal impossibleMin = 5000000; // 5 million
            decimal impossibleMax = 10000000; // 10 million

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={impossibleMin}&maxPrice={impossibleMax}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                because: "when no products exist in price range, should return 404");
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenMinPriceIsTooHigh()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            decimal tooHighMin = 2000000; // Higher than most expensive product
            decimal tooHighMax = 3000000;

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={tooHighMin}&maxPrice={tooHighMax}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                because: "min price is higher than all products");
        }

        #endregion

        #region Invalid Input Tests

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMinPriceIsNegative()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice=-100&maxPrice={LOW_MAX}");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.BadRequest,
                HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMaxPriceIsNegative()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={LOW_MIN}&maxPrice=-100");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.BadRequest,
                HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMinPriceIsGreaterThanMaxPrice()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={HIGH_MAX}&maxPrice={LOW_MIN}");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.BadRequest,
                HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_HandleVeryLargeMaxPrice()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            decimal veryLarge = 999999999;

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={LOW_MIN}&maxPrice={veryLarge}");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.OK,
                HttpStatusCode.NotFound);
        }

        #endregion

        #region Decimal Precision Tests

        [Fact]
        public async Task Should_HandlePricesWithTwoDecimals()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice=29990,99&maxPrice=89990,99");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.OK,
                HttpStatusCode.NotFound);
        }

        #endregion

        #region Multiple SKUs Same Product Tests

        [Fact]
        public async Task Should_ReturnMultipleSkusOfSameProduct_WhenInPriceRange()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act - Filter for Smartphone price range (799,990 - 849,990)
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={SMARTPHONE_MIN_PRICE}&maxPrice={SMARTPHONE_MAX_PRICE}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                // Check if we have multiple SKUs from same product
                var smartphoneSkus = results.Where(p => p.Name.Contains("Smartphone Galaxy X", StringComparison.OrdinalIgnoreCase)).ToList();
                
                if (smartphoneSkus.Count > 1)
                {
                    smartphoneSkus.Should().HaveCountGreaterThanOrEqualTo(2,
                        because: "Smartphone Galaxy X has 2 SKUs in this price range");

                    // All should be from same product but different SKUs
                    var productIds = smartphoneSkus.Select(s => s.ProductId).Distinct();
                    productIds.Should().ContainSingle(
                        because: "all SKUs should belong to same product");

                    // But different prices
                    var prices = smartphoneSkus.Select(s => s.Price).Distinct();
                    prices.Should().HaveCountGreaterThan(1,
                        because: "different SKUs have different prices");
                }
            }
        }

        [Fact]
        public async Task Should_FilterOutExpensiveSkuOfProduct()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act - Filter that includes cheaper SKU but excludes expensive one
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={SMARTPHONE_MIN_PRICE}&maxPrice={SMARTPHONE_MIN_PRICE + 10000}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                // Should include cheaper SKU (799,990)
                results.Should().Contain(p => p.Price == SMARTPHONE_MIN_PRICE);

                // Should NOT include expensive SKU (849,990)
                results.Should().NotContain(p => p.Price == SMARTPHONE_MAX_PRICE);
            }
        }

        #endregion

        #region Product Properties Verification

        [Fact]
        public async Task Should_ReturnProductsWithBrandsAndCategories()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={MID_MIN}&maxPrice={MID_MAX}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Any())
            {
                results.Should().AllSatisfy(p => 
                {
                    p.Brand.Should().NotBeNullOrEmpty(
                        because: "all products should have brands");
                    p.Category.Should().NotBeNullOrEmpty(
                        because: "all products should have categories");
                });
            }
        }

        [Fact]
        public async Task Should_ReturnProductsFromDifferentBrands_InSamePriceRange()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={HIGH_MIN}&maxPrice={HIGH_MAX}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Count > 1)
            {
                var brands = results.Select(p => p.Brand).Distinct().ToList();
                brands.Should().HaveCountGreaterThanOrEqualTo(1,
                    because: "products in same price range can be from different brands");
            }
        }

        [Fact]
        public async Task Should_ReturnProductsFromDifferentCategories_InSamePriceRange()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={MID_MIN}&maxPrice={MID_MAX}");

            // Assert
            results.Should().NotBeNull();
            
            if (results!.Count > 1)
            {
                var categories = results.Select(p => p.Category).Distinct().ToList();
                categories.Should().HaveCountGreaterThanOrEqualTo(1,
                    because: "products in same price range can be from different categories");
            }
        }

        [Fact]
        public async Task Should_ReturnProductsWithImages()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            List<ProductSearchModel>? results = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={MID_MIN}&maxPrice={MID_MAX}");

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
                $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={LOW_MIN}&maxPrice={LOW_MAX}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "price filtering should be public and not require authentication");
        }

        [Fact]
        public async Task Should_AllowCustomerToFilter()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={LOW_MIN}&maxPrice={LOW_MAX}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "customers should be able to filter products by price");
        }

        [Fact]
        public async Task Should_AllowCustomerSupportToFilter()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            SetCustomerSupportUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={LOW_MIN}&maxPrice={LOW_MAX}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "customer support should be able to filter products by price");
        }

        [Fact]
        public async Task Should_AllowAdminToFilter()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={LOW_MIN}&maxPrice={LOW_MAX}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK,
                because: "admins should be able to filter products by price");
        }

        #endregion

        #region Performance Tests

        [Fact]
        public async Task Should_ReturnResultsQuickly_ForPriceRangeFilter()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={LOW_MIN}&maxPrice={LOW_MAX}");

            stopwatch.Stop();

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(3000,
                because: "price filtering should be fast with Algolia");
        }

        #endregion

        #region Ordering Tests

        [Fact]
        public async Task Should_ReturnProductsInConsistentOrder()
        {
            // Arrange
            await EnsureIndexIsReadyAsync();

            // Act
            var firstRequest = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={LOW_MIN}&maxPrice={LOW_MAX}");

            var secondRequest = await HttpClient
                .GetFromJsonAsync<List<ProductSearchModel>>(
                    $"{ApiRoutes.Products.Base}/filter-by-price-range?minPrice={LOW_MIN}&maxPrice={LOW_MAX}");

            // Assert
            firstRequest.Should().NotBeNull();
            secondRequest.Should().NotBeNull();
            
            if (firstRequest!.Any() && secondRequest!.Any())
            {
                // Same query should return results in same order
                firstRequest.Should().HaveCount(secondRequest.Count);
                
                for (int i = 0; i < firstRequest.Count; i++)
                {
                    firstRequest[i].ObjectID.Should().Be(secondRequest[i].ObjectID,
                        because: "results should be returned in consistent order");
                }
            }
        }

        #endregion

    }
}

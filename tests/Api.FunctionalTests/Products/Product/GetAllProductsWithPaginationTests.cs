using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Products.Products.GetAllProductsWithPagination;
using FluentAssertions;
using SharedKernel;
using System.Net;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Products.Product
{
    public class GetAllProductsWithPaginationTests : BaseFunctionalTest
    {
        #region Seed Data Constants

        /// <summary>
        /// Total de productos en el seed: 20
        /// IDs: 1-20
        /// - Product 1: Smartphone Galaxy X (Nike, Electronics)
        /// - Product 2: iPhone Pro Max (Adidas, Electronics)
        /// - Product 3: Laptop Ultrabook Pro (Adidas, Computers)
        /// - Product 4: Auriculares Wireless ANC (Puma, Audio)
        /// - Product 5: Smart TV 65 4K (Reebok, TVs)
        /// - Product 6: Tablet Android Plus (Nike, Electronics)
        /// - Product 7: Mouse Inalámbrico Ergo (New Balance, Accessories)
        /// - Product 8: Teclado Mecánico Pro (New Balance, Accessories)
        /// - Product 9: Monitor 27 QHD (Reebok, Monitors)
        /// - Product 10: Disco SSD NVMe 1TB (Under Armour, Storage) - IsDigital: true
        /// - Product 11: Impresora Multifuncional WiFi (Converse, Printers)
        /// - Product 12: Cámara Mirrorless Pro (Columbia, Cameras)
        /// - Product 13: Smartwatch Fitness (Nike, Wearables)
        /// - Product 14: Parlante Bluetooth Portátil (Puma, Audio)
        /// - Product 15: Router WiFi 6 AX (Asics, Networking)
        /// - Product 16: Webcam Full HD (Converse, Accessories)
        /// - Product 17: Consola Gaming NextGen (Fila, Gaming)
        /// - Product 18: Control Inalámbrico Pro (Fila, Gaming)
        /// - Product 19: Silla Gamer Ergonómica (Champion, Furniture)
        /// - Product 20: Escritorio Ajustable Pro (Champion, Furniture)
        /// </summary>
        private const int TOTAL_PRODUCTS_IN_SEED = 20;

        /// <summary>
        /// Product ID 1: Smartphone Galaxy X
        /// Primer producto en el seed, útil para verificar orden
        /// </summary>
        private const long FIRST_PRODUCT_ID = 1;

        /// <summary>
        /// Product ID 20: Escritorio Ajustable Pro
        /// Último producto en el seed, útil para verificar orden
        /// </summary>
        private const long LAST_PRODUCT_ID = 20;

        #endregion

        public GetAllProductsWithPaginationTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageNumberIsLessThan1()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}?pageNumber=0&pageSize=10");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                because: "page number must be at least 1");
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageSizeIsLessThan1()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=0");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                because: "page size must be at least 1");
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageSizeIsGreaterThan100()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=101");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                because: "page size cannot exceed 100");
        }

        [Fact]
        public async Task Should_ReturnOk_AndProducts_WhenPageNumberAndPageSizeAreValid()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<ProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=10");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Should().NotBeEmpty();
            products.Items.Count.Should().BeLessThanOrEqualTo(10);
        }

        [Fact]
        public async Task Should_ReturnCorrectTotalCount_WhenGettingFirstPage()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<ProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=10");

            // Assert
            products.Should().NotBeNull();
            products!.TotalCount.Should().Be(TOTAL_PRODUCTS_IN_SEED,
                because: "there are 20 products in the seed data");
        }

        [Fact]
        public async Task Should_ReturnCorrectTotalPages_WhenPageSizeIs10()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<ProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=10");

            // Assert
            products.Should().NotBeNull();
            products!.TotalPages.Should().Be(2,
                because: "20 products / 10 per page = 2 pages");
        }

        [Fact]
        public async Task Should_ReturnCorrectTotalPages_WhenPageSizeIs5()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<ProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=5");

            // Assert
            products.Should().NotBeNull();
            products!.TotalPages.Should().Be(4,
                because: "20 products / 5 per page = 4 pages");
        }

        [Fact]
        public async Task Should_ReturnFirstPageOf10Products()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<ProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=10");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Count.Should().Be(10);
            products.PageNumber.Should().Be(1);
            products.HasPreviousPage.Should().BeFalse(
                because: "first page has no previous page");
            products.HasNextPage.Should().BeTrue(
                because: "there are more products in page 2");
        }

        [Fact]
        public async Task Should_ReturnSecondPageOf10Products()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<ProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=2&pageSize=10");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Count.Should().Be(10);
            products.PageNumber.Should().Be(2);
            products.HasPreviousPage.Should().BeTrue(
                because: "page 2 has page 1 before it");
            products.HasNextPage.Should().BeFalse(
                because: "there are only 2 pages total");
        }

        [Fact]
        public async Task Should_ReturnAllProducts_WhenPageSizeIs100()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<ProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=100");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Count.Should().Be(TOTAL_PRODUCTS_IN_SEED);
            products.TotalPages.Should().Be(1,
                because: "all products fit in one page");
            products.HasPreviousPage.Should().BeFalse();
            products.HasNextPage.Should().BeFalse();
        }

        [Fact]
        public async Task Should_ReturnEmptyList_WhenPageNumberExceedsTotalPages()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<ProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=100&pageSize=10");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Should().BeEmpty();
            products.TotalCount.Should().Be(TOTAL_PRODUCTS_IN_SEED);
            products.HasPreviousPage.Should().BeTrue();
            products.HasNextPage.Should().BeFalse();
        }

        [Fact]
        public async Task Should_ReturnProductsWithAllProperties()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<ProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=5");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Should().NotBeEmpty();

            ProductResponse firstProduct = products.Items.First();
            firstProduct.Id.Should().BeGreaterThan(0);
            firstProduct.Name.Should().NotBeNullOrEmpty();
            firstProduct.Slug.Should().NotBeNullOrEmpty();
            firstProduct.Brand.Should().NotBeNull();
            firstProduct.Brand!.Name.Should().NotBeNullOrEmpty();
            firstProduct.Category.Should().NotBeNull();
            firstProduct.Category!.Name.Should().NotBeNullOrEmpty();
            firstProduct.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Should_ReturnProductsWithSkus()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Get first page, which includes Product 1 (Smartphone Galaxy X with 3 SKUs)
            PaginatedList<ProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=10");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Should().NotBeEmpty();

            ProductResponse? smartphoneGalaxyX = products.Items.FirstOrDefault(p => p.Id == FIRST_PRODUCT_ID);
            smartphoneGalaxyX.Should().NotBeNull();
            smartphoneGalaxyX!.ProductSkus.Should().HaveCount(3,
                because: "Smartphone Galaxy X has 3 SKUs in seed data");
            smartphoneGalaxyX.ProductSkus.Should().Contain(s => s.SkuCode == "GALX-128-BLK");
        }

        [Fact]
        public async Task Should_ReturnProductsWithImages()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<ProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=10");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Should().NotBeEmpty();

            // Product 1 (Smartphone Galaxy X) has 3 images
            ProductResponse? productWithImages = products.Items.FirstOrDefault(p => p.Id == FIRST_PRODUCT_ID);
            productWithImages.Should().NotBeNull();
            productWithImages!.ProductGalleries.Should().HaveCount(3);
            productWithImages.ProductGalleries.Should().Contain(g => g.IsPrimary == true);
        }

        [Fact]
        public async Task Should_ReturnDifferentProducts_OnDifferentPages()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<ProductResponse>? firstPage = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=5");

            PaginatedList<ProductResponse>? secondPage = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=2&pageSize=5");

            // Assert
            firstPage.Should().NotBeNull();
            secondPage.Should().NotBeNull();

            // Verify that products from first page are not in second page
            List<long> firstPageIds = firstPage!.Items.Select(p => p.Id).ToList();
            List<long> secondPageIds = secondPage!.Items.Select(p => p.Id).ToList();

            firstPageIds.Should().NotIntersectWith(secondPageIds,
                because: "different pages should contain different products");
        }

        [Fact]
        public async Task Should_ReturnProductsInConsistentOrder()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Get same page twice
            PaginatedList<ProductResponse>? firstCall = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=10");

            PaginatedList<ProductResponse>? secondCall = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=10");

            // Assert
            firstCall.Should().NotBeNull();
            secondCall.Should().NotBeNull();
            
            List<long> firstCallIds = firstCall!.Items.Select(p => p.Id).ToList();
            List<long> secondCallIds = secondCall!.Items.Select(p => p.Id).ToList();

            firstCallIds.Should().Equal(secondCallIds,
                because: "pagination should return results in consistent order");
        }

        [Fact]
        public async Task Should_ReturnLastPage_WithRemainingProducts()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Page size 7 creates 3 pages (7, 7, 6)
            PaginatedList<ProductResponse>? lastPage = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=3&pageSize=7");

            // Assert
            lastPage.Should().NotBeNull();
            lastPage!.Items.Count.Should().Be(6,
                because: "20 products with page size 7 leaves 6 products on last page");
            lastPage.PageNumber.Should().Be(3);
            lastPage.TotalPages.Should().Be(3);
            lastPage.HasPreviousPage.Should().BeTrue();
            lastPage.HasNextPage.Should().BeFalse();
        }

        [Fact]
        public async Task Should_ReturnCorrectPageNumbers_AcrossMultiplePages()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<ProductResponse>? page1 = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=5");
            
            PaginatedList<ProductResponse>? page2 = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=2&pageSize=5");
            
            PaginatedList<ProductResponse>? page3 = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=3&pageSize=5");

            // Assert
            page1.Should().NotBeNull();
            page2.Should().NotBeNull();
            page3.Should().NotBeNull();

            page1!.PageNumber.Should().Be(1);
            page2!.PageNumber.Should().Be(2);
            page3!.PageNumber.Should().Be(3);

            // Verify navigation flags
            page1.HasPreviousPage.Should().BeFalse();
            page1.HasNextPage.Should().BeTrue();

            page2.HasPreviousPage.Should().BeTrue();
            page2.HasNextPage.Should().BeTrue();

            page3.HasPreviousPage.Should().BeTrue();
            page3.HasNextPage.Should().BeTrue();
        }

        [Fact]
        public async Task Should_ReturnProducts_WithBrandInformation()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<ProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=10");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Should().NotBeEmpty();
            products.Items.Should().OnlyContain(p => p.Brand != null,
                because: "all products must have a brand");
            products.Items.Should().OnlyContain(p => !string.IsNullOrEmpty(p.Brand!.Name),
                because: "all brands must have a name");
        }

        [Fact]
        public async Task Should_ReturnProducts_WithCategoryInformation()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<ProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=10");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Should().NotBeEmpty();
            products.Items.Should().OnlyContain(p => p.Category != null,
                because: "all products must have a category");
            products.Items.Should().OnlyContain(p => !string.IsNullOrEmpty(p.Category!.Name),
                because: "all categories must have a name");
        }

        [Fact]
        public async Task Should_ReturnActiveProducts()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<ProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=20");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Should().NotBeEmpty();
            products.Items.Should().OnlyContain(p => p.IsActive == true,
                because: "all products in seed data are active");
        }

        [Fact]
        public async Task Should_ReturnProducts_IncludingDigitalProduct()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Page size 20 to get all products including Product 10 (SSD digital)
            PaginatedList<ProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=20");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Should().Contain(p => p.Id == 10,
                because: "Product 10 (Disco SSD NVMe 1TB) is a digital product");
            
            ProductResponse? digitalProduct = products.Items.FirstOrDefault(p => p.Id == 10);
            digitalProduct.Should().NotBeNull();
            digitalProduct!.IsDigital.Should().BeTrue();
        }

        [Fact]
        public async Task Should_ReturnProducts_WithVariedSkuCounts()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<ProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<ProductResponse>>(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=10");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Should().NotBeEmpty();

            // Product 1 has 3 SKUs
            products.Items.Should().Contain(p => p.Id == 1 && p.ProductSkus.Count == 3);
            
            // Product 7 has 1 SKU
            ProductResponse? mouseProduct = products.Items.FirstOrDefault(p => p.Id == 7);
            if (mouseProduct != null)
            {
                mouseProduct.ProductSkus.Count.Should().Be(1);
            }
        }

        [Theory]
        [InlineData(HttpStatusCode.Unauthorized, "no authentication token")]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotAuthenticated(
            HttpStatusCode expectedStatusCode,
            string reason)
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization = null;

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=10");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserIsNotAdmin()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync(
                $"{ApiRoutes.Products.Base}?pageNumber=1&pageSize=10");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

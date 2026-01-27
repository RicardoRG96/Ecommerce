using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Products.Products.GetAllPublishedProductsWithPagination;
using FluentAssertions;
using SharedKernel;
using System.Net;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Products.Product
{
    public class GetAllPublishedProductsWithPaginationTests : BaseFunctionalTest
    {
        #region Seed Data Constants

        /// <summary>
        /// Total de productos en el seed: 20
        /// IMPORTANTE: TODOS los productos están publicados por defecto en el seed
        /// IsPublished = true para todos
        /// 
        /// IDs: 1-20
        /// - Product 1: Smartphone Galaxy X (Nike, Electronics, 3 SKUs, 3 Images)
        /// - Product 2: iPhone Pro Max (Adidas, Electronics, 3 SKUs, 2 Images)
        /// - Product 3: Laptop Ultrabook Pro (Adidas, Computers, 1 SKU, 2 Images)
        /// - Product 4: Auriculares Wireless ANC (Puma, Audio, 2 SKUs, 2 Images)
        /// - Product 5: Smart TV 65 4K (Reebok, TVs, 1 SKU, 1 Image)
        /// - Product 6: Tablet Android Plus (Nike, Electronics, 1 SKU, 2 Images)
        /// - Product 7: Mouse Inalámbrico Ergo (New Balance, Accessories, 1 SKU, 0 Images)
        /// - Product 8: Teclado Mecánico Pro (New Balance, Accessories, 1 SKU, 0 Images)
        /// - Product 9: Monitor 27 QHD (Reebok, Monitors, 1 SKU, 1 Image)
        /// - Product 10: Disco SSD NVMe 1TB (Under Armour, Storage, 1 SKU, 0 Images) - IsDigital: true
        /// - Product 11-20: Otros productos con SKUs e imágenes variadas
        /// </summary>
        private const int TOTAL_PUBLISHED_PRODUCTS_IN_SEED = 20;

        /// <summary>
        /// Productos específicos para tests
        /// </summary>
        private const long SMARTPHONE_GALAXY_X_ID = 1;
        private const long IPHONE_PRO_MAX_ID = 2;
        private const long LAPTOP_ULTRABOOK_PRO_ID = 3;
        private const long SMART_TV_65_4K_ID = 5;
        private const long CAMARA_MIRRORLESS_PRO_ID = 12;

        #endregion

        public GetAllPublishedProductsWithPaginationTests(FunctionalTestWebAppFactory factory) 
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
                $"{ApiRoutes.Products.Base}/published?pageNumber=0&pageSize=10");

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
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=0");

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
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=101");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
                because: "page size cannot exceed 100");
        }

        [Fact]
        public async Task Should_ReturnOk_AndPublishedProducts_WhenPageNumberAndPageSizeAreValid()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Todos los productos están publicados en el seed
            PaginatedList<PublishedProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=10");

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
            PaginatedList<PublishedProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=10");

            // Assert
            products.Should().NotBeNull();
            products!.TotalCount.Should().Be(TOTAL_PUBLISHED_PRODUCTS_IN_SEED,
                because: "all 20 products are published in seed data");
        }

        [Fact]
        public async Task Should_ReturnCorrectTotalPages_WhenPageSizeIs10()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<PublishedProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=10");

            // Assert
            products.Should().NotBeNull();
            products!.TotalPages.Should().Be(2,
                because: "20 published products / 10 per page = 2 pages");
        }

        [Fact]
        public async Task Should_ReturnCorrectTotalPages_WhenPageSizeIs5()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<PublishedProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=5");

            // Assert
            products.Should().NotBeNull();
            products!.TotalPages.Should().Be(4,
                because: "20 published products / 5 per page = 4 pages");
        }

        [Fact]
        public async Task Should_ReturnFirstPageOf10PublishedProducts()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<PublishedProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=10");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Count.Should().Be(10);
            products.PageNumber.Should().Be(1);
            products.HasPreviousPage.Should().BeFalse(
                because: "first page has no previous page");
            products.HasNextPage.Should().BeTrue(
                because: "there are more published products in page 2");
        }

        [Fact]
        public async Task Should_ReturnSecondPageOf10PublishedProducts()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<PublishedProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=2&pageSize=10");

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
        public async Task Should_ReturnAllPublishedProducts_WhenPageSizeIs100()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<PublishedProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=100");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Count.Should().Be(TOTAL_PUBLISHED_PRODUCTS_IN_SEED);
            products.TotalPages.Should().Be(1,
                because: "all published products fit in one page");
            products.HasPreviousPage.Should().BeFalse();
            products.HasNextPage.Should().BeFalse();
        }

        [Fact]
        public async Task Should_ReturnEmptyList_WhenPageNumberExceedsTotalPages()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<PublishedProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=100&pageSize=10");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Should().BeEmpty();
            products.TotalCount.Should().Be(TOTAL_PUBLISHED_PRODUCTS_IN_SEED);
            products.HasPreviousPage.Should().BeTrue();
            products.HasNextPage.Should().BeFalse();
        }

        [Fact]
        public async Task Should_ReturnPublishedProductsWithAllProperties()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<PublishedProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=5");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Should().NotBeEmpty();

            PublishedProductResponse firstProduct = products.Items.First();
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
        public async Task Should_ReturnPublishedProductsWithSkus()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Get first page which includes Product 1 (Smartphone Galaxy X with 3 SKUs)
            PaginatedList<PublishedProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=10");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Should().NotBeEmpty();

            PublishedProductResponse? smartphoneGalaxyX = products.Items.FirstOrDefault(p => p.Id == SMARTPHONE_GALAXY_X_ID);
            smartphoneGalaxyX.Should().NotBeNull();
            smartphoneGalaxyX!.ProductSkus.Should().HaveCount(3,
                because: "Smartphone Galaxy X has 3 SKUs in seed data");
            smartphoneGalaxyX.ProductSkus.Should().Contain(s => s.SkuCode == "GALX-128-BLK");
        }

        [Fact]
        public async Task Should_ReturnPublishedProductsWithImages()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<PublishedProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=10");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Should().NotBeEmpty();

            // Product 1 (Smartphone Galaxy X) has 3 images
            PublishedProductResponse? productWithImages = products.Items.FirstOrDefault(p => p.Id == SMARTPHONE_GALAXY_X_ID);
            productWithImages.Should().NotBeNull();
            productWithImages!.ProductGalleries.Should().HaveCount(3);
            productWithImages.ProductGalleries.Should().Contain(g => g.IsPrimary == true);
        }

        [Fact]
        public async Task Should_ReturnDifferentPublishedProducts_OnDifferentPages()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<PublishedProductResponse>? firstPage = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=5");

            PaginatedList<PublishedProductResponse>? secondPage = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=2&pageSize=5");

            // Assert
            firstPage.Should().NotBeNull();
            secondPage.Should().NotBeNull();

            List<long> firstPageIds = firstPage!.Items.Select(p => p.Id).ToList();
            List<long> secondPageIds = secondPage!.Items.Select(p => p.Id).ToList();

            firstPageIds.Should().NotIntersectWith(secondPageIds,
                because: "different pages should contain different published products");
        }

        [Fact]
        public async Task Should_ReturnPublishedProductsInConsistentOrder()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Get same page twice
            PaginatedList<PublishedProductResponse>? firstCall = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=10");

            PaginatedList<PublishedProductResponse>? secondCall = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=10");

            // Assert
            firstCall.Should().NotBeNull();
            secondCall.Should().NotBeNull();
            
            List<long> firstCallIds = firstCall!.Items.Select(p => p.Id).ToList();
            List<long> secondCallIds = secondCall!.Items.Select(p => p.Id).ToList();

            firstCallIds.Should().Equal(secondCallIds,
                because: "pagination should return results in consistent order");
        }

        [Fact]
        public async Task Should_ReturnLastPage_WithRemainingPublishedProducts()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Page size 7 creates 3 pages (7, 7, 6)
            PaginatedList<PublishedProductResponse>? lastPage = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=3&pageSize=7");

            // Assert
            lastPage.Should().NotBeNull();
            lastPage!.Items.Count.Should().Be(6,
                because: "20 published products with page size 7 leaves 6 products on last page");
            lastPage.PageNumber.Should().Be(3);
            lastPage.TotalPages.Should().Be(3);
            lastPage.HasPreviousPage.Should().BeTrue();
            lastPage.HasNextPage.Should().BeFalse();
        }

        [Fact]
        public async Task Should_ReduceTotalCount_WhenProductIsUnpublished()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - First call with all products published
            PaginatedList<PublishedProductResponse>? productsBeforeUnpublish = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=10");

            // Unpublish one product
            await UnpublishProduct(SMARTPHONE_GALAXY_X_ID);

            // Act - Second call after unpublishing
            PaginatedList<PublishedProductResponse>? productsAfterUnpublish = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=10");

            // Assert
            productsBeforeUnpublish.Should().NotBeNull();
            productsAfterUnpublish.Should().NotBeNull();
            
            productsBeforeUnpublish!.TotalCount.Should().Be(20);
            productsAfterUnpublish!.TotalCount.Should().Be(19);
            productsAfterUnpublish.Items.Should().NotContain(p => p.Id == SMARTPHONE_GALAXY_X_ID);

            // Cleanup - republish for other tests
            await PublishProduct(SMARTPHONE_GALAXY_X_ID);
        }

        [Fact]
        public async Task Should_NotReturnUnpublishedProducts()
        {
            // Arrange
            SetAdminAuthentication();
            
            // Unpublish varios productos
            await UnpublishProduct(SMARTPHONE_GALAXY_X_ID);
            await UnpublishProduct(IPHONE_PRO_MAX_ID);
            await UnpublishProduct(LAPTOP_ULTRABOOK_PRO_ID);

            // Act
            PaginatedList<PublishedProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=20");

            // Assert
            products.Should().NotBeNull();
            products!.TotalCount.Should().Be(17,
                because: "3 products were unpublished");
            products.Items.Should().NotContain(p => p.Id == SMARTPHONE_GALAXY_X_ID);
            products.Items.Should().NotContain(p => p.Id == IPHONE_PRO_MAX_ID);
            products.Items.Should().NotContain(p => p.Id == LAPTOP_ULTRABOOK_PRO_ID);

            // Cleanup - republish for other tests
            await PublishProduct(SMARTPHONE_GALAXY_X_ID);
            await PublishProduct(IPHONE_PRO_MAX_ID);
            await PublishProduct(LAPTOP_ULTRABOOK_PRO_ID);
        }

        [Fact]
        public async Task Should_IncreaseTotalCount_WhenProductIsRepublished()
        {
            // Arrange
            SetAdminAuthentication();
            
            // Unpublish a product first
            await UnpublishProduct(IPHONE_PRO_MAX_ID);

            // Act - Get count after unpublishing
            PaginatedList<PublishedProductResponse>? productsAfterUnpublish = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=10");

            // Republish the product
            await PublishProduct(IPHONE_PRO_MAX_ID);

            // Act - Get count after republishing
            PaginatedList<PublishedProductResponse>? productsAfterRepublish = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=10");

            // Assert
            productsAfterUnpublish.Should().NotBeNull();
            productsAfterRepublish.Should().NotBeNull();
            
            productsAfterUnpublish!.TotalCount.Should().Be(19);
            productsAfterRepublish!.TotalCount.Should().Be(20);
            productsAfterRepublish.Items.Should().Contain(p => p.Id == IPHONE_PRO_MAX_ID);
        }

        [Fact]
        public async Task Should_ReturnPublishedProducts_WithBrandInformation()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<PublishedProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=10");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Should().NotBeEmpty();
            products.Items.Should().OnlyContain(p => p.Brand != null,
                because: "all published products must have a brand");
            products.Items.Should().OnlyContain(p => !string.IsNullOrEmpty(p.Brand!.Name),
                because: "all brands must have a name");
        }

        [Fact]
        public async Task Should_ReturnPublishedProducts_WithCategoryInformation()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<PublishedProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=10");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Should().NotBeEmpty();
            products.Items.Should().OnlyContain(p => p.Category != null,
                because: "all published products must have a category");
            products.Items.Should().OnlyContain(p => !string.IsNullOrEmpty(p.Category!.Name),
                because: "all categories must have a name");
        }

        [Fact]
        public async Task Should_ReturnActivePublishedProducts()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            PaginatedList<PublishedProductResponse>? products = await HttpClient.GetFromJsonAsync<PaginatedList<PublishedProductResponse>>(
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=20");

            // Assert
            products.Should().NotBeNull();
            products!.Items.Should().NotBeEmpty();
            products.Items.Should().OnlyContain(p => p.IsActive == true,
                because: "all products in seed data are active");
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
                $"{ApiRoutes.Products.Base}/published?pageNumber=1&pageSize=10");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }

        #region Helper Methods

        /// <summary>
        /// Publica un producto mediante el endpoint PATCH /products/{id}/publish
        /// </summary>
        private async Task PublishProduct(long productId)
        {
            HttpResponseMessage response = await HttpClient.PatchAsync(
                $"{ApiRoutes.Products.Base}/{productId}/publish", 
                null!);
            
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Despublica un producto mediante el endpoint PATCH /products/{id}/unpublish
        /// </summary>
        private async Task UnpublishProduct(long productId)
        {
            HttpResponseMessage response = await HttpClient.PatchAsync(
                $"{ApiRoutes.Products.Base}/{productId}/unpublish", 
                null!);
            
            response.EnsureSuccessStatusCode();
        }

        #endregion
    }
}

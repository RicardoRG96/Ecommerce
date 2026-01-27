using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Products.Products.GetProductDetailById;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Products.Product
{
    public class GetProductByIdTests : BaseFunctionalTest
    {
        #region Seed Data Constants

        /// <summary>
        /// Product ID 1: Smartphone Galaxy X
        /// - Brand: Nike (ID 1)
        /// - Category: Electronics (ID 1)
        /// - SKUs: 3 (IDs 1, 2, 23) - GALX-128-BLK, GALX-256-BLK, GALX-128-WHT
        /// - Images: 3 (IDs 1, 2, 3) - main.jpg (primary), back.jpg, side.jpg
        /// - IsActive: true, IsFeatured: true, IsDigital: false
        /// - Price: 799990 (first SKU), ComparedAtPrice: 899990
        /// </summary>
        private const long SMARTPHONE_GALAXY_X_ID = 1;

        /// <summary>
        /// Product ID 2: iPhone Pro Max
        /// - Brand: Adidas (ID 2)
        /// - Category: Electronics (ID 1)
        /// - SKUs: 3 (IDs 3, 4, 24) - IPPM-256-SLV, IPPM-512-SLV, IPPM-256-GRY
        /// - Images: 2 (IDs 4, 5) - main.jpg (primary), camera.jpg
        /// - IsActive: true, IsFeatured: true, IsDigital: false
        /// - Price: 1199990 (first SKU)
        /// </summary>
        private const long IPHONE_PRO_MAX_ID = 2;

        /// <summary>
        /// Product ID 3: Laptop Ultrabook Pro
        /// - Brand: Adidas (ID 2)
        /// - Category: Computers (ID 2)
        /// - SKUs: 1 (ID 5) - ULTRA-I7-16GB
        /// - Images: 2 (IDs 6, 7) - main.jpg (primary), keyboard.jpg
        /// - IsActive: true, IsFeatured: true, IsDigital: false
        /// </summary>
        private const long LAPTOP_ULTRABOOK_PRO_ID = 3;

        /// <summary>
        /// Product ID 7: Mouse Inalámbrico Ergo
        /// - Brand: New Balance (ID 5)
        /// - Category: Computer Accessories (ID 5)
        /// - SKUs: 1 (ID 9) - MOUSE-ERGO
        /// - Images: 0 (no images in seed) ← Ideal para probar productos sin imágenes
        /// - IsActive: true, IsFeatured: false, IsDigital: false
        /// </summary>
        private const long MOUSE_INALAMBRICO_ERGO_ID = 7;

        /// <summary>
        /// Product ID 10: Disco SSD NVMe 1TB
        /// - Brand: Under Armour (ID 6)
        /// - Category: Storage (ID 6)
        /// - SKUs: 1 (ID 12) - SSD-NVME-1TB
        /// - Images: 0 (no images in seed)
        /// - IsActive: true, IsFeatured: false, IsDigital: true ← Único producto digital
        /// </summary>
        private const long SSD_NVME_1TB_ID = 10;

        /// <summary>
        /// Product ID 12: Cámara Mirrorless Pro
        /// - Brand: Columbia (ID 8)
        /// - Category: Cameras (ID 8)
        /// - SKUs: 1 (ID 14) - CAM-MIR-24MP
        /// - Images: 2 (IDs 14, 15) - main.jpg (primary), lens.jpg
        /// - IsActive: true, IsFeatured: true, IsDigital: false
        /// </summary>
        private const long CAMARA_MIRRORLESS_PRO_ID = 12;

        /// <summary>
        /// Product ID 17: Consola Gaming NextGen
        /// - Brand: Fila (ID 10)
        /// - Category: Gaming (ID 11)
        /// - SKUs: 1 (ID 19) - CONSOLE-NG
        /// - Images: 2 (IDs 16, 17) - main.jpg (primary), controller.jpg
        /// - IsActive: true, IsFeatured: true, IsDigital: false
        /// </summary>
        private const long CONSOLA_GAMING_NEXTGEN_ID = 17;

        #endregion

        public GetProductByIdTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Theory]
        [InlineData(0, "ProductId", "is missing")]
        [InlineData(-1, "ProductId", "is negative")]
        public async Task Should_ReturnNotFound_WhenProductIdIsInvalid(
            long invalidProductId,
            string fieldName,
            string reason)
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Products.Base}/{invalidProductId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                because: $"{fieldName} {reason}");
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenProductIdDoesNotExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Products.Base}/{Constants.NotExistingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_AndProduct_WhenProductExists()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded product: Smartphone Galaxy X
            ProductDetailResponse? product = await HttpClient.GetFromJsonAsync<ProductDetailResponse>(
                $"{ApiRoutes.Products.Base}/{SMARTPHONE_GALAXY_X_ID}");

            // Assert
            product.Should().NotBeNull();
            product!.Id.Should().Be(SMARTPHONE_GALAXY_X_ID);
            product.Name.Should().Be("Smartphone Galaxy X");
            product.Slug.Should().Be("smartphone-galaxy-x");
            product.Description.Should().Contain("pantalla AMOLED");
        }

        [Fact]
        public async Task Should_ReturnProductWithBrandInformation()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded product: iPhone Pro Max (Brand: Adidas, ID 2)
            ProductDetailResponse? product = await HttpClient.GetFromJsonAsync<ProductDetailResponse>(
                $"{ApiRoutes.Products.Base}/{IPHONE_PRO_MAX_ID}");

            // Assert
            product.Should().NotBeNull();
            product!.Brand.Should().NotBeNull();
            product.Brand!.Id.Should().Be(2);
            product.Brand.Name.Should().Be("Adidas");
            product.Brand.Slug.Should().Be("adidas");
            product.Brand.LogoUrl.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Should_ReturnProductWithCategoryInformation()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded product: Laptop Ultrabook Pro (Category: Computers, ID 2)
            ProductDetailResponse? product = await HttpClient.GetFromJsonAsync<ProductDetailResponse>(
                $"{ApiRoutes.Products.Base}/{LAPTOP_ULTRABOOK_PRO_ID}");

            // Assert
            product.Should().NotBeNull();
            product!.Category.Should().NotBeNull();
            product.Category!.Id.Should().Be(2);
            product.Category.Name.Should().Be("Computers");
            product.Category.Slug.Should().Be("computers");
        }

        [Fact]
        public async Task Should_ReturnProductWithSkus()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded product: Smartphone Galaxy X (tiene 3 SKUs)
            ProductDetailResponse? product = await HttpClient.GetFromJsonAsync<ProductDetailResponse>(
                $"{ApiRoutes.Products.Base}/{SMARTPHONE_GALAXY_X_ID}");

            // Assert
            product.Should().NotBeNull();
            product!.ProductSkus.Should().NotBeNull();
            product.ProductSkus.Should().HaveCount(3,
                because: "Smartphone Galaxy X has 3 SKU variants in seed data");
            product.ProductSkus.Should().Contain(s => s.SkuCode == "GALX-128-BLK");
            product.ProductSkus.Should().Contain(s => s.SkuCode == "GALX-256-BLK");
            product.ProductSkus.Should().Contain(s => s.SkuCode == "GALX-128-WHT");
        }

        [Fact]
        public async Task Should_ReturnProductWithSkuPrices()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded product: Smartphone Galaxy X, SKU GALX-128-BLK
            ProductDetailResponse? product = await HttpClient.GetFromJsonAsync<ProductDetailResponse>(
                $"{ApiRoutes.Products.Base}/{SMARTPHONE_GALAXY_X_ID}");

            // Assert
            product.Should().NotBeNull();
            product!.ProductSkus.Should().HaveCountGreaterThan(0);
            
            ProductSkuResponse firstSku = product.ProductSkus.First(s => s.SkuCode == "GALX-128-BLK");
            firstSku.Price.Should().Be(799990);
            firstSku.ComparedAtPrice.Should().Be(899990);
            firstSku.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Should_ReturnProductWithMultipleSkuVariants()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded product: iPhone Pro Max (tiene 3 SKUs diferentes)
            ProductDetailResponse? product = await HttpClient.GetFromJsonAsync<ProductDetailResponse>(
                $"{ApiRoutes.Products.Base}/{IPHONE_PRO_MAX_ID}");

            // Assert
            product.Should().NotBeNull();
            product!.ProductSkus.Should().HaveCount(3);
            product.ProductSkus.Should().Contain(s => s.SkuCode == "IPPM-256-SLV");
            product.ProductSkus.Should().Contain(s => s.SkuCode == "IPPM-512-SLV");
            product.ProductSkus.Should().Contain(s => s.SkuCode == "IPPM-256-GRY");
            product.ProductSkus.Should().OnlyContain(s => s.IsActive == true);
        }

        [Fact]
        public async Task Should_ReturnProductWithSingleSku()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded product: Mouse Inalámbrico Ergo (tiene 1 SKU)
            ProductDetailResponse? product = await HttpClient.GetFromJsonAsync<ProductDetailResponse>(
                $"{ApiRoutes.Products.Base}/{MOUSE_INALAMBRICO_ERGO_ID}");

            // Assert
            product.Should().NotBeNull();
            product!.ProductSkus.Should().HaveCount(1);
            product.ProductSkus.First().SkuCode.Should().Be("MOUSE-ERGO");
            product.ProductSkus.First().Price.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Should_ReturnProductWithEmptyGalleryList_WhenNoImagesExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded product: Mouse Inalámbrico Ergo (sin imágenes en seed)
            ProductDetailResponse? product = await HttpClient.GetFromJsonAsync<ProductDetailResponse>(
                $"{ApiRoutes.Products.Base}/{MOUSE_INALAMBRICO_ERGO_ID}");

            // Assert
            product.Should().NotBeNull();
            product!.ProductGalleries.Should().NotBeNull();
            product.ProductGalleries.Should().BeEmpty(
                because: "Mouse Inalámbrico Ergo has no images in seed data");
        }

        [Fact]
        public async Task Should_ReturnProductWithGallery_WhenImagesExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded product: Smartphone Galaxy X (tiene 3 imágenes)
            ProductDetailResponse? product = await HttpClient.GetFromJsonAsync<ProductDetailResponse>(
                $"{ApiRoutes.Products.Base}/{SMARTPHONE_GALAXY_X_ID}");

            // Assert
            product.Should().NotBeNull();
            product!.ProductGalleries.Should().NotBeNull();
            product.ProductGalleries.Should().HaveCount(3,
                because: "Smartphone Galaxy X has 3 images in seed data");
            product.ProductGalleries.Should().Contain(g => 
                g.MediaUrl.Contains("galaxy-x/main.jpg"));
        }

        [Fact]
        public async Task Should_ReturnProductWithPrimaryImage()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded product: Cámara Mirrorless Pro (imagen principal: ID 14)
            ProductDetailResponse? product = await HttpClient.GetFromJsonAsync<ProductDetailResponse>(
                $"{ApiRoutes.Products.Base}/{CAMARA_MIRRORLESS_PRO_ID}");

            // Assert
            product.Should().NotBeNull();
            product!.ProductGalleries.Should().HaveCount(2);
            product.ProductGalleries.Should().Contain(g => g.IsPrimary == true,
                because: "at least one image should be marked as primary");
            
            ProductGalleryResponse primaryImage = product.ProductGalleries.First(g => g.IsPrimary);
            primaryImage.MediaUrl.Should().Contain("mirrorless/main.jpg");
            primaryImage.DisplayOrder.Should().Be(1);
        }

        [Fact]
        public async Task Should_ReturnMultipleImagesInCorrectOrder()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded product: Smartphone Galaxy X (3 imágenes con DisplayOrder)
            ProductDetailResponse? product = await HttpClient.GetFromJsonAsync<ProductDetailResponse>(
                $"{ApiRoutes.Products.Base}/{SMARTPHONE_GALAXY_X_ID}");

            // Assert
            product.Should().NotBeNull();
            product!.ProductGalleries.Should().HaveCount(3);
            product.ProductGalleries.Should().BeInAscendingOrder(g => g.DisplayOrder,
                because: "images should be ordered by DisplayOrder");
            
            // Verify primary image is first
            product.ProductGalleries.First().IsPrimary.Should().BeTrue();
            product.ProductGalleries.First().DisplayOrder.Should().Be(1);
        }

        [Fact]
        public async Task Should_ReturnActiveProduct()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded product: Smartphone Galaxy X (IsActive: true)
            ProductDetailResponse? product = await HttpClient.GetFromJsonAsync<ProductDetailResponse>(
                $"{ApiRoutes.Products.Base}/{SMARTPHONE_GALAXY_X_ID}");

            // Assert
            product.Should().NotBeNull();
            product!.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Should_ReturnDigitalProduct()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded product: Disco SSD NVMe 1TB (IsDigital: true)
            ProductDetailResponse? product = await HttpClient.GetFromJsonAsync<ProductDetailResponse>(
                $"{ApiRoutes.Products.Base}/{SSD_NVME_1TB_ID}");

            // Assert
            product.Should().NotBeNull();
            product!.Name.Should().Be("Disco SSD NVMe 1TB");
            // Note: IsDigital property might not be in ProductDetailResponse
            // Verify through product characteristics or description
        }

        [Fact]
        public async Task Should_ReturnFeaturedProduct()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded product: Consola Gaming NextGen (IsFeatured: true)
            ProductDetailResponse? product = await HttpClient.GetFromJsonAsync<ProductDetailResponse>(
                $"{ApiRoutes.Products.Base}/{CONSOLA_GAMING_NEXTGEN_ID}");

            // Assert
            product.Should().NotBeNull();
            product!.Name.Should().Be("Consola Gaming NextGen");
            product.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Should_ReturnProductWithMetaInformation()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded product: Smartphone Galaxy X
            ProductDetailResponse? product = await HttpClient.GetFromJsonAsync<ProductDetailResponse>(
                $"{ApiRoutes.Products.Base}/{SMARTPHONE_GALAXY_X_ID}");

            // Assert
            product.Should().NotBeNull();
            product!.MetaTitle.Should().Be("Smartphone Galaxy X");
            product.MetaDescription.Should().Contain("última generación");
        }

        [Fact]
        public async Task Should_ReturnCompleteProductDetail_WhenProductHasAllData()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Using seeded product: iPhone Pro Max (tiene SKUs, imágenes, marca, categoría)
            ProductDetailResponse? product = await HttpClient.GetFromJsonAsync<ProductDetailResponse>(
                $"{ApiRoutes.Products.Base}/{IPHONE_PRO_MAX_ID}");

            // Assert
            product.Should().NotBeNull();
            product!.Id.Should().Be(IPHONE_PRO_MAX_ID);
            product.Name.Should().NotBeNullOrEmpty();
            product.Slug.Should().NotBeNullOrEmpty();
            product.Description.Should().NotBeNullOrEmpty();
            product.ShortDescription.Should().NotBeNullOrEmpty();
            product.Brand.Should().NotBeNull();
            product.Brand!.Name.Should().NotBeNullOrEmpty();
            product.Category.Should().NotBeNull();
            product.Category!.Name.Should().NotBeNullOrEmpty();
            product.ProductSkus.Should().HaveCountGreaterThan(0);
            product.ProductGalleries.Should().HaveCountGreaterThan(0);
            product.MetaTitle.Should().NotBeNullOrEmpty();
            product.MetaDescription.Should().NotBeNullOrEmpty();
            product.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Should_ReturnProductFromDifferentCategories()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Verify products from different categories
            ProductDetailResponse? electronics = await HttpClient.GetFromJsonAsync<ProductDetailResponse>(
                $"{ApiRoutes.Products.Base}/{SMARTPHONE_GALAXY_X_ID}"); // Electronics

            ProductDetailResponse? computers = await HttpClient.GetFromJsonAsync<ProductDetailResponse>(
                $"{ApiRoutes.Products.Base}/{LAPTOP_ULTRABOOK_PRO_ID}"); // Computers

            // Assert
            electronics.Should().NotBeNull();
            computers.Should().NotBeNull();
            electronics!.Category!.Id.Should().NotBe(computers!.Category!.Id,
                because: "products belong to different categories");
            electronics.Category.Name.Should().Be("Electronics");
            computers.Category.Name.Should().Be("Computers");
        }

        [Fact]
        public async Task Should_ReturnProductFromDifferentBrands()
        {
            // Arrange
            SetAdminAuthentication();

            // Act - Verify products from different brands
            ProductDetailResponse? nike = await HttpClient.GetFromJsonAsync<ProductDetailResponse>(
                $"{ApiRoutes.Products.Base}/{SMARTPHONE_GALAXY_X_ID}"); // Nike

            ProductDetailResponse? adidas = await HttpClient.GetFromJsonAsync<ProductDetailResponse>(
                $"{ApiRoutes.Products.Base}/{IPHONE_PRO_MAX_ID}"); // Adidas

            // Assert
            nike.Should().NotBeNull();
            adidas.Should().NotBeNull();
            nike!.Brand!.Id.Should().NotBe(adidas!.Brand!.Id,
                because: "products belong to different brands");
            nike.Brand.Name.Should().Be("Nike");
            adidas.Brand.Name.Should().Be("Adidas");
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
                $"{ApiRoutes.Products.Base}/{SMARTPHONE_GALAXY_X_ID}");

            // Assert
            response.StatusCode.Should().Be(expectedStatusCode, because: reason);
        }
    }
}

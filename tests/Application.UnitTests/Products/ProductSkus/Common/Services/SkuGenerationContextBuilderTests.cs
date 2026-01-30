using Application.Abstractions.Data.Repositories.Products;
using Application.Products.ProductSkus.Common.Services;
using Domain.Entities.Products;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTests.Products.ProductSkus.Common.Services
{
    public sealed class SkuGenerationContextBuilderTests
    {
        private readonly SkuGenerationContextBuilder _sut;
        private readonly IProductRepository _productRepositoryMock;
        private readonly ICategoryRepository _categoryRepositoryMock;
        private readonly IBrandRepository _brandRepositoryMock;
        private readonly IAttributeValueRepository _attributeValueRepositoryMock;

        public SkuGenerationContextBuilderTests()
        {
            _productRepositoryMock = Substitute.For<IProductRepository>();
            _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
            _brandRepositoryMock = Substitute.For<IBrandRepository>();
            _attributeValueRepositoryMock = Substitute.For<IAttributeValueRepository>();

            _sut = new SkuGenerationContextBuilder(
                _productRepositoryMock,
                _categoryRepositoryMock,
                _brandRepositoryMock,
                _attributeValueRepositoryMock);
        }

        #region BuildForProductAsync - Success Cases

        [Fact]
        public async Task BuildForProductAsync_WithValidProductId_ShouldReturnContextWithoutVariants()
        {
            // Arrange
            const long productId = 123;
            const long categoryId = 1;
            const long brandId = 2;

            var product = Product.Create(
                name: "Samsung Galaxy S24",
                description: "Latest smartphone",
                shortDescription: "Flagship phone",
                brandId: brandId,
                categoryId: categoryId,
                productTaxCategoryId: 1,
                isActive: true,
                isFeatured: false,
                isDigital: false,
                metaTitle: "Samsung Galaxy S24",
                metaDescription: "Latest smartphone",
                metaKeywords: "samsung, galaxy, smartphone");

            var category = Category.Create(
                parentId: null,
                name: "Electronics & Gadgets",
                description: "Tech products",
                imageUrl: null,
                icon: null,
                isActive: true,
                isVisibleInMenu: true,
                displayOrder: 1);

            var brand = Brand.Create(
                name: "Samsung",
                description: "Korean tech company",
                logoUrl: null,
                bannerUrl: null,
                websiteUrl: null,
                isActive: true,
                isFeatured: false,
                displayOrder: 1);

            _productRepositoryMock
                .GetByIdAsync(productId, Arg.Any<CancellationToken>())
                .Returns(product);

            _categoryRepositoryMock
                .GetByIdAsync(categoryId, Arg.Any<CancellationToken>())
                .Returns(category);

            _brandRepositoryMock
                .GetByIdAsync(brandId, Arg.Any<CancellationToken>())
                .Returns(brand);

            // Act
            var result = await _sut.BuildForProductAsync(productId, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.ProductId.Should().Be(productId);
            result.CategoryCode.Should().Be("ELGA"); // First letters of words
            result.BrandCode.Should().Be("SAMS");
            result.ProductCode.Should().NotBeNullOrWhiteSpace();
            result.Variants.Should().BeNull();
            result.GeneratedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task BuildForProductAsync_WithSingleWordCategory_ShouldExtractCorrectly()
        {
            // Arrange
            const long productId = 123;
            const long categoryId = 1;
            const long brandId = 2;

            var product = Product.Create("Product", "desc", "short", brandId, categoryId, 1, true, false, false, "title", "desc", "keywords");
            var category = Category.Create(null, "Electronics", "desc");
            var brand = Brand.Create("Samsung", "desc");

            _productRepositoryMock.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
            _categoryRepositoryMock.GetByIdAsync(categoryId, Arg.Any<CancellationToken>()).Returns(category);
            _brandRepositoryMock.GetByIdAsync(brandId, Arg.Any<CancellationToken>()).Returns(brand);

            // Act
            var result = await _sut.BuildForProductAsync(productId, CancellationToken.None);

            // Assert
            result.CategoryCode.Should().Be("ELEC"); // First 4 letters
        }

        [Fact]
        public async Task BuildForProductAsync_WithTwoWordCategory_ShouldCombineCorrectly()
        {
            // Arrange
            const long productId = 123;
            const long categoryId = 1;
            const long brandId = 2;

            var product = Product.Create("Product", "desc", "short", brandId, categoryId, 1, true, false, false, "title", "desc", "keywords");
            var category = Category.Create(null, "Home Appliances", "desc");
            var brand = Brand.Create("Samsung", "desc");

            _productRepositoryMock.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
            _categoryRepositoryMock.GetByIdAsync(categoryId, Arg.Any<CancellationToken>()).Returns(category);
            _brandRepositoryMock.GetByIdAsync(brandId, Arg.Any<CancellationToken>()).Returns(brand);

            // Act
            var result = await _sut.BuildForProductAsync(productId, CancellationToken.None);

            // Assert
            result.CategoryCode.Should().Be("HOAP"); // First 2 letters of each word
        }

        [Fact]
        public async Task BuildForProductAsync_WithThreeOrMoreWordCategory_ShouldUseFirstLetters()
        {
            // Arrange
            const long productId = 123;
            const long categoryId = 1;
            const long brandId = 2;

            var product = Product.Create("Product", "desc", "short", brandId, categoryId, 1, true, false, false, "title", "desc", "keywords");
            var category = Category.Create(null, "Electronics & Home Appliances", "desc");
            var brand = Brand.Create("Samsung", "desc");

            _productRepositoryMock.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
            _categoryRepositoryMock.GetByIdAsync(categoryId, Arg.Any<CancellationToken>()).Returns(category);
            _brandRepositoryMock.GetByIdAsync(brandId, Arg.Any<CancellationToken>()).Returns(brand);

            // Act
            var result = await _sut.BuildForProductAsync(productId, CancellationToken.None);

            // Assert
            result.CategoryCode.Should().Be("EHA"); // First letter of first 3 words
        }

        #endregion

        #region BuildForProductAsync - Failure Cases

        [Fact]
        public async Task BuildForProductAsync_WithNonExistentProduct_ShouldThrowInvalidOperationException()
        {
            // Arrange
            const long productId = 999;

            _productRepositoryMock
                .GetByIdAsync(productId, Arg.Any<CancellationToken>())
                .Returns((Product?)null);

            // Act
            Func<Task> act = async () => await _sut.BuildForProductAsync(productId, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"*{productId}*");
        }

        [Fact]
        public async Task BuildForProductAsync_WithNonExistentCategory_ShouldThrowInvalidOperationException()
        {
            // Arrange
            const long productId = 123;
            const long categoryId = 999;
            const long brandId = 2;

            var product = Product.Create("Product", "desc", "short", brandId, categoryId, 1, true, false, false, "title", "desc", "keywords");

            _productRepositoryMock
                .GetByIdAsync(productId, Arg.Any<CancellationToken>())
                .Returns(product);

            _categoryRepositoryMock
                .GetByIdAsync(categoryId, Arg.Any<CancellationToken>())
                .Returns((Category?)null);

            // Act
            Func<Task> act = async () => await _sut.BuildForProductAsync(productId, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"*{categoryId}*");
        }

        [Fact]
        public async Task BuildForProductAsync_WithNonExistentBrand_ShouldThrowInvalidOperationException()
        {
            // Arrange
            const long productId = 123;
            const long categoryId = 1;
            const long brandId = 999;

            var product = Product.Create("Product", "desc", "short", brandId, categoryId, 1, true, false, false, "title", "desc", "keywords");
            var category = Category.Create(null, "Electronics", "desc");

            _productRepositoryMock
                .GetByIdAsync(productId, Arg.Any<CancellationToken>())
                .Returns(product);

            _categoryRepositoryMock
                .GetByIdAsync(categoryId, Arg.Any<CancellationToken>())
                .Returns(category);

            _brandRepositoryMock
                .GetByIdAsync(brandId, Arg.Any<CancellationToken>())
                .Returns((Brand?)null);

            // Act
            Func<Task> act = async () => await _sut.BuildForProductAsync(productId, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"*{brandId}*");
        }

        #endregion

        #region BuildWithAttributesAsync - Success Cases

        [Fact]
        public async Task BuildWithAttributesAsync_WithValidAttributeValueIds_ShouldReturnContextWithVariants()
        {
            // Arrange
            const long productId = 123;
            const long categoryId = 1;
            const long brandId = 2;

            var product = Product.Create("Product", "desc", "short", brandId, categoryId, 1, true, false, false, "title", "desc", "keywords");
            var category = Category.Create(null, "Electronics", "desc");
            var brand = Brand.Create("Samsung", "desc");

            var attributeValueIds = new[] { 1L, 2L };
            var attributeValues = new Dictionary<string, string>
            {
                { "Color", "Black" },
                { "Size", "XL" }
            };

            _productRepositoryMock.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
            _categoryRepositoryMock.GetByIdAsync(categoryId, Arg.Any<CancellationToken>()).Returns(category);
            _brandRepositoryMock.GetByIdAsync(brandId, Arg.Any<CancellationToken>()).Returns(brand);
            _attributeValueRepositoryMock
                .GetByIdsAsync(Arg.Is<IEnumerable<long>>(ids => ids.SequenceEqual(attributeValueIds)), Arg.Any<CancellationToken>())
                .Returns(attributeValues);

            // Act
            var result = await _sut.BuildWithAttributesAsync(productId, attributeValueIds, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Variants.Should().NotBeNull();
            result.Variants.Should().HaveCount(2);
            result.Variants.Should().ContainKey("Color");
            result.Variants.Should().ContainKey("Size");
        }

        [Fact]
        public async Task BuildWithAttributesAsync_WithEmptyAttributeValueIds_ShouldReturnContextWithNullVariants()
        {
            // Arrange
            const long productId = 123;
            const long categoryId = 1;
            const long brandId = 2;

            var product = Product.Create("Product", "desc", "short", brandId, categoryId, 1, true, false, false, "title", "desc", "keywords");
            var category = Category.Create(null, "Electronics", "desc");
            var brand = Brand.Create("Samsung", "desc");

            var attributeValueIds = Enumerable.Empty<long>();

            _productRepositoryMock.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
            _categoryRepositoryMock.GetByIdAsync(categoryId, Arg.Any<CancellationToken>()).Returns(category);
            _brandRepositoryMock.GetByIdAsync(brandId, Arg.Any<CancellationToken>()).Returns(brand);

            // Act
            var result = await _sut.BuildWithAttributesAsync(productId, attributeValueIds, CancellationToken.None);

            // Assert
            result.Variants.Should().BeNull();
        }

        [Fact]
        public async Task BuildWithAttributesAsync_WithNonExistentAttributeValues_ShouldReturnContextWithNullVariants()
        {
            // Arrange
            const long productId = 123;
            const long categoryId = 1;
            const long brandId = 2;

            var product = Product.Create("Product", "desc", "short", brandId, categoryId, 1, true, false, false, "title", "desc", "keywords");
            var category = Category.Create(null, "Electronics", "desc");
            var brand = Brand.Create("Samsung", "desc");

            var attributeValueIds = new[] { 999L, 888L };
            var emptyDictionary = new Dictionary<string, string>();

            _productRepositoryMock.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
            _categoryRepositoryMock.GetByIdAsync(categoryId, Arg.Any<CancellationToken>()).Returns(category);
            _brandRepositoryMock.GetByIdAsync(brandId, Arg.Any<CancellationToken>()).Returns(brand);
            _attributeValueRepositoryMock
                .GetByIdsAsync(Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>())
                .Returns(emptyDictionary);

            // Act
            var result = await _sut.BuildWithAttributesAsync(productId, attributeValueIds, CancellationToken.None);

            // Assert
            result.Variants.Should().BeNull();
        }

        #endregion

        #region Repository Interaction Verification

        [Fact]
        public async Task BuildForProductAsync_ShouldCallProductRepositoryOnce()
        {
            // Arrange
            const long productId = 123;
            const long categoryId = 1;
            const long brandId = 2;

            var product = Product.Create("Product", "desc", "short", brandId, categoryId, 1, true, false, false, "title", "desc", "keywords");
            var category = Category.Create(null, "Electronics", "desc");
            var brand = Brand.Create("Samsung", "desc");

            _productRepositoryMock.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
            _categoryRepositoryMock.GetByIdAsync(categoryId, Arg.Any<CancellationToken>()).Returns(category);
            _brandRepositoryMock.GetByIdAsync(brandId, Arg.Any<CancellationToken>()).Returns(brand);

            // Act
            await _sut.BuildForProductAsync(productId, CancellationToken.None);

            // Assert
            await _productRepositoryMock
                .Received(1)
                .GetByIdAsync(productId, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task BuildWithAttributesAsync_ShouldCallAttributeValueRepository()
        {
            // Arrange
            const long productId = 123;
            const long categoryId = 1;
            const long brandId = 2;

            var product = Product.Create("Product", "desc", "short", brandId, categoryId, 1, true, false, false, "title", "desc", "keywords");
            var category = Category.Create(null, "Electronics", "desc");
            var brand = Brand.Create("Samsung", "desc");

            var attributeValueIds = new[] { 1L, 2L };
            var attributeValues = new Dictionary<string, string> { { "Color", "Black" } };

            _productRepositoryMock.GetByIdAsync(productId, Arg.Any<CancellationToken>()).Returns(product);
            _categoryRepositoryMock.GetByIdAsync(categoryId, Arg.Any<CancellationToken>()).Returns(category);
            _brandRepositoryMock.GetByIdAsync(brandId, Arg.Any<CancellationToken>()).Returns(brand);
            _attributeValueRepositoryMock
                .GetByIdsAsync(Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>())
                .Returns(attributeValues);

            // Act
            await _sut.BuildWithAttributesAsync(productId, attributeValueIds, CancellationToken.None);

            // Assert
            await _attributeValueRepositoryMock
                .Received(1)
                .GetByIdsAsync(Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>());
        }

        #endregion
    }
}
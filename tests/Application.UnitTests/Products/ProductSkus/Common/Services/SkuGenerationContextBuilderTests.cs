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
                slug: "samsung-galaxy-s24",
                description: "Latest smartphone",
                shortDescription: "Flagship phone",
                brandId: brandId,
                categoryId: categoryId,
                productTaxCategoryId: 1,
                isActive: true,
                isFeatured: false,
                isDigital: false);

            var category = Category.Create(
                name: "Electronics & Gadgets",
                slug: "electronics-gadgets",
                description: "Tech products",
                parentId: null,
                imageUrl: null,
                displayOrder: 1,
                isActive: true);

            var brand = Brand.Create(
                name: "Samsung",
                slug: "samsung",
                description: "Korean tech company",
                logoUrl: null,
                displayOrder: 1,
                isActive: true);

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
            result.CategoryCode.Should().Be("EGA"); // First letters of words
            result.BrandCode.Should().Be("SAMS");
            result.ProductCode.Should().Be("samsung-galaxy-s24");
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

            var product = Product.Create("Product", "slug", "desc", "short", brandId, categoryId, 1, true, false, false);
            var category = Category.Create("Electronics", "electronics", "desc", null, null, 1, true);
            var brand = Brand.Create("Samsung", "samsung", "desc", null, 1, true);

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

            var product = Product.Create("Product", "slug", "desc", "short", brandId, categoryId, 1, true, false, false);
            var category = Category.Create("Home Appliances", "home-appliances", "desc", null, null, 1, true);
            var brand = Brand.Create("Samsung", "samsung", "desc", null, 1, true);

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

            var product = Product.Create("Product", "slug", "desc", "short", brandId, categoryId, 1, true, false, false);
            var category = Category.Create("Electronics & Home Appliances", "electronics-home-appliances", "desc", null, null, 1, true);
            var brand = Brand.Create("Samsung", "desc", null, 1, true);

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

            var product = Product.Create("Product", "slug", "desc", "short", brandId, categoryId, 1, true, false, false);

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

            var product = Product.Create("Product", "slug", "desc", "short", brandId, categoryId, 1, true, false, false);
            var category = Category.Create("Electronics", "electronics", "desc", null, null, 1, true);

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

            var product = Product.Create("Product", "slug", "desc", "short", brandId, categoryId, 1, true, false, false);
            var category = Category.Create("Electronics", "electronics", "desc", null, null, 1, true);
            var brand = Brand.Create("Samsung", "samsung", "desc", null, 1, true);

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

            var product = Product.Create("Product", "slug", "desc", "short", brandId, categoryId, 1, true, false, false);
            var category = Category.Create("Electronics", "electronics", "desc", null, null, 1, true);
            var brand = Brand.Create("Samsung", "samsung", "desc", null, 1, true);

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

            var product = Product.Create("Product", "slug", "desc", "short", brandId, categoryId, 1, true, false, false);
            var category = Category.Create("Electronics", "electronics", "desc", null, null, 1, true);
            var brand = Brand.Create("Samsung", "samsung", "desc", null, 1, true);

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

            var product = Product.Create("Product", "slug", "desc", "short", brandId, categoryId, 1, true, false, false);
            var category = Category.Create("Electronics", "electronics", "desc", null, null, 1, true);
            var brand = Brand.Create("Samsung", "samsung", "desc", null, 1, true);

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

            var product = Product.Create("Product", "slug", "desc", "short", brandId, categoryId, 1, true, false, false);
            var category = Category.Create("Electronics", "electronics", "desc", null, null, 1, true);
            var brand = Brand.Create("Samsung", "samsung", "desc", null, 1, true);

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
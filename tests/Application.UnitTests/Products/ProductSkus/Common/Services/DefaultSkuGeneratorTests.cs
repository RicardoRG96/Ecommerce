using Application.Products.ProductSkus.Common.Services;
using FluentAssertions;

namespace Application.UnitTests.Products.ProductSkus.Common.Services
{
    public sealed class DefaultSkuGeneratorTests
    {
        private readonly DefaultSkuGenerator _sut;

        public DefaultSkuGeneratorTests()
        {
            _sut = new DefaultSkuGenerator();
        }

        #region Generate - Success Cases

        [Fact]
        public void Generate_WithValidContextWithoutVariants_ShouldReturnFormattedSku()
        {
            // Arrange
            var context = new SkuGenerationContext
            {
                ProductSkuId = 1,
                ProductId = 123,
                CategoryCode = "Electronics",
                BrandCode = "Samsung",
                ProductCode = "galaxy-s24",
                Variants = null,
                GeneratedAt = new DateTime(2026, 1, 29, 12, 0, 0, DateTimeKind.Utc)
            };

            // Act
            string result = _sut.Generate(context);

            // Assert
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().MatchRegex(@"^[A-Z0-9]+-[A-Z0-9]+-\d{6}-[A-Z0-9]{4}$");
            result.Should().StartWith("ELE-SAMS-000123-");
            result.Split('-').Should().HaveCount(4);
        }

        [Fact]
        public void Generate_WithValidContextWithVariants_ShouldIncludeVariantCodes()
        {
            // Arrange
            var context = new SkuGenerationContext
            {
                ProductSkuId = 1,
                ProductId = 123,
                CategoryCode = "Electronics",
                BrandCode = "Samsung",
                ProductCode = "galaxy-s24",
                Variants = new Dictionary<string, string>
                {
                    { "Color", "Black" },
                    { "Size", "XL" }
                },
                GeneratedAt = new DateTime(2026, 1, 29, 12, 0, 0, DateTimeKind.Utc)
            };

            // Act
            string result = _sut.Generate(context);

            // Assert
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().Contain("BLA");
            result.Should().Contain("XL0");
            result.Split('-').Should().HaveCountGreaterThan(4);
        }

        [Fact]
        public void Generate_WithSingleCharacterBrandCode_ShouldPadCorrectly()
        {
            // Arrange
            var context = new SkuGenerationContext
            {
                ProductSkuId = 1,
                ProductId = 123,
                CategoryCode = "Electronics",
                BrandCode = "A",
                GeneratedAt = DateTime.UtcNow
            };

            // Act
            string result = _sut.Generate(context);

            // Assert
            result.Should().Contain("A000");
        }

        [Fact]
        public void Generate_WithSpecialCharactersInCodes_ShouldRemoveSpecialCharacters()
        {
            // Arrange
            var context = new SkuGenerationContext
            {
                ProductSkuId = 1,
                ProductId = 123,
                CategoryCode = "Electrónic$",
                BrandCode = "Samsung®",
                GeneratedAt = DateTime.UtcNow
            };

            // Act
            string result = _sut.Generate(context);

            // Assert
            result.Should().NotContain("ó");
            result.Should().NotContain("$");
            result.Should().NotContain("®");
            result.Should().MatchRegex(@"^[A-Z0-9-]+$");
        }

        [Fact]
        public void Generate_WithLongCategoryName_ShouldTruncateToMaxLength()
        {
            // Arrange
            var context = new SkuGenerationContext
            {
                ProductSkuId = 1,
                ProductId = 123,
                CategoryCode = "ElectronicsAndGadgets",
                BrandCode = "SamsungElectronics",
                GeneratedAt = DateTime.UtcNow
            };

            // Act
            string result = _sut.Generate(context);

            // Assert
            var parts = result.Split('-');
            parts[0].Length.Should().Be(3); // Category max length
            parts[1].Length.Should().Be(4); // Brand max length
        }

        [Fact]
        public void Generate_WithEmptyVariantsDictionary_ShouldNotIncludeVariantPart()
        {
            // Arrange
            var context = new SkuGenerationContext
            {
                ProductSkuId = 1,
                ProductId = 123,
                CategoryCode = "Electronics",
                BrandCode = "Samsung",
                Variants = new Dictionary<string, string>(), // Empty
                GeneratedAt = DateTime.UtcNow
            };

            // Act
            string result = _sut.Generate(context);

            // Assert
            result.Split('-').Should().HaveCount(4); // Category-Brand-ProductId-Suffix only
        }

        [Fact]
        public void Generate_WithCallsVariants_ShouldOrderVariantsAlphabetically()
        {
            // Arrange
            var context = new SkuGenerationContext
            {
                ProductSkuId = 1,
                ProductId = 123,
                CategoryCode = "Electronics",
                BrandCode = "Samsung",
                Variants = new Dictionary<string, string>
                {
                    { "Size", "XL" },
                    { "Color", "Black" },
                    { "Material", "Cotton" }
                },
                GeneratedAt = DateTime.UtcNow
            };

            // Act
            string result = _sut.Generate(context);

            // Assert
            var parts = result.Split('-');
            // Should be ordered alphabetically: Color, Material, Size
            parts.Should().Contain("BLA"); // Black (Color)
            parts.Should().Contain("COT"); // Cotton (Material)
            parts.Should().Contain("XL0"); // XL (Size)
            
            // Verify order
            int blackIndex = Array.IndexOf(parts, "BLA");
            int cottonIndex = Array.IndexOf(parts, "COT");
            int xlIndex = Array.IndexOf(parts, "XL0");
            
            blackIndex.Should().BeLessThan(cottonIndex);
            cottonIndex.Should().BeLessThan(xlIndex);
        }

        [Fact]
        public void Generate_WithDifferentTimestamps_ShouldGenerateDifferentSuffixes()
        {
            // Arrange
            var context1 = new SkuGenerationContext
            {
                ProductSkuId = 1,
                ProductId = 123,
                CategoryCode = "Electronics",
                BrandCode = "Samsung",
                GeneratedAt = new DateTime(2026, 1, 29, 12, 0, 0, DateTimeKind.Utc)
            };

            var context2 = new SkuGenerationContext
            {
                ProductSkuId = 1,
                ProductId = 123,
                CategoryCode = "Electronics",
                BrandCode = "Samsung",
                GeneratedAt = new DateTime(2026, 1, 29, 12, 0, 1, DateTimeKind.Utc) // 1 second later
            };

            // Act
            string result1 = _sut.Generate(context1);
            string result2 = _sut.Generate(context2);

            // Assert
            result1.Should().NotBe(result2);
        }

        #endregion

        #region Generate - Validation Failure Cases

        [Fact]
        public void Generate_WithNullContext_ShouldThrowArgumentNullException()
        {
            // Arrange
            SkuGenerationContext context = null!;

            // Act
            Action act = () => _sut.Generate(context);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("*context*");
        }

        [Fact]
        public void Generate_WithZeroProductId_ShouldThrowArgumentException()
        {
            // Arrange
            var context = new SkuGenerationContext
            {
                ProductSkuId = 1,
                ProductId = 0, // Invalid
                CategoryCode = "Electronics",
                BrandCode = "Samsung",
                GeneratedAt = DateTime.UtcNow
            };

            // Act
            Action act = () => _sut.Generate(context);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*ProductId*");
        }

        [Fact]
        public void Generate_WithNegativeProductId_ShouldThrowArgumentException()
        {
            // Arrange
            var context = new SkuGenerationContext
            {
                ProductSkuId = 1,
                ProductId = -1, // Invalid
                CategoryCode = "Electronics",
                BrandCode = "Samsung",
                GeneratedAt = DateTime.UtcNow
            };

            // Act
            Action act = () => _sut.Generate(context);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*ProductId*");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Generate_WithInvalidCategoryCode_ShouldThrowArgumentException(string? invalidCategoryCode)
        {
            // Arrange
            var context = new SkuGenerationContext
            {
                ProductSkuId = 1,
                ProductId = 123,
                CategoryCode = invalidCategoryCode!,
                BrandCode = "Samsung",
                GeneratedAt = DateTime.UtcNow
            };

            // Act
            Action act = () => _sut.Generate(context);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*CategoryCode*");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Generate_WithInvalidBrandCode_ShouldThrowArgumentException(string? invalidBrandCode)
        {
            // Arrange
            var context = new SkuGenerationContext
            {
                ProductSkuId = 1,
                ProductId = 123,
                CategoryCode = "Electronics",
                BrandCode = invalidBrandCode!,
                GeneratedAt = DateTime.UtcNow
            };

            // Act
            Action act = () => _sut.Generate(context);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*BrandCode*");
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void Generate_WithVeryLargeProductId_ShouldHandleCorrectly()
        {
            // Arrange
            var context = new SkuGenerationContext
            {
                ProductSkuId = 1,
                ProductId = 999999999, // Large number
                CategoryCode = "Electronics",
                BrandCode = "Samsung",
                GeneratedAt = DateTime.UtcNow
            };

            // Act
            string result = _sut.Generate(context);

            // Assert
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().Contain("999999999");
        }

        [Fact]
        public void Generate_WithNumericOnlyCodes_ShouldHandleCorrectly()
        {
            // Arrange
            var context = new SkuGenerationContext
            {
                ProductSkuId = 1,
                ProductId = 123,
                CategoryCode = "123",
                BrandCode = "456",
                GeneratedAt = DateTime.UtcNow
            };

            // Act
            string result = _sut.Generate(context);

            // Assert
            result.Should().StartWith("123-456");
        }

        [Fact]
        public void Generate_WithMixedCaseInput_ShouldConvertToUpperCase()
        {
            // Arrange
            var context = new SkuGenerationContext
            {
                ProductSkuId = 1,
                ProductId = 123,
                CategoryCode = "ElEcTrOnIcS",
                BrandCode = "SaMsUnG",
                Variants = new Dictionary<string, string>
                {
                    { "Color", "BlAcK" }
                },
                GeneratedAt = DateTime.UtcNow
            };

            // Act
            string result = _sut.Generate(context);

            // Assert
            result.Should().MatchRegex(@"^[A-Z0-9-]+$");
            result.Should().Contain("ELE");
            result.Should().Contain("SAMS");
            result.Should().Contain("BLA");
        }

        [Fact]
        public void Generate_WithWhitespaceInCodes_ShouldTrimAndNormalize()
        {
            // Arrange
            var context = new SkuGenerationContext
            {
                ProductSkuId = 1,
                ProductId = 123,
                CategoryCode = "  Electronics  ",
                BrandCode = "  Samsung  ",
                GeneratedAt = DateTime.UtcNow
            };

            // Act
            string result = _sut.Generate(context);

            // Assert
            result.Should().NotContain(" ");
            result.Should().StartWith("ELE-SAMS");
        }

        #endregion

        #region Format Validation

        [Fact]
        public void Generate_ShouldAlwaysProduceUpperCaseAlphanumericWithDashes()
        {
            // Arrange
            var context = new SkuGenerationContext
            {
                ProductSkuId = 1,
                ProductId = 123,
                CategoryCode = "electronics & gadgets",
                BrandCode = "samsung-galaxy",
                Variants = new Dictionary<string, string>
                {
                    { "Color", "midnight-black" }
                },
                GeneratedAt = DateTime.UtcNow
            };

            // Act
            string result = _sut.Generate(context);

            // Assert
            result.Should().MatchRegex(@"^[A-Z0-9]+(-[A-Z0-9]+)*$");
        }

        [Fact]
        public void Generate_ShouldNotStartOrEndWithDash()
        {
            // Arrange
            var context = new SkuGenerationContext
            {
                ProductSkuId = 1,
                ProductId = 123,
                CategoryCode = "Electronics",
                BrandCode = "Samsung",
                GeneratedAt = DateTime.UtcNow
            };

            // Act
            string result = _sut.Generate(context);

            // Assert
            result.Should().NotStartWith("-");
            result.Should().NotEndWith("-");
        }

        [Fact]
        public void Generate_ShouldNotContainConsecutiveDashes()
        {
            // Arrange
            var context = new SkuGenerationContext
            {
                ProductSkuId = 1,
                ProductId = 123,
                CategoryCode = "Electronics",
                BrandCode = "Samsung",
                Variants = new Dictionary<string, string>
                {
                    { "Color", "Black" }
                },
                GeneratedAt = DateTime.UtcNow
            };

            // Act
            string result = _sut.Generate(context);

            // Assert
            result.Should().NotContain("--");
        }

        #endregion
    }
}
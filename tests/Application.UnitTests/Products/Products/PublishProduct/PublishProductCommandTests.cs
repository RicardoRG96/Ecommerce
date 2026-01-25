using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.Products.Common.Services;
using Application.Products.Products.PublishProduct;
using Domain.Entities.Products;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Products.Products.PublishProduct
{
    public class PublishProductCommandTests
    {
        private const long ProductId = 1;

        private static readonly PublishProductCommand _command = new(ProductId);

        private readonly PublishProductCommandHandler _handler;
        private readonly IProductRepository _productRepositoryMock;
        private readonly IPublishProductValidator _validatorMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public PublishProductCommandTests()
        {
            _productRepositoryMock = Substitute.For<IProductRepository>();
            _validatorMock = Substitute.For<IPublishProductValidator>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _handler = new(
                _productRepositoryMock,
                _validatorMock,
                _unitOfWorkMock);
        }

        #region Helper Methods

        private Product CreateValidProduct()
        {
            return new Product
            {
                Id = ProductId,
                Name = "iPhone 15 Pro",
                IsActive = true,
                IsPublished = false,
                IsFeatured = true,
                IsDigital = false,
                BrandId = 1,
                CategoryId = 2,
                ProductTaxCategoryId = 3
            };
        }

        private void SetupSuccessfulScenario(Product product)
        {
            _productRepositoryMock
                .GetByIdAsync(ProductId, Arg.Any<CancellationToken>())
                .Returns(product);

            _validatorMock
                .ValidateCanBePublishedAsync(ProductId, Arg.Any<CancellationToken>())
                .Returns(Result.Success());
        }

        private void SetupProductNotFound()
        {
            _productRepositoryMock
                .GetByIdAsync(ProductId, Arg.Any<CancellationToken>())
                .Returns((Product?)null);
        }

        private void SetupValidationFailure(Product product, Error error)
        {
            _productRepositoryMock
                .GetByIdAsync(ProductId, Arg.Any<CancellationToken>())
                .Returns(product);

            _validatorMock
                .ValidateCanBePublishedAsync(ProductId, Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        #endregion

        #region Success Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenProductIsValidForPublishing()
        {
            // Arrange
            Product product = CreateValidProduct();
            SetupSuccessfulScenario(product);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_SetIsPublishedToTrue_WhenProductIsValid()
        {
            // Arrange
            Product product = CreateValidProduct();
            product.IsPublished = false;
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            product.IsPublished.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_CallPublishMethod_WhenProductIsValid()
        {
            // Arrange
            Product product = CreateValidProduct();
            SetupSuccessfulScenario(product);

            bool publishCalled = false;
            product.IsPublished = false;

            // Act
            await _handler.Handle(_command, default);
            publishCalled = product.IsPublished;

            // Assert
            publishCalled.Should().BeTrue();
        }

        #endregion

        #region Product Not Found Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenProductDoesNotExist()
        {
            // Arrange
            SetupProductNotFound();

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductErrors.NotFound(ProductId));
        }

        [Fact]
        public async Task Handle_Should_NotCallValidateCanBePublishedAsync_WhenProductDoesNotExist()
        {
            // Arrange
            SetupProductNotFound();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .DidNotReceive()
                .ValidateCanBePublishedAsync(
                    Arg.Any<long>(),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenProductDoesNotExist()
        {
            // Arrange
            SetupProductNotFound();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<Product>());
        }

        [Fact]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenProductDoesNotExist()
        {
            // Arrange
            SetupProductNotFound();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region Validation Failure Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenProductIsNotActive()
        {
            // Arrange
            Product product = CreateValidProduct();
            SetupValidationFailure(product, ProductErrors.ProductNotActive);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductErrors.ProductNotActive);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenBrandIsNotActive()
        {
            // Arrange
            Product product = CreateValidProduct();
            SetupValidationFailure(product, ProductErrors.BrandNotActive);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductErrors.BrandNotActive);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenCategoryIsNotActive()
        {
            // Arrange
            Product product = CreateValidProduct();
            SetupValidationFailure(product, ProductErrors.CategoryNotActive);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductErrors.CategoryNotActive);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenProductTaxCategoryIsNotActive()
        {
            // Arrange
            Product product = CreateValidProduct();
            SetupValidationFailure(product, ProductErrors.ProductTaxCategoryNotActive);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductErrors.ProductTaxCategoryNotActive);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenProductHasNoSkus()
        {
            // Arrange
            Product product = CreateValidProduct();
            SetupValidationFailure(product, ProductErrors.ProductMustHaveAtLeastOneSku);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductErrors.ProductMustHaveAtLeastOneSku);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenProductSkuIsNotActive()
        {
            // Arrange
            Product product = CreateValidProduct();
            SetupValidationFailure(product, ProductErrors.ProductSkuNotActive);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductErrors.ProductSkuNotActive);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenProductSkuHasInvalidPrice()
        {
            // Arrange
            Product product = CreateValidProduct();
            SetupValidationFailure(product, ProductErrors.ProductSkuWithoutValidPrice);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductErrors.ProductSkuWithoutValidPrice);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenProductHasNoImages()
        {
            // Arrange
            Product product = CreateValidProduct();
            SetupValidationFailure(product, ProductErrors.ProductMustHaveAtLeastOneImage);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductErrors.ProductMustHaveAtLeastOneImage);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenProductHasNoPrimaryImage()
        {
            // Arrange
            Product product = CreateValidProduct();
            SetupValidationFailure(product, ProductErrors.ProductWithoutPrimaryImage);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductErrors.ProductWithoutPrimaryImage);
        }

        [Theory]
        [InlineData(nameof(ProductErrors.ProductNotActive))]
        [InlineData(nameof(ProductErrors.BrandNotActive))]
        [InlineData(nameof(ProductErrors.CategoryNotActive))]
        [InlineData(nameof(ProductErrors.ProductTaxCategoryNotActive))]
        [InlineData(nameof(ProductErrors.ProductMustHaveAtLeastOneSku))]
        [InlineData(nameof(ProductErrors.ProductSkuNotActive))]
        [InlineData(nameof(ProductErrors.ProductSkuWithoutValidPrice))]
        [InlineData(nameof(ProductErrors.ProductMustHaveAtLeastOneImage))]
        [InlineData(nameof(ProductErrors.ProductWithoutPrimaryImage))]
        public async Task Handle_Should_NotPublishProduct_WhenValidationFails(string errorPropertyName)
        {
            // Arrange
            Product product = CreateValidProduct();
            var error = GetErrorByPropertyName(errorPropertyName);
            SetupValidationFailure(product, error);

            bool originalPublishedState = product.IsPublished;

            // Act
            await _handler.Handle(_command, default);

            // Assert
            product.IsPublished.Should().Be(originalPublishedState);
        }

        [Theory]
        [InlineData(nameof(ProductErrors.ProductNotActive))]
        [InlineData(nameof(ProductErrors.BrandNotActive))]
        [InlineData(nameof(ProductErrors.CategoryNotActive))]
        [InlineData(nameof(ProductErrors.ProductTaxCategoryNotActive))]
        [InlineData(nameof(ProductErrors.ProductMustHaveAtLeastOneSku))]
        [InlineData(nameof(ProductErrors.ProductSkuNotActive))]
        [InlineData(nameof(ProductErrors.ProductSkuWithoutValidPrice))]
        [InlineData(nameof(ProductErrors.ProductMustHaveAtLeastOneImage))]
        [InlineData(nameof(ProductErrors.ProductWithoutPrimaryImage))]
        public async Task Handle_Should_NotCallUpdate_WhenValidationFails(string errorPropertyName)
        {
            // Arrange
            Product product = CreateValidProduct();
            var error = GetErrorByPropertyName(errorPropertyName);
            SetupValidationFailure(product, error);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<Product>());
        }

        [Theory]
        [InlineData(nameof(ProductErrors.ProductNotActive))]
        [InlineData(nameof(ProductErrors.BrandNotActive))]
        [InlineData(nameof(ProductErrors.CategoryNotActive))]
        [InlineData(nameof(ProductErrors.ProductTaxCategoryNotActive))]
        [InlineData(nameof(ProductErrors.ProductMustHaveAtLeastOneSku))]
        [InlineData(nameof(ProductErrors.ProductSkuNotActive))]
        [InlineData(nameof(ProductErrors.ProductSkuWithoutValidPrice))]
        [InlineData(nameof(ProductErrors.ProductMustHaveAtLeastOneImage))]
        [InlineData(nameof(ProductErrors.ProductWithoutPrimaryImage))]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenValidationFails(string errorPropertyName)
        {
            // Arrange
            Product product = CreateValidProduct();
            var error = GetErrorByPropertyName(errorPropertyName);
            SetupValidationFailure(product, error);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region Validation Call Verification

        [Fact]
        public async Task Handle_Should_CallValidateCanBePublishedAsync_WithCorrectProductId()
        {
            // Arrange
            Product product = CreateValidProduct();
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateCanBePublishedAsync(
                    ProductId,
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallValidateCanBePublishedAsync_AfterGettingProduct()
        {
            // Arrange
            Product product = CreateValidProduct();
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(async () =>
            {
                await _productRepositoryMock.GetByIdAsync(ProductId, Arg.Any<CancellationToken>());
                await _validatorMock.ValidateCanBePublishedAsync(ProductId, Arg.Any<CancellationToken>());
            });
        }

        #endregion

        #region Repository and UnitOfWork Interactions

        [Fact]
        public async Task Handle_Should_CallUpdate_ExactlyOnce_WhenProductIsValid()
        {
            // Arrange
            Product product = CreateValidProduct();
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productRepositoryMock
                .Received(1)
                .Update(product);
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_ExactlyOnce_WhenProductIsValid()
        {
            // Arrange
            Product product = CreateValidProduct();
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_AfterUpdate_WhenProductIsValid()
        {
            // Arrange
            Product product = CreateValidProduct();
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(() =>
            {
                _productRepositoryMock.Update(product);
                _unitOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>());
            });
        }

        [Fact]
        public async Task Handle_Should_CallUpdate_WithSameProductInstance()
        {
            // Arrange
            Product product = CreateValidProduct();
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productRepositoryMock
                .Received(1)
                .Update(Arg.Is<Product>(p => p == product && p.IsPublished == true));
        }

        #endregion

        #region Property Preservation Tests

        [Fact]
        public async Task Handle_Should_PreserveProductId_WhenPublishing()
        {
            // Arrange
            Product product = CreateValidProduct();
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            product.Id.Should().Be(ProductId);
        }

        [Fact]
        public async Task Handle_Should_PreserveIsActiveProperty_WhenPublishing()
        {
            // Arrange
            Product product = CreateValidProduct();
            product.IsActive = true;
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            product.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_PreserveIsFeaturedProperty_WhenPublishing()
        {
            // Arrange
            Product product = CreateValidProduct();
            product.IsFeatured = true;
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            product.IsFeatured.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_PreserveOtherProperties_WhenPublishing()
        {
            // Arrange
            Product product = CreateValidProduct();
            string originalName = product.Name;
            long originalBrandId = product.BrandId;
            long originalCategoryId = product.CategoryId;
            
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            product.Name.Should().Be(originalName);
            product.BrandId.Should().Be(originalBrandId);
            product.CategoryId.Should().Be(originalCategoryId);
        }

        #endregion

        #region Edge Case Scenarios

        [Fact]
        public async Task Handle_Should_AllowPublishing_WhenAlreadyPublished()
        {
            // Arrange
            Product product = CreateValidProduct();
            product.IsPublished = true;
            SetupSuccessfulScenario(product);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            product.IsPublished.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_PublishInactiveProduct_IfValidatorAllows()
        {
            // Arrange
            Product product = CreateValidProduct();
            product.IsActive = false;
            product.IsPublished = false;

            // Setup validator to return success despite inactive product
            // (This tests that handler relies on validator)
            _productRepositoryMock
                .GetByIdAsync(ProductId, Arg.Any<CancellationToken>())
                .Returns(product);

            _validatorMock
                .ValidateCanBePublishedAsync(ProductId, Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            product.IsPublished.Should().BeTrue();
        }

        #endregion

        #region Helper Methods for Tests

        private static Error GetErrorByPropertyName(string propertyName) => propertyName switch
        {
            nameof(ProductErrors.ProductNotActive) => ProductErrors.ProductNotActive,
            nameof(ProductErrors.BrandNotActive) => ProductErrors.BrandNotActive,
            nameof(ProductErrors.CategoryNotActive) => ProductErrors.CategoryNotActive,
            nameof(ProductErrors.ProductTaxCategoryNotActive) => ProductErrors.ProductTaxCategoryNotActive,
            nameof(ProductErrors.ProductMustHaveAtLeastOneSku) => ProductErrors.ProductMustHaveAtLeastOneSku,
            nameof(ProductErrors.ProductSkuNotActive) => ProductErrors.ProductSkuNotActive,
            nameof(ProductErrors.ProductSkuWithoutValidPrice) => ProductErrors.ProductSkuWithoutValidPrice,
            nameof(ProductErrors.ProductMustHaveAtLeastOneImage) => ProductErrors.ProductMustHaveAtLeastOneImage,
            nameof(ProductErrors.ProductWithoutPrimaryImage) => ProductErrors.ProductWithoutPrimaryImage,
            _ => throw new ArgumentException($"Unknown error property: {propertyName}")
        };

        #endregion
    }
}

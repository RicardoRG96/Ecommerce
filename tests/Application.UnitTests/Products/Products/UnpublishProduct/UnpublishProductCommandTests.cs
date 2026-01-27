using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.Products.UnpublishProduct;
using Domain.Entities.Products;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Products.Products.UnpublishProduct
{
    public class UnpublishProductCommandTests
    {
        private const long ProductId = 1;

        private static readonly UnpublishProductCommand _command = new(ProductId);

        private readonly UnpublishProductCommandHandler _handler;
        private readonly IProductRepository _productRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public UnpublishProductCommandTests()
        {
            _productRepositoryMock = Substitute.For<IProductRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _handler = new(
                _productRepositoryMock,
                _unitOfWorkMock);
        }

        #region Helper Methods

        private Product CreatePublishedProduct()
        {
            return new Product
            {
                Id = ProductId,
                Name = "iPhone 15 Pro",
                IsActive = true,
                IsPublished = true,
                IsFeatured = true,
                IsDigital = false,
                BrandId = 1,
                CategoryId = 2,
                ProductTaxCategoryId = 3
            };
        }

        private Product CreateUnpublishedProduct()
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
        }

        private void SetupProductNotFound()
        {
            _productRepositoryMock
                .GetByIdAsync(ProductId, Arg.Any<CancellationToken>())
                .Returns((Product?)null);
        }

        #endregion

        #region Success Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenProductIsPublished()
        {
            // Arrange
            Product product = CreatePublishedProduct();
            SetupSuccessfulScenario(product);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_SetIsPublishedToFalse_WhenProductIsPublished()
        {
            // Arrange
            Product product = CreatePublishedProduct();
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            product.IsPublished.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_CallUnpublishMethod_WhenProductIsPublished()
        {
            // Arrange
            Product product = CreatePublishedProduct();
            SetupSuccessfulScenario(product);

            bool unpublishCalled = false;
            product.IsPublished = true;

            // Act
            await _handler.Handle(_command, default);
            unpublishCalled = !product.IsPublished;

            // Assert
            unpublishCalled.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenProductIsAlreadyUnpublished()
        {
            // Arrange
            Product product = CreateUnpublishedProduct();
            SetupSuccessfulScenario(product);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_NotChangeIsPublished_WhenProductIsAlreadyUnpublished()
        {
            // Arrange
            Product product = CreateUnpublishedProduct();
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            product.IsPublished.Should().BeFalse();
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

        #region Already Unpublished Scenarios

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenProductIsAlreadyUnpublished()
        {
            // Arrange
            Product product = CreateUnpublishedProduct();
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<Product>());
        }

        [Fact]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenProductIsAlreadyUnpublished()
        {
            // Arrange
            Product product = CreateUnpublishedProduct();
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_BeIdempotent_WhenCalledMultipleTimes()
        {
            // Arrange
            Product product = CreateUnpublishedProduct();
            SetupSuccessfulScenario(product);

            // Act
            Result firstResult = await _handler.Handle(_command, default);
            Result secondResult = await _handler.Handle(_command, default);

            // Assert
            firstResult.IsSuccess.Should().BeTrue();
            secondResult.IsSuccess.Should().BeTrue();
            product.IsPublished.Should().BeFalse();
        }

        #endregion

        #region Repository and UnitOfWork Interactions

        [Fact]
        public async Task Handle_Should_CallUpdate_ExactlyOnce_WhenProductIsPublished()
        {
            // Arrange
            Product product = CreatePublishedProduct();
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productRepositoryMock
                .Received(1)
                .Update(product);
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_ExactlyOnce_WhenProductIsPublished()
        {
            // Arrange
            Product product = CreatePublishedProduct();
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_AfterUpdate_WhenProductIsPublished()
        {
            // Arrange
            Product product = CreatePublishedProduct();
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
            Product product = CreatePublishedProduct();
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productRepositoryMock
                .Received(1)
                .Update(Arg.Is<Product>(p => p == product && p.IsPublished == false));
        }

        [Fact]
        public async Task Handle_Should_CallGetByIdAsync_ExactlyOnce()
        {
            // Arrange
            Product product = CreatePublishedProduct();
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productRepositoryMock
                .Received(1)
                .GetByIdAsync(ProductId, Arg.Any<CancellationToken>());
        }

        #endregion

        #region Property Preservation Tests

        [Fact]
        public async Task Handle_Should_PreserveProductId_WhenUnpublishing()
        {
            // Arrange
            Product product = CreatePublishedProduct();
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            product.Id.Should().Be(ProductId);
        }

        [Fact]
        public async Task Handle_Should_PreserveIsActiveProperty_WhenUnpublishing()
        {
            // Arrange
            Product product = CreatePublishedProduct();
            product.IsActive = true;
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            product.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_PreserveIsFeaturedProperty_WhenUnpublishing()
        {
            // Arrange
            Product product = CreatePublishedProduct();
            product.IsFeatured = true;
            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            product.IsFeatured.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_PreserveOtherProperties_WhenUnpublishing()
        {
            // Arrange
            Product product = CreatePublishedProduct();
            string originalName = product.Name;
            long originalBrandId = product.BrandId;
            long originalCategoryId = product.CategoryId;
            bool originalIsActive = product.IsActive;
            bool originalIsFeatured = product.IsFeatured;
            bool originalIsDigital = product.IsDigital;

            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            product.Name.Should().Be(originalName);
            product.BrandId.Should().Be(originalBrandId);
            product.CategoryId.Should().Be(originalCategoryId);
            product.IsActive.Should().Be(originalIsActive);
            product.IsFeatured.Should().Be(originalIsFeatured);
            product.IsDigital.Should().Be(originalIsDigital);
        }

        [Fact]
        public async Task Handle_Should_OnlyChangeIsPublishedProperty_WhenUnpublishing()
        {
            // Arrange
            Product product = CreatePublishedProduct();

            var originalProduct = new
            {
                product.Id,
                product.Name,
                product.BrandId,
                product.CategoryId,
                product.ProductTaxCategoryId,
                product.IsActive,
                product.IsFeatured,
                product.IsDigital
            };

            SetupSuccessfulScenario(product);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            product.Id.Should().Be(originalProduct.Id);
            product.Name.Should().Be(originalProduct.Name);
            product.BrandId.Should().Be(originalProduct.BrandId);
            product.CategoryId.Should().Be(originalProduct.CategoryId);
            product.ProductTaxCategoryId.Should().Be(originalProduct.ProductTaxCategoryId);
            product.IsActive.Should().Be(originalProduct.IsActive);
            product.IsFeatured.Should().Be(originalProduct.IsFeatured);
            product.IsDigital.Should().Be(originalProduct.IsDigital);
            product.IsPublished.Should().BeFalse();
        }

        #endregion

        #region Edge Case Scenarios

        [Fact]
        public async Task Handle_Should_UnpublishActiveProduct()
        {
            // Arrange
            Product product = CreatePublishedProduct();
            product.IsActive = true;
            product.IsPublished = true;
            SetupSuccessfulScenario(product);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            product.IsPublished.Should().BeFalse();
            product.IsActive.Should().BeTrue(); // Should remain active
        }

        [Fact]
        public async Task Handle_Should_UnpublishInactiveProduct()
        {
            // Arrange
            Product product = CreatePublishedProduct();
            product.IsActive = false;
            product.IsPublished = true;
            SetupSuccessfulScenario(product);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            product.IsPublished.Should().BeFalse();
            product.IsActive.Should().BeFalse(); // Should remain inactive
        }

        [Fact]
        public async Task Handle_Should_HandleMultipleUnpublishAttempts_Gracefully()
        {
            // Arrange
            Product product = CreatePublishedProduct();
            SetupSuccessfulScenario(product);

            // Act
            Result firstResult = await _handler.Handle(_command, default);
            Result secondResult = await _handler.Handle(_command, default);
            Result thirdResult = await _handler.Handle(_command, default);

            // Assert
            firstResult.IsSuccess.Should().BeTrue();
            secondResult.IsSuccess.Should().BeTrue();
            thirdResult.IsSuccess.Should().BeTrue();
            product.IsPublished.Should().BeFalse();

            // Should only update once (first time)
            _productRepositoryMock
                .Received(1)
                .Update(Arg.Any<Product>());
        }

        #endregion

        #region State Transition Tests

        [Fact]
        public async Task Handle_Should_TransitionFromPublishedToUnpublished()
        {
            // Arrange
            Product product = CreatePublishedProduct();
            product.IsPublished = true;
            SetupSuccessfulScenario(product);

            bool wasPublished = product.IsPublished;

            // Act
            await _handler.Handle(_command, default);

            bool isNowUnpublished = !product.IsPublished;

            // Assert
            wasPublished.Should().BeTrue();
            isNowUnpublished.Should().BeTrue();
            product.IsPublished.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_MaintainUnpublishedState()
        {
            // Arrange
            Product product = CreateUnpublishedProduct();
            product.IsPublished = false;
            SetupSuccessfulScenario(product);

            bool wasUnpublished = !product.IsPublished;

            // Act
            await _handler.Handle(_command, default);

            bool isStillUnpublished = !product.IsPublished;

            // Assert
            wasUnpublished.Should().BeTrue();
            isStillUnpublished.Should().BeTrue();
            product.IsPublished.Should().BeFalse();
        }

        #endregion

        #region Cancellation Token Tests

        [Fact]
        public async Task Handle_Should_PassCancellationToken_ToGetByIdAsync()
        {
            // Arrange
            Product product = CreatePublishedProduct();
            SetupSuccessfulScenario(product);

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            // Act
            await _handler.Handle(_command, cancellationToken);

            // Assert
            await _productRepositoryMock
                .Received(1)
                .GetByIdAsync(ProductId, cancellationToken);
        }

        [Fact]
        public async Task Handle_Should_PassCancellationToken_ToSaveChangesAsync()
        {
            // Arrange
            Product product = CreatePublishedProduct();
            SetupSuccessfulScenario(product);

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            // Act
            await _handler.Handle(_command, cancellationToken);

            // Assert
            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(cancellationToken);
        }

        #endregion
    }
}

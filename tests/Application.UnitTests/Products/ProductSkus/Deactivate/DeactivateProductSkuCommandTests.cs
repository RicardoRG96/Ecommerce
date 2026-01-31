using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.ProductSkus.Common.Services;
using Application.Products.ProductSkus.Deactivate;
using Domain.Entities.Products;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Products.ProductSkus.Deactivate
{
    public class DeactivateProductSkuCommandTests
    {
        private const long ProductSkuId = 1;
        private const long ProductId = 10;

        private static readonly DeactivateProductSkuCommand _command = new(ProductSkuId);

        private readonly DeactivateProductSkuCommandHandler _handler;
        private readonly IProductSkuRepository _productSkuRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public DeactivateProductSkuCommandTests()
        {
            _productSkuRepositoryMock = Substitute.For<IProductSkuRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _handler = new(
                _productSkuRepositoryMock,
                Substitute.For<IProductSkuValidator>(), // Aún necesario en el constructor
                _unitOfWorkMock);
        }

        #region Helper Methods

        private ProductSku CreateActiveProductSku()
        {
            return new ProductSku
            {
                Id = ProductSkuId,
                ProductId = ProductId,
                SkuCode = "PROD-001-SKU-001",
                BarCode = "1234567890123",
                Price = 999.99m,
                IsActive = true
            };
        }

        private ProductSku CreateInactiveProductSku()
        {
            return new ProductSku
            {
                Id = ProductSkuId,
                ProductId = ProductId,
                SkuCode = "PROD-001-SKU-002",
                BarCode = "9876543210123",
                Price = 1299.99m,
                IsActive = false
            };
        }

        private void SetupProductSkuFound(ProductSku productSku)
        {
            _productSkuRepositoryMock
                .GetByIdAsync(ProductSkuId, Arg.Any<CancellationToken>())
                .Returns(productSku);
        }

        private void SetupProductSkuNotFound()
        {
            _productSkuRepositoryMock
                .GetByIdAsync(ProductSkuId, Arg.Any<CancellationToken>())
                .ReturnsNull();
        }

        #endregion

        #region ProductSku Not Found Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenProductSkuDoesNotExist()
        {
            // Arrange
            SetupProductSkuNotFound();

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductSkuErrors.NotFound(ProductSkuId));
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenProductSkuDoesNotExist()
        {
            // Arrange
            SetupProductSkuNotFound();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productSkuRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<ProductSku>());
        }

        [Fact]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenProductSkuDoesNotExist()
        {
            // Arrange
            SetupProductSkuNotFound();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region Active ProductSku Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenProductSkuIsActive()
        {
            // Arrange
            ProductSku productSku = CreateActiveProductSku();
            SetupProductSkuFound(productSku);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_DeactivateProductSku_WhenProductSkuIsActive()
        {
            // Arrange
            ProductSku productSku = CreateActiveProductSku();
            SetupProductSkuFound(productSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            productSku.IsActive.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_CallUpdate_WhenProductSkuIsActive()
        {
            // Arrange
            ProductSku productSku = CreateActiveProductSku();
            SetupProductSkuFound(productSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productSkuRepositoryMock
                .Received(1)
                .Update(Arg.Any<ProductSku>());
        }

        [Fact]
        public async Task Handle_Should_CallUpdateWithCorrectProductSku_WhenProductSkuIsActive()
        {
            // Arrange
            ProductSku productSku = CreateActiveProductSku();
            SetupProductSkuFound(productSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productSkuRepositoryMock
                .Received(1)
                .Update(productSku);
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_WhenProductSkuIsActive()
        {
            // Arrange
            ProductSku productSku = CreateActiveProductSku();
            SetupProductSkuFound(productSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_AfterUpdate_WhenProductSkuIsActive()
        {
            // Arrange
            ProductSku productSku = CreateActiveProductSku();
            SetupProductSkuFound(productSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(() =>
            {
                _productSkuRepositoryMock.Update(Arg.Any<ProductSku>());
                _unitOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>());
            });
        }

        #endregion

        #region Already Inactive ProductSku Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenProductSkuIsAlreadyInactive()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_NotChangeIsActive_WhenProductSkuIsAlreadyInactive()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            productSku.IsActive.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenProductSkuIsAlreadyInactive()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productSkuRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<ProductSku>());
        }

        [Fact]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenProductSkuIsAlreadyInactive()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region Repository Interaction Scenarios

        [Fact]
        public async Task Handle_Should_CallGetByIdAsync_Always()
        {
            // Arrange
            ProductSku productSku = CreateActiveProductSku();
            SetupProductSkuFound(productSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productSkuRepositoryMock
                .Received(1)
                .GetByIdAsync(ProductSkuId, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallGetByIdAsync_WithCorrectProductSkuId()
        {
            // Arrange
            ProductSku productSku = CreateActiveProductSku();
            SetupProductSkuFound(productSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productSkuRepositoryMock
                .Received(1)
                .GetByIdAsync(
                    Arg.Is<long>(id => id == ProductSkuId),
                    Arg.Any<CancellationToken>());
        }

        #endregion

        #region Edge Cases

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenDifferentProductSkuIdDoesNotExist()
        {
            // Arrange
            long nonExistentProductSkuId = 9999;
            var command = new DeactivateProductSkuCommand(nonExistentProductSkuId);

            _productSkuRepositoryMock
                .GetByIdAsync(nonExistentProductSkuId, Arg.Any<CancellationToken>())
                .ReturnsNull();

            // Act
            Result result = await _handler.Handle(command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductSkuErrors.NotFound(nonExistentProductSkuId));
        }

        [Fact]
        public async Task Handle_Should_PreserveProductSkuId_AfterDeactivation()
        {
            // Arrange
            ProductSku productSku = CreateActiveProductSku();
            SetupProductSkuFound(productSku);
            long originalId = productSku.Id;

            // Act
            await _handler.Handle(_command, default);

            // Assert
            productSku.Id.Should().Be(originalId);
        }

        [Fact]
        public async Task Handle_Should_PreserveProductId_AfterDeactivation()
        {
            // Arrange
            ProductSku productSku = CreateActiveProductSku();
            SetupProductSkuFound(productSku);
            long originalProductId = productSku.ProductId;

            // Act
            await _handler.Handle(_command, default);

            // Assert
            productSku.ProductId.Should().Be(originalProductId);
        }

        [Fact]
        public async Task Handle_Should_PreserveSkuCode_AfterDeactivation()
        {
            // Arrange
            ProductSku productSku = CreateActiveProductSku();
            SetupProductSkuFound(productSku);
            string originalSkuCode = productSku.SkuCode!;

            // Act
            await _handler.Handle(_command, default);

            // Assert
            productSku.SkuCode.Should().Be(originalSkuCode);
        }

        [Fact]
        public async Task Handle_Should_OnlyModifyIsActiveProperty_WhenDeactivatingProductSku()
        {
            // Arrange
            ProductSku productSku = CreateActiveProductSku();
            string originalSkuCode = productSku.SkuCode!;
            long originalProductId = productSku.ProductId;
            decimal originalPrice = productSku.Price;

            SetupProductSkuFound(productSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            productSku.IsActive.Should().BeFalse();
            productSku.SkuCode.Should().Be(originalSkuCode);
            productSku.ProductId.Should().Be(originalProductId);
            productSku.Price.Should().Be(originalPrice);
        }

        [Fact]
        public async Task Handle_Should_CallDeactivate_OnProductSkuEntity()
        {
            // Arrange
            ProductSku productSku = CreateActiveProductSku();
            SetupProductSkuFound(productSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            productSku.IsActive.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_ExecuteInCorrectOrder_WhenDeactivating()
        {
            // Arrange
            ProductSku productSku = CreateActiveProductSku();
            SetupProductSkuFound(productSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(async () =>
            {
                await _productSkuRepositoryMock.GetByIdAsync(Arg.Any<long>(), Arg.Any<CancellationToken>());
                _productSkuRepositoryMock.Update(Arg.Any<ProductSku>());
                await _unitOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>());
            });
        }

        #endregion
    }
}

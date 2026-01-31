using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.ProductSkus.Activate;
using Application.Products.ProductSkus.Common.Services;
using Domain.Entities.Products;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Products.ProductSkus.Activate
{
    public class ActivateProductSkuCommandTests
    {
        private const long ProductSkuId = 1;
        private const long ProductId = 10;

        private static readonly ActivateProductSkuCommand _command = new(ProductSkuId);

        private readonly ActivateProductSkuCommandHandler _handler;
        private readonly IProductSkuRepository _productSkuRepositoryMock;
        private readonly IProductSkuValidator _validatorMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public ActivateProductSkuCommandTests()
        {
            _productSkuRepositoryMock = Substitute.For<IProductSkuRepository>();
            _validatorMock = Substitute.For<IProductSkuValidator>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _handler = new(
                _productSkuRepositoryMock,
                _validatorMock,
                _unitOfWorkMock);
        }

        #region Helper Methods

        private ProductSku CreateInactiveProductSku()
        {
            return new ProductSku
            {
                Id = ProductSkuId,
                ProductId = ProductId,
                SkuCode = "PROD-001-SKU-001",
                BarCode = "1234567890123",
                Price = 999.99m,
                IsActive = false
            };
        }

        private ProductSku CreateActiveProductSku()
        {
            return new ProductSku
            {
                Id = ProductSkuId,
                ProductId = ProductId,
                SkuCode = "PROD-001-SKU-002",
                BarCode = "9876543210123",
                Price = 1299.99m,
                IsActive = true
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

        private void SetupSuccessfulValidations()
        {
            _validatorMock
                .ValidateProductIsPublishedAsync(
                    ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateAttributeValuesAreActive(
                    ProductSkuId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());
        }

        private void SetupProductNotPublishedValidationFailure(Error error)
        {
            _validatorMock
                .ValidateProductIsPublishedAsync(
                    ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        private void SetupAttributeValuesNotActiveValidationFailure(Error error)
        {
            _validatorMock
                .ValidateProductIsPublishedAsync(
                    ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateAttributeValuesAreActive(
                    ProductSkuId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
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
        public async Task Handle_Should_NotCallValidateProductIsPublishedAsync_WhenProductSkuDoesNotExist()
        {
            // Arrange
            SetupProductSkuNotFound();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .DidNotReceive()
                .ValidateProductIsPublishedAsync(
                    Arg.Any<long>(),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallValidateAttributeValuesAreActive_WhenProductSkuDoesNotExist()
        {
            // Arrange
            SetupProductSkuNotFound();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .DidNotReceive()
                .ValidateAttributeValuesAreActive(
                    Arg.Any<long>(),
                    Arg.Any<CancellationToken>());
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

        #region Already Active ProductSku Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenProductSkuIsAlreadyActive()
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
        public async Task Handle_Should_NotChangeIsActive_WhenProductSkuIsAlreadyActive()
        {
            // Arrange
            ProductSku productSku = CreateActiveProductSku();
            SetupProductSkuFound(productSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            productSku.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_NotCallValidations_WhenProductSkuIsAlreadyActive()
        {
            // Arrange
            ProductSku productSku = CreateActiveProductSku();
            SetupProductSkuFound(productSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .DidNotReceive()
                .ValidateProductIsPublishedAsync(
                    Arg.Any<long>(),
                    Arg.Any<CancellationToken>());

            await _validatorMock
                .DidNotReceive()
                .ValidateAttributeValuesAreActive(
                    Arg.Any<long>(),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenProductSkuIsAlreadyActive()
        {
            // Arrange
            ProductSku productSku = CreateActiveProductSku();
            SetupProductSkuFound(productSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productSkuRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<ProductSku>());
        }

        [Fact]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenProductSkuIsAlreadyActive()
        {
            // Arrange
            ProductSku productSku = CreateActiveProductSku();
            SetupProductSkuFound(productSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region Product Not Published Failure Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenProductNotFound()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupProductNotPublishedValidationFailure(ProductErrors.NotFound(ProductId));

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductErrors.NotFound(ProductId));
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenProductNotPublished()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupProductNotPublishedValidationFailure(ProductSkuErrors.ProductNotPublished);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductSkuErrors.ProductNotPublished);
        }

        [Fact]
        public async Task Handle_Should_NotActivateProductSku_WhenProductNotPublished()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupProductNotPublishedValidationFailure(ProductSkuErrors.ProductNotPublished);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            productSku.IsActive.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_NotCallValidateAttributeValuesAreActive_WhenProductNotPublished()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupProductNotPublishedValidationFailure(ProductSkuErrors.ProductNotPublished);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .DidNotReceive()
                .ValidateAttributeValuesAreActive(
                    Arg.Any<long>(),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenProductNotPublished()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupProductNotPublishedValidationFailure(ProductSkuErrors.ProductNotPublished);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productSkuRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<ProductSku>());
        }

        [Fact]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenProductNotPublished()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupProductNotPublishedValidationFailure(ProductSkuErrors.ProductNotPublished);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region Attribute Values Not Active Failure Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenAttributeValuesAreNotActive()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupAttributeValuesNotActiveValidationFailure(ProductSkuErrors.AttributeValuesNotActive);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductSkuErrors.AttributeValuesNotActive);
        }

        [Fact]
        public async Task Handle_Should_NotActivateProductSku_WhenAttributeValuesAreNotActive()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupAttributeValuesNotActiveValidationFailure(ProductSkuErrors.AttributeValuesNotActive);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            productSku.IsActive.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenAttributeValuesAreNotActive()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupAttributeValuesNotActiveValidationFailure(ProductSkuErrors.AttributeValuesNotActive);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productSkuRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<ProductSku>());
        }

        [Fact]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenAttributeValuesAreNotActive()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupAttributeValuesNotActiveValidationFailure(ProductSkuErrors.AttributeValuesNotActive);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region Inactive ProductSku Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenProductSkuIsInactive()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupSuccessfulValidations();

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_ActivateProductSku_WhenProductSkuIsInactive()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupSuccessfulValidations();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            productSku.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_CallUpdate_WhenProductSkuIsInactive()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupSuccessfulValidations();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productSkuRepositoryMock
                .Received(1)
                .Update(Arg.Any<ProductSku>());
        }

        [Fact]
        public async Task Handle_Should_CallUpdateWithCorrectProductSku_WhenProductSkuIsInactive()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupSuccessfulValidations();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productSkuRepositoryMock
                .Received(1)
                .Update(productSku);
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_WhenProductSkuIsInactive()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupSuccessfulValidations();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_AfterUpdate_WhenProductSkuIsInactive()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupSuccessfulValidations();

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

        #region Validation Scenarios

        [Fact]
        public async Task Handle_Should_CallValidateProductIsPublishedAsync_WhenProductSkuIsInactive()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupSuccessfulValidations();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateProductIsPublishedAsync(
                    ProductId,
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallValidateProductIsPublishedAsync_WithCorrectProductId()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupSuccessfulValidations();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateProductIsPublishedAsync(
                    Arg.Is<long>(id => id == productSku.ProductId),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallValidateAttributeValuesAreActive_WhenProductIsPublished()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupSuccessfulValidations();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateAttributeValuesAreActive(
                    ProductSkuId,
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallValidateAttributeValuesAreActive_WithCorrectProductSkuId()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupSuccessfulValidations();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateAttributeValuesAreActive(
                    Arg.Is<long>(id => id == productSku.Id),
                    Arg.Any<CancellationToken>());
        }

        #endregion

        #region Repository Interaction Scenarios

        [Fact]
        public async Task Handle_Should_CallGetByIdAsync_Always()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupSuccessfulValidations();

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
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupSuccessfulValidations();

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
            var command = new ActivateProductSkuCommand(nonExistentProductSkuId);

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
        public async Task Handle_Should_PreserveProductSkuId_AfterActivation()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupSuccessfulValidations();
            long originalId = productSku.Id;

            // Act
            await _handler.Handle(_command, default);

            // Assert
            productSku.Id.Should().Be(originalId);
        }

        [Fact]
        public async Task Handle_Should_PreserveProductId_AfterActivation()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupSuccessfulValidations();
            long originalProductId = productSku.ProductId;

            // Act
            await _handler.Handle(_command, default);

            // Assert
            productSku.ProductId.Should().Be(originalProductId);
        }

        [Fact]
        public async Task Handle_Should_PreserveSkuCode_AfterActivation()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupSuccessfulValidations();
            string originalSkuCode = productSku.SkuCode!;

            // Act
            await _handler.Handle(_command, default);

            // Assert
            productSku.SkuCode.Should().Be(originalSkuCode);
        }

        [Fact]
        public async Task Handle_Should_OnlyModifyIsActiveProperty_WhenActivatingProductSku()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            string originalSkuCode = productSku.SkuCode!;
            long originalProductId = productSku.ProductId;
            decimal originalPrice = productSku.Price;

            SetupProductSkuFound(productSku);
            SetupSuccessfulValidations();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            productSku.IsActive.Should().BeTrue();
            productSku.SkuCode.Should().Be(originalSkuCode);
            productSku.ProductId.Should().Be(originalProductId);
            productSku.Price.Should().Be(originalPrice);
        }

        [Fact]
        public async Task Handle_Should_ExecuteValidationsInCorrectOrder_WhenActivating()
        {
            // Arrange
            ProductSku productSku = CreateInactiveProductSku();
            SetupProductSkuFound(productSku);
            SetupSuccessfulValidations();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(async () =>
            {
                await _validatorMock.ValidateProductIsPublishedAsync(Arg.Any<long>(), Arg.Any<CancellationToken>());
                await _validatorMock.ValidateAttributeValuesAreActive(Arg.Any<long>(), Arg.Any<CancellationToken>());
            });
        }

        #endregion
    }
}

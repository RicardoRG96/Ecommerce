using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.ProductSkus.Common.Services;
using Application.Products.ProductSkus.Update;
using Domain.Entities.Products;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Products.ProductSkus.Update
{
    public class UpdateProductSkuCommandTests
    {
        private const long ProductSkuId = 1;
        private const long ProductId = 10;
        private const long OriginalProductId = 5;
        private const string BarCode = "9876543210123";
        private const string OriginalBarCode = "1234567890123";
        private const string OriginalSkuCode = "PROD-001-SKU-001";
        private const decimal Price = 1299.99m;
        private const decimal OriginalPrice = 999.99m;
        private const decimal Cost = 600.50m;
        private const decimal OriginalCost = 500.50m;
        private const decimal Weight = 2.5m;
        private const decimal OriginalWeight = 1.5m;
        private const decimal Length = 15.0m;
        private const decimal OriginalLength = 10.0m;
        private const decimal Width = 8.0m;
        private const decimal OriginalWidth = 5.0m;
        private const decimal Height = 5.0m;
        private const decimal OriginalHeight = 3.0m;
        private const int DisplayOrder = 10;
        private const int OriginalDisplayOrder = 1;

        private static readonly UpdateProductSkuCommand _command = new(
            Id: ProductSkuId,
            ProductId: ProductId,
            BarCode: BarCode,
            Price: Price,
            Cost: Cost,
            Weight: Weight,
            Length: Length,
            Width: Width,
            Height: Height,
            DisplayOrder: DisplayOrder);

        private readonly UpdateProductSkuCommandHandler _handler;
        private readonly IProductSkuRepository _productSkuRepositoryMock;
        private readonly IProductSkuValidator _validatorMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public UpdateProductSkuCommandTests()
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

        private ProductSku CreateExistingProductSku()
        {
            return new ProductSku
            {
                Id = ProductSkuId,
                ProductId = OriginalProductId,
                SkuCode = OriginalSkuCode,
                BarCode = OriginalBarCode,
                Price = OriginalPrice,
                Cost = OriginalCost,
                Weight = OriginalWeight,
                Length = OriginalLength,
                Width = OriginalWidth,
                Height = OriginalHeight,
                ComparedAtPrice = null,
                IsActive = true,
                DisplayOrder = OriginalDisplayOrder
            };
        }

        private void SetupSuccessfulScenario(ProductSku productSku)
        {
            _productSkuRepositoryMock
                .GetByIdAsync(ProductSkuId, Arg.Any<CancellationToken>())
                .Returns(productSku);

            _validatorMock
                .ValidateProductIsPublishedAsync(
                    _command.ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateBarCodeIsUnique(
                    _command.BarCode,
                    ProductSkuId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());
        }

        private void SetupProductSkuNotFound()
        {
            _productSkuRepositoryMock
                .GetByIdAsync(ProductSkuId, Arg.Any<CancellationToken>())
                .Returns((ProductSku?)null);
        }

        private void SetupProductNotPublishedFailure(ProductSku productSku, Error error)
        {
            _productSkuRepositoryMock
                .GetByIdAsync(ProductSkuId, Arg.Any<CancellationToken>())
                .Returns(productSku);

            _validatorMock
                .ValidateProductIsPublishedAsync(
                    _command.ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        private void SetupBarCodeNotUniqueFailure(ProductSku productSku, Error error)
        {
            _productSkuRepositoryMock
                .GetByIdAsync(ProductSkuId, Arg.Any<CancellationToken>())
                .Returns(productSku);

            _validatorMock
                .ValidateProductIsPublishedAsync(
                    _command.ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateBarCodeIsUnique(
                    _command.BarCode,
                    ProductSkuId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        #endregion

        #region Success Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenProductSkuExistsAndAllValidationsPass()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupSuccessfulScenario(existingProductSku);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_UpdateBasicProperties_WhenProductSkuIsValid()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupSuccessfulScenario(existingProductSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingProductSku.ProductId.Should().Be(_command.ProductId);
            existingProductSku.BarCode.Should().Be(_command.BarCode);
            existingProductSku.Price.Should().Be(_command.Price);
            existingProductSku.DisplayOrder.Should().Be(_command.DisplayOrder);
        }

        [Fact]
        public async Task Handle_Should_UpdateOptionalProperties_WhenProvided()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupSuccessfulScenario(existingProductSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingProductSku.Cost.Should().Be(Cost);
            existingProductSku.Weight.Should().Be(Weight);
            existingProductSku.Length.Should().Be(Length);
            existingProductSku.Width.Should().Be(Width);
            existingProductSku.Height.Should().Be(Height);
        }

        [Fact]
        public async Task Handle_Should_UpdateOptionalPropertiesToNull_WhenNotProvided()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            var commandWithNulls = _command with
            {
                Cost = null,
                Weight = null,
                Length = null,
                Width = null,
                Height = null
            };

            _productSkuRepositoryMock
                .GetByIdAsync(ProductSkuId, Arg.Any<CancellationToken>())
                .Returns(existingProductSku);

            _validatorMock
                .ValidateProductIsPublishedAsync(
                    commandWithNulls.ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateBarCodeIsUnique(
                    commandWithNulls.BarCode,
                    ProductSkuId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithNulls, default);

            // Assert
            existingProductSku.Cost.Should().BeNull();
            existingProductSku.Weight.Should().BeNull();
            existingProductSku.Length.Should().BeNull();
            existingProductSku.Width.Should().BeNull();
            existingProductSku.Height.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_UpdateBarCodeToNull_WhenNotProvided()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            var commandWithoutBarCode = _command with { BarCode = null };

            _productSkuRepositoryMock
                .GetByIdAsync(ProductSkuId, Arg.Any<CancellationToken>())
                .Returns(existingProductSku);

            _validatorMock
                .ValidateProductIsPublishedAsync(
                    commandWithoutBarCode.ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateBarCodeIsUnique(
                    null,
                    ProductSkuId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithoutBarCode, default);

            // Assert
            existingProductSku.BarCode.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_AllowSameBarCode_WhenUpdatingSameProductSku()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            existingProductSku.BarCode = _command.BarCode;

            SetupSuccessfulScenario(existingProductSku);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_UpdateProductId_WhenChanged()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupSuccessfulScenario(existingProductSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingProductSku.ProductId.Should().Be(ProductId);
            existingProductSku.ProductId.Should().NotBe(OriginalProductId);
        }

        [Fact]
        public async Task Handle_Should_UpdatePriceToZero_WhenSpecified()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            var commandWithZeroPrice = _command with { Price = 0m };

            _productSkuRepositoryMock
                .GetByIdAsync(ProductSkuId, Arg.Any<CancellationToken>())
                .Returns(existingProductSku);

            _validatorMock
                .ValidateProductIsPublishedAsync(
                    commandWithZeroPrice.ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateBarCodeIsUnique(
                    commandWithZeroPrice.BarCode,
                    ProductSkuId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithZeroPrice, default);

            // Assert
            existingProductSku.Price.Should().Be(0m);
        }

        [Fact]
        public async Task Handle_Should_UpdateDisplayOrderToZero_WhenSpecified()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            var commandWithZeroDisplayOrder = _command with { DisplayOrder = 0 };

            _productSkuRepositoryMock
                .GetByIdAsync(ProductSkuId, Arg.Any<CancellationToken>())
                .Returns(existingProductSku);

            _validatorMock
                .ValidateProductIsPublishedAsync(
                    commandWithZeroDisplayOrder.ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateBarCodeIsUnique(
                    commandWithZeroDisplayOrder.BarCode,
                    ProductSkuId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithZeroDisplayOrder, default);

            // Assert
            existingProductSku.DisplayOrder.Should().Be(0);
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
        public async Task Handle_Should_NotCallValidateBarCodeIsUnique_WhenProductSkuDoesNotExist()
        {
            // Arrange
            SetupProductSkuNotFound();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .DidNotReceive()
                .ValidateBarCodeIsUnique(
                    Arg.Any<string?>(),
                    Arg.Any<long?>(),
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

        #region Product Not Published Failure Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenProductNotFound()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupProductNotPublishedFailure(existingProductSku, ProductErrors.NotFound(ProductId));

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
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupProductNotPublishedFailure(existingProductSku, ProductSkuErrors.ProductNotPublished);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductSkuErrors.ProductNotPublished);
        }

        [Fact]
        public async Task Handle_Should_NotCallValidateBarCodeIsUnique_WhenProductNotPublished()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupProductNotPublishedFailure(existingProductSku, ProductSkuErrors.ProductNotPublished);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .DidNotReceive()
                .ValidateBarCodeIsUnique(
                    Arg.Any<string?>(),
                    Arg.Any<long?>(),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenProductNotPublished()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupProductNotPublishedFailure(existingProductSku, ProductSkuErrors.ProductNotPublished);

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
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupProductNotPublishedFailure(existingProductSku, ProductSkuErrors.ProductNotPublished);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region BarCode Not Unique Failure Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenBarCodeAlreadyExists()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupBarCodeNotUniqueFailure(existingProductSku, ProductSkuErrors.DuplicatedBarCode);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductSkuErrors.DuplicatedBarCode);
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenBarCodeAlreadyExists()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupBarCodeNotUniqueFailure(existingProductSku, ProductSkuErrors.DuplicatedBarCode);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productSkuRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<ProductSku>());
        }

        [Fact]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenBarCodeAlreadyExists()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupBarCodeNotUniqueFailure(existingProductSku, ProductSkuErrors.DuplicatedBarCode);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region Validation Scenarios

        [Fact]
        public async Task Handle_Should_CallGetByIdAsync_Always()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupSuccessfulScenario(existingProductSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productSkuRepositoryMock
                .Received(1)
                .GetByIdAsync(ProductSkuId, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallValidateProductIsPublishedAsync_WhenProductSkuExists()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupSuccessfulScenario(existingProductSku);

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
        public async Task Handle_Should_CallValidateBarCodeIsUnique_WhenProductIsPublished()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupSuccessfulScenario(existingProductSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateBarCodeIsUnique(
                    BarCode,
                    ProductSkuId,
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_PassCorrectProductSkuIdToValidator_WhenValidating()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupSuccessfulScenario(existingProductSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateBarCodeIsUnique(
                    Arg.Any<string?>(),
                    ProductSkuId,
                    Arg.Any<CancellationToken>());
        }

        #endregion

        #region Repository and UnitOfWork Interactions

        [Fact]
        public async Task Handle_Should_CallUpdate_ExactlyOnce_WhenProductSkuIsValid()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupSuccessfulScenario(existingProductSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productSkuRepositoryMock
                .Received(1)
                .Update(Arg.Any<ProductSku>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_ExactlyOnce_WhenProductSkuIsValid()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupSuccessfulScenario(existingProductSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_AfterUpdate_WhenProductSkuIsValid()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupSuccessfulScenario(existingProductSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(() =>
            {
                _productSkuRepositoryMock.Update(Arg.Any<ProductSku>());
                _unitOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>());
            });
        }

        [Fact]
        public async Task Handle_Should_UpdateCorrectProductSkuInstance_WhenProductSkuIsValid()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupSuccessfulScenario(existingProductSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productSkuRepositoryMock
                .Received(1)
                .Update(existingProductSku);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public async Task Handle_Should_UpdateDisplayOrder_ToLargeValue()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            var commandWithLargeDisplayOrder = _command with { DisplayOrder = 999 };

            _productSkuRepositoryMock
                .GetByIdAsync(ProductSkuId, Arg.Any<CancellationToken>())
                .Returns(existingProductSku);

            _validatorMock
                .ValidateProductIsPublishedAsync(
                    commandWithLargeDisplayOrder.ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateBarCodeIsUnique(
                    commandWithLargeDisplayOrder.BarCode,
                    ProductSkuId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithLargeDisplayOrder, default);

            // Assert
            existingProductSku.DisplayOrder.Should().Be(999);
        }

        [Fact]
        public async Task Handle_Should_PreserveProductSkuId_AfterUpdate()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupSuccessfulScenario(existingProductSku);
            long originalId = existingProductSku.Id;

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingProductSku.Id.Should().Be(originalId);
        }

        [Fact]
        public async Task Handle_Should_PreserveSkuCode_AfterUpdate()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupSuccessfulScenario(existingProductSku);
            string originalSkuCode = existingProductSku.SkuCode!;

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingProductSku.SkuCode.Should().Be(originalSkuCode);
        }

        [Fact]
        public async Task Handle_Should_NotChangeIsActiveProperty_WhenUpdating()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            existingProductSku.IsActive = true;
            SetupSuccessfulScenario(existingProductSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingProductSku.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_NotChangeComparedAtPriceProperty_WhenUpdating()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            existingProductSku.ComparedAtPrice = 1499.99m;
            SetupSuccessfulScenario(existingProductSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingProductSku.ComparedAtPrice.Should().Be(1499.99m);
        }

        [Fact]
        public async Task Handle_Should_UpdateAllPropertiesAtOnce_WhenAllAreChanged()
        {
            // Arrange
            ProductSku existingProductSku = CreateExistingProductSku();
            SetupSuccessfulScenario(existingProductSku);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingProductSku.ProductId.Should().Be(_command.ProductId);
            existingProductSku.BarCode.Should().Be(_command.BarCode);
            existingProductSku.Price.Should().Be(_command.Price);
            existingProductSku.Cost.Should().Be(_command.Cost);
            existingProductSku.Weight.Should().Be(_command.Weight);
            existingProductSku.Length.Should().Be(_command.Length);
            existingProductSku.Width.Should().Be(_command.Width);
            existingProductSku.Height.Should().Be(_command.Height);
            existingProductSku.DisplayOrder.Should().Be(_command.DisplayOrder);
        }

        #endregion
    }
}

using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.ProductSkus.Common.Services;
using Application.Products.ProductSkus.Create;
using Domain.Entities.Products;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Products.ProductSkus.Create
{
    public class CreateProductSkuCommandTests
    {
        private const long ProductId = 1;
        private const string BarCode = "1234567890123";
        private const decimal Price = 999.99m;
        private const decimal Cost = 500.50m;
        private const decimal Weight = 1.5m;
        private const decimal Length = 10.0m;
        private const decimal Width = 5.0m;
        private const decimal Height = 3.0m;
        private const int DisplayOrder = 1;
        private const string GeneratedSkuCode = "PROD-001-SKU-001";

        private static readonly CreateProductSkuCommand _command = new(
            ProductId: ProductId,
            BarCode: BarCode,
            Price: Price,
            Cost: Cost,
            Weight: Weight,
            Length: Length,
            Width: Width,
            Height: Height,
            IsActive: true,
            DisplayOrder: DisplayOrder);

        private readonly CreateProductSkuCommandHandler _handler;
        private readonly IProductSkuRepository _productSkuRepositoryMock;
        private readonly IProductSkuValidator _validatorMock;
        private readonly ISkuGenerator _skuGeneratorMock;
        private readonly ISkuGenerationContextBuilder _skuGenerationContextBuilderMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public CreateProductSkuCommandTests()
        {
            _productSkuRepositoryMock = Substitute.For<IProductSkuRepository>();
            _validatorMock = Substitute.For<IProductSkuValidator>();
            _skuGeneratorMock = Substitute.For<ISkuGenerator>();
            _skuGenerationContextBuilderMock = Substitute.For<ISkuGenerationContextBuilder>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _handler = new(
                _productSkuRepositoryMock,
                _validatorMock,
                _skuGeneratorMock,
                _skuGenerationContextBuilderMock,
                _unitOfWorkMock);
        }

        #region Helper Methods

        private void SetupSuccessfulValidation()
        {
            _validatorMock
                .ValidateProductIsPublishedAsync(
                    _command.ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateBarCodeIsUnique(
                    _command.BarCode,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateSkuCodeIsUnique(
                    GeneratedSkuCode,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());
        }

        private void SetupSkuGeneration()
        {
            var context = new SkuGenerationContext
            {
                ProductSkuId = 0,
                ProductId = _command.ProductId,
                CategoryCode = "CAT001",
                BrandCode = "BRD001",
                ProductCode = "PROD-001",
                Variants = null,
                GeneratedAt = DateTime.UtcNow
            };

            _skuGenerationContextBuilderMock
                .BuildForProductAsync(
                    _command.ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(context);

            _skuGeneratorMock
                .Generate(Arg.Any<SkuGenerationContext>())
                .Returns(GeneratedSkuCode);
        }

        private void SetupProductNotPublishedFailure(Error error)
        {
            _validatorMock
                .ValidateProductIsPublishedAsync(
                    _command.ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        private void SetupBarCodeNotUniqueFailure(Error error)
        {
            _validatorMock
                .ValidateProductIsPublishedAsync(
                    _command.ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateBarCodeIsUnique(
                    _command.BarCode,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        private void SetupSkuCodeNotUniqueFailure(Error error)
        {
            _validatorMock
                .ValidateProductIsPublishedAsync(
                    _command.ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateBarCodeIsUnique(
                    _command.BarCode,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            SetupSkuGeneration();

            _validatorMock
                .ValidateSkuCodeIsUnique(
                    GeneratedSkuCode,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        #endregion

        #region Success Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenProductSkuIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();
            SetupSkuGeneration();

            // Act
            Result<long> result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_ReturnProductSkuId_WhenProductSkuIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();
            SetupSkuGeneration();

            // Act
            Result<long> result = await _handler.Handle(_command, default);

            // Assert
            result.Value.Should().BeGreaterThanOrEqualTo(0);
        }

        [Fact]
        public async Task Handle_Should_GenerateSkuCode_WhenProductSkuIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();
            SetupSkuGeneration();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productSkuRepositoryMock
                .Received(1)
                .AddAsync(
                    Arg.Is<ProductSku>(ps => ps.SkuCode == GeneratedSkuCode),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateProductSkuWithCorrectBasicProperties_WhenProductSkuIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();
            SetupSkuGeneration();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productSkuRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<ProductSku>(ps =>
                    ps.ProductId == _command.ProductId &&
                    ps.BarCode == _command.BarCode &&
                    ps.Price == _command.Price &&
                    ps.DisplayOrder == DisplayOrder &&
                    ps.IsActive == _command.IsActive),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateProductSkuWithOptionalProperties_WhenProvided()
        {
            // Arrange
            SetupSuccessfulValidation();
            SetupSkuGeneration();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productSkuRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<ProductSku>(ps =>
                    ps.Cost == Cost &&
                    ps.Weight == Weight &&
                    ps.Length == Length &&
                    ps.Width == Width &&
                    ps.Height == Height),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateProductSkuWithNullOptionalProperties_WhenNotProvided()
        {
            // Arrange
            var commandWithoutOptionals = _command with
            {
                Cost = null,
                Weight = null,
                Length = null,
                Width = null,
                Height = null
            };

            _validatorMock
                .ValidateProductIsPublishedAsync(
                    commandWithoutOptionals.ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateBarCodeIsUnique(
                    commandWithoutOptionals.BarCode,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateSkuCodeIsUnique(
                    GeneratedSkuCode,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            SetupSkuGeneration();

            // Act
            await _handler.Handle(commandWithoutOptionals, default);

            // Assert
            await _productSkuRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<ProductSku>(ps =>
                    ps.Cost == null &&
                    ps.Weight == null &&
                    ps.Length == null &&
                    ps.Width == null &&
                    ps.Height == null),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateProductSkuWithNullBarCode_WhenNotProvided()
        {
            // Arrange
            var commandWithoutBarCode = _command with { BarCode = null };

            _validatorMock
                .ValidateProductIsPublishedAsync(
                    commandWithoutBarCode.ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateBarCodeIsUnique(
                    null,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateSkuCodeIsUnique(
                    GeneratedSkuCode,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            SetupSkuGeneration();

            // Act
            await _handler.Handle(commandWithoutBarCode, default);

            // Assert
            await _productSkuRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<ProductSku>(ps => ps.BarCode == null),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_SetComparedAtPriceToNull_WhenCreatingProductSku()
        {
            // Arrange
            SetupSuccessfulValidation();
            SetupSkuGeneration();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productSkuRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<ProductSku>(ps => ps.ComparedAtPrice == null),
                    Arg.Any<CancellationToken>());
        }

        #endregion

        #region Validation Scenarios

        [Fact]
        public async Task Handle_Should_CallValidateProductIsPublishedAsync_Always()
        {
            // Arrange
            SetupSuccessfulValidation();
            SetupSkuGeneration();

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
            SetupSuccessfulValidation();
            SetupSkuGeneration();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateBarCodeIsUnique(
                    BarCode,
                    null,
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallBuildForProductAsync_WhenValidationsPassed()
        {
            // Arrange
            SetupSuccessfulValidation();
            SetupSkuGeneration();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _skuGenerationContextBuilderMock
                .Received(1)
                .BuildForProductAsync(
                    ProductId,
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallGenerate_WhenValidationsPassed()
        {
            // Arrange
            SetupSuccessfulValidation();
            SetupSkuGeneration();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _skuGeneratorMock
                .Received(1)
                .Generate(Arg.Any<SkuGenerationContext>());
        }

        [Fact]
        public async Task Handle_Should_CallValidateSkuCodeIsUnique_AfterGeneratingSku()
        {
            // Arrange
            SetupSuccessfulValidation();
            SetupSkuGeneration();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateSkuCodeIsUnique(
                    GeneratedSkuCode,
                    null,
                    Arg.Any<CancellationToken>());
        }

        #endregion

        #region Product Not Published Failure Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenProductNotFound()
        {
            // Arrange
            SetupProductNotPublishedFailure(ProductErrors.NotFound(ProductId));

            // Act
            Result<long> result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductErrors.NotFound(ProductId));
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenProductNotPublished()
        {
            // Arrange
            SetupProductNotPublishedFailure(ProductSkuErrors.ProductNotPublished);

            // Act
            Result<long> result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductSkuErrors.ProductNotPublished);
        }

        [Fact]
        public async Task Handle_Should_NotCallValidateBarCodeIsUnique_WhenProductNotPublished()
        {
            // Arrange
            SetupProductNotPublishedFailure(ProductSkuErrors.ProductNotPublished);

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
        public async Task Handle_Should_NotCallSkuGeneration_WhenProductNotPublished()
        {
            // Arrange
            SetupProductNotPublishedFailure(ProductSkuErrors.ProductNotPublished);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _skuGenerationContextBuilderMock
                .DidNotReceive()
                .BuildForProductAsync(
                    Arg.Any<long>(),
                    Arg.Any<CancellationToken>());

            _skuGeneratorMock
                .DidNotReceive()
                .Generate(Arg.Any<SkuGenerationContext>());
        }

        [Fact]
        public async Task Handle_Should_NotCallAddAsync_WhenProductNotPublished()
        {
            // Arrange
            SetupProductNotPublishedFailure(ProductSkuErrors.ProductNotPublished);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productSkuRepositoryMock
                .DidNotReceive()
                .AddAsync(Arg.Any<ProductSku>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUnitOfWork_WhenProductNotPublished()
        {
            // Arrange
            SetupProductNotPublishedFailure(ProductSkuErrors.ProductNotPublished);

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
            SetupBarCodeNotUniqueFailure(ProductSkuErrors.DuplicatedBarCode);

            // Act
            Result<long> result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductSkuErrors.DuplicatedBarCode);
        }

        [Fact]
        public async Task Handle_Should_NotCallSkuGeneration_WhenBarCodeAlreadyExists()
        {
            // Arrange
            SetupBarCodeNotUniqueFailure(ProductSkuErrors.DuplicatedBarCode);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _skuGenerationContextBuilderMock
                .DidNotReceive()
                .BuildForProductAsync(
                    Arg.Any<long>(),
                    Arg.Any<CancellationToken>());

            _skuGeneratorMock
                .DidNotReceive()
                .Generate(Arg.Any<SkuGenerationContext>());
        }

        [Fact]
        public async Task Handle_Should_NotCallAddAsync_WhenBarCodeAlreadyExists()
        {
            // Arrange
            SetupBarCodeNotUniqueFailure(ProductSkuErrors.DuplicatedBarCode);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productSkuRepositoryMock
                .DidNotReceive()
                .AddAsync(Arg.Any<ProductSku>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUnitOfWork_WhenBarCodeAlreadyExists()
        {
            // Arrange
            SetupBarCodeNotUniqueFailure(ProductSkuErrors.DuplicatedBarCode);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region SkuCode Not Unique Failure Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenSkuCodeAlreadyExists()
        {
            // Arrange
            SetupSkuCodeNotUniqueFailure(ProductSkuErrors.DuplicatedSkuCode);

            // Act
            Result<long> result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductSkuErrors.DuplicatedSkuCode);
        }

        [Fact]
        public async Task Handle_Should_NotCallAddAsync_WhenSkuCodeAlreadyExists()
        {
            // Arrange
            SetupSkuCodeNotUniqueFailure(ProductSkuErrors.DuplicatedSkuCode);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productSkuRepositoryMock
                .DidNotReceive()
                .AddAsync(Arg.Any<ProductSku>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUnitOfWork_WhenSkuCodeAlreadyExists()
        {
            // Arrange
            SetupSkuCodeNotUniqueFailure(ProductSkuErrors.DuplicatedSkuCode);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region Repository and UnitOfWork Interactions

        [Fact]
        public async Task Handle_Should_CallAddAsync_ExactlyOnce_WhenProductSkuIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();
            SetupSkuGeneration();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productSkuRepositoryMock
                .Received(1)
                .AddAsync(Arg.Any<ProductSku>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_ExactlyOnce_WhenProductSkuIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();
            SetupSkuGeneration();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_AfterAddAsync_WhenProductSkuIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();
            SetupSkuGeneration();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(async () =>
            {
                await _productSkuRepositoryMock.AddAsync(Arg.Any<ProductSku>(), Arg.Any<CancellationToken>());
                await _unitOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>());
            });
        }

        #endregion

        #region Edge Cases

        [Fact]
        public async Task Handle_Should_CreateProductSku_WithZeroDisplayOrder()
        {
            // Arrange
            var commandWithZeroDisplayOrder = _command with { DisplayOrder = 0 };

            _validatorMock
                .ValidateProductIsPublishedAsync(
                    commandWithZeroDisplayOrder.ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateBarCodeIsUnique(
                    commandWithZeroDisplayOrder.BarCode,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateSkuCodeIsUnique(
                    GeneratedSkuCode,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            SetupSkuGeneration();

            // Act
            Result<long> result = await _handler.Handle(commandWithZeroDisplayOrder, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            await _productSkuRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<ProductSku>(ps => ps.DisplayOrder == 0),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateProductSku_WithIsActiveFalse()
        {
            // Arrange
            var commandWithInactive = _command with { IsActive = false };

            _validatorMock
                .ValidateProductIsPublishedAsync(
                    commandWithInactive.ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateBarCodeIsUnique(
                    commandWithInactive.BarCode,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateSkuCodeIsUnique(
                    GeneratedSkuCode,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            SetupSkuGeneration();

            // Act
            Result<long> result = await _handler.Handle(commandWithInactive, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            await _productSkuRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<ProductSku>(ps => ps.IsActive == false),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateProductSku_WithZeroPrice()
        {
            // Arrange
            var commandWithZeroPrice = _command with { Price = 0m };

            _validatorMock
                .ValidateProductIsPublishedAsync(
                    commandWithZeroPrice.ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateBarCodeIsUnique(
                    commandWithZeroPrice.BarCode,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateSkuCodeIsUnique(
                    GeneratedSkuCode,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            SetupSkuGeneration();

            // Act
            Result<long> result = await _handler.Handle(commandWithZeroPrice, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            await _productSkuRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<ProductSku>(ps => ps.Price == 0m),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateProductSku_WithDifferentProductIds()
        {
            // Arrange
            long differentProductId = 999;
            var commandWithDifferentProduct = _command with { ProductId = differentProductId };

            _validatorMock
                .ValidateProductIsPublishedAsync(
                    differentProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateBarCodeIsUnique(
                    commandWithDifferentProduct.BarCode,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateSkuCodeIsUnique(
                    GeneratedSkuCode,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            SetupSkuGeneration();

            // Act
            Result<long> result = await _handler.Handle(commandWithDifferentProduct, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            await _productSkuRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<ProductSku>(ps => ps.ProductId == differentProductId),
                    Arg.Any<CancellationToken>());
        }

        #endregion
    }
}

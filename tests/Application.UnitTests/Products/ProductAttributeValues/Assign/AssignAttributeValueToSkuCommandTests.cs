using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.ProductAttributeValues.Assign;
using Application.Products.ProductAttributeValues.Common.Services;
using Domain.Entities.Products;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Products.ProductAttributeValues.Assign
{
    public class AssignAttributeValueToSkuCommandTests
    {
        private const long SkuId = 1;
        private const long AttributeValueId = 2;

        private static readonly AssignAttributeValueToSkuCommand _command = new(
            SkuId: SkuId,
            AttributeValueId: AttributeValueId);

        private readonly AssignAttributeValueToSkuCommandHandler _handler;
        private readonly IProductAttributeValueRepository _productAttributeValueRepositoryMock;
        private readonly IProductAttributeValueValidator _validatorMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public AssignAttributeValueToSkuCommandTests()
        {
            _productAttributeValueRepositoryMock = Substitute.For<IProductAttributeValueRepository>();
            _validatorMock = Substitute.For<IProductAttributeValueValidator>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _handler = new(
                _productAttributeValueRepositoryMock,
                _validatorMock,
                _unitOfWorkMock);
        }

        #region Helper Methods

        private void SetupSuccessfulValidation()
        {
            _validatorMock
                .ValidateAttributeValueIsActive(
                    _command.AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateSkuIsActive(
                    _command.SkuId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateAttributeValueIsNotAlreadyAssignedToSku(
                    _command.SkuId,
                    _command.AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());
        }

        private void SetupAttributeValueNotActiveFailure(Error error)
        {
            _validatorMock
                .ValidateAttributeValueIsActive(
                    _command.AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        private void SetupSkuNotActiveFailure(Error error)
        {
            _validatorMock
                .ValidateAttributeValueIsActive(
                    _command.AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateSkuIsActive(
                    _command.SkuId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        private void SetupAlreadyAssignedFailure(Error error)
        {
            _validatorMock
                .ValidateAttributeValueIsActive(
                    _command.AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateSkuIsActive(
                    _command.SkuId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateAttributeValueIsNotAlreadyAssignedToSku(
                    _command.SkuId,
                    _command.AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        #endregion

        #region Success Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenAssignmentIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_CreateProductAttributeValueWithCorrectProperties_WhenAssignmentIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productAttributeValueRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<ProductAttributeValue>(pav =>
                    pav.ProductSkuId == SkuId &&
                    pav.AttributeValueId == AttributeValueId),
                    Arg.Any<CancellationToken>());
        }

        #endregion

        #region Validation Scenarios

        [Fact]
        public async Task Handle_Should_CallValidateAttributeValueIsActive_Always()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateAttributeValueIsActive(
                    AttributeValueId,
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallValidateSkuIsActive_WhenAttributeValueIsActive()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateSkuIsActive(
                    SkuId,
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallValidateAttributeValueIsNotAlreadyAssignedToSku_WhenSkuIsActive()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateAttributeValueIsNotAlreadyAssignedToSku(
                    SkuId,
                    AttributeValueId,
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_ExecuteValidationsInCorrectOrder()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(async () =>
            {
                await _validatorMock.ValidateAttributeValueIsActive(Arg.Any<long>(), Arg.Any<CancellationToken>());
                await _validatorMock.ValidateSkuIsActive(Arg.Any<long>(), Arg.Any<CancellationToken>());
                await _validatorMock.ValidateAttributeValueIsNotAlreadyAssignedToSku(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<CancellationToken>());
            });
        }

        #endregion

        #region AttributeValue Not Active Failure Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenAttributeValueIsNotActive()
        {
            // Arrange
            SetupAttributeValueNotActiveFailure(ProductAttributeValueErrors.AttributeValueNotActive);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductAttributeValueErrors.AttributeValueNotActive);
        }

        [Fact]
        public async Task Handle_Should_NotCallValidateSkuIsActive_WhenAttributeValueIsNotActive()
        {
            // Arrange
            SetupAttributeValueNotActiveFailure(ProductAttributeValueErrors.AttributeValueNotActive);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .DidNotReceive()
                .ValidateSkuIsActive(
                    Arg.Any<long>(),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallValidateAttributeValueIsNotAlreadyAssignedToSku_WhenAttributeValueIsNotActive()
        {
            // Arrange
            SetupAttributeValueNotActiveFailure(ProductAttributeValueErrors.AttributeValueNotActive);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .DidNotReceive()
                .ValidateAttributeValueIsNotAlreadyAssignedToSku(
                    Arg.Any<long>(),
                    Arg.Any<long>(),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallAddAsync_WhenAttributeValueIsNotActive()
        {
            // Arrange
            SetupAttributeValueNotActiveFailure(ProductAttributeValueErrors.AttributeValueNotActive);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productAttributeValueRepositoryMock
                .DidNotReceive()
                .AddAsync(Arg.Any<ProductAttributeValue>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUnitOfWork_WhenAttributeValueIsNotActive()
        {
            // Arrange
            SetupAttributeValueNotActiveFailure(ProductAttributeValueErrors.AttributeValueNotActive);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region Sku Not Active Failure Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenSkuIsNotActive()
        {
            // Arrange
            SetupSkuNotActiveFailure(ProductAttributeValueErrors.ProductSkuNotActive);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductAttributeValueErrors.ProductSkuNotActive);
        }

        [Fact]
        public async Task Handle_Should_NotCallValidateAttributeValueIsNotAlreadyAssignedToSku_WhenSkuIsNotActive()
        {
            // Arrange
            SetupSkuNotActiveFailure(ProductAttributeValueErrors.ProductSkuNotActive);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .DidNotReceive()
                .ValidateAttributeValueIsNotAlreadyAssignedToSku(
                    Arg.Any<long>(),
                    Arg.Any<long>(),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallAddAsync_WhenSkuIsNotActive()
        {
            // Arrange
            SetupSkuNotActiveFailure(ProductAttributeValueErrors.ProductSkuNotActive);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productAttributeValueRepositoryMock
                .DidNotReceive()
                .AddAsync(Arg.Any<ProductAttributeValue>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUnitOfWork_WhenSkuIsNotActive()
        {
            // Arrange
            SetupSkuNotActiveFailure(ProductAttributeValueErrors.ProductSkuNotActive);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region Already Assigned Failure Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenAttributeValueIsAlreadyAssignedToSku()
        {
            // Arrange
            SetupAlreadyAssignedFailure(ProductAttributeValueErrors.Duplicated);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductAttributeValueErrors.Duplicated);
        }

        [Fact]
        public async Task Handle_Should_NotCallAddAsync_WhenAttributeValueIsAlreadyAssignedToSku()
        {
            // Arrange
            SetupAlreadyAssignedFailure(ProductAttributeValueErrors.Duplicated);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productAttributeValueRepositoryMock
                .DidNotReceive()
                .AddAsync(Arg.Any<ProductAttributeValue>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUnitOfWork_WhenAttributeValueIsAlreadyAssignedToSku()
        {
            // Arrange
            SetupAlreadyAssignedFailure(ProductAttributeValueErrors.Duplicated);

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
        public async Task Handle_Should_CallAddAsync_ExactlyOnce_WhenAssignmentIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productAttributeValueRepositoryMock
                .Received(1)
                .AddAsync(Arg.Any<ProductAttributeValue>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_ExactlyOnce_WhenAssignmentIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_AfterAddAsync_WhenAssignmentIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(async () =>
            {
                await _productAttributeValueRepositoryMock.AddAsync(Arg.Any<ProductAttributeValue>(), Arg.Any<CancellationToken>());
                await _unitOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>());
            });
        }

        #endregion

        #region Edge Cases

        [Fact]
        public async Task Handle_Should_CreateProductAttributeValue_WithDifferentSkuIds()
        {
            // Arrange
            long differentSkuId = 999;
            var commandWithDifferentSku = _command with { SkuId = differentSkuId };

            _validatorMock
                .ValidateAttributeValueIsActive(
                    commandWithDifferentSku.AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateSkuIsActive(
                    differentSkuId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateAttributeValueIsNotAlreadyAssignedToSku(
                    differentSkuId,
                    commandWithDifferentSku.AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            Result result = await _handler.Handle(commandWithDifferentSku, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            await _productAttributeValueRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<ProductAttributeValue>(pav =>
                    pav.ProductSkuId == differentSkuId),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateProductAttributeValue_WithDifferentAttributeValueIds()
        {
            // Arrange
            long differentAttributeValueId = 888;
            var commandWithDifferentAttributeValue = _command with { AttributeValueId = differentAttributeValueId };

            _validatorMock
                .ValidateAttributeValueIsActive(
                    differentAttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateSkuIsActive(
                    commandWithDifferentAttributeValue.SkuId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateAttributeValueIsNotAlreadyAssignedToSku(
                    commandWithDifferentAttributeValue.SkuId,
                    differentAttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            Result result = await _handler.Handle(commandWithDifferentAttributeValue, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            await _productAttributeValueRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<ProductAttributeValue>(pav =>
                    pav.AttributeValueId == differentAttributeValueId),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_PassCorrectSkuIdToValidator()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateSkuIsActive(
                    Arg.Is<long>(id => id == SkuId),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_PassCorrectAttributeValueIdToValidator()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateAttributeValueIsActive(
                    Arg.Is<long>(id => id == AttributeValueId),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_PassCorrectParametersToAlreadyAssignedValidator()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateAttributeValueIsNotAlreadyAssignedToSku(
                    Arg.Is<long>(skuId => skuId == SkuId),
                    Arg.Is<long>(attrValueId => attrValueId == AttributeValueId),
                    Arg.Any<CancellationToken>());
        }

        #endregion
    }
}

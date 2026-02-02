using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.ProductAttributeValues.Common.Services;
using Application.Products.ProductAttributeValues.Remove;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Products.ProductAttributeValues.Remove
{
    public class RemoveAttributeValueFromSkuCommandTests
    {
        private const long ProductSkuId = 1;
        private const long AttributeValueId = 2;

        private static readonly RemoveAttributeValueFromSkuCommand _command = new(
            ProductSkuId: ProductSkuId,
            AttributeValueId: AttributeValueId);

        private readonly RemoveAttributeValueFromSkuCommandHandler _handler;
        private readonly IProductAttributeValueRepository _productAttributeValueRepositoryMock;
        private readonly IProductAttributeValueValidator _validatorMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public RemoveAttributeValueFromSkuCommandTests()
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
                .ValidateCanRemoveAttributeValueFromSku(
                    _command.ProductSkuId,
                    _command.AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());
        }

        private void SetupValidationFailure(Error error)
        {
            _validatorMock
                .ValidateCanRemoveAttributeValueFromSku(
                    _command.ProductSkuId,
                    _command.AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        #endregion

        #region Success Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenRemovalIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_CallRemoveAttributeValueFromSku_WhenRemovalIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productAttributeValueRepositoryMock
                .Received(1)
                .RemoveAttributeValueFromSku(
                    ProductSkuId,
                    AttributeValueId,
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallRemoveAttributeValueFromSku_WithCorrectParameters()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productAttributeValueRepositoryMock
                .Received(1)
                .RemoveAttributeValueFromSku(
                    Arg.Is<long>(id => id == ProductSkuId),
                    Arg.Is<long>(id => id == AttributeValueId),
                    Arg.Any<CancellationToken>());
        }

        #endregion

        #region Validation Failure Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenAttributeValueNotAssignedToSku()
        {
            // Arrange
            SetupValidationFailure(ProductAttributeValueErrors.NotAssignedToSku);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductAttributeValueErrors.NotAssignedToSku);
        }

        [Fact]
        public async Task Handle_Should_NotCallRemoveAttributeValueFromSku_WhenValidationFails()
        {
            // Arrange
            SetupValidationFailure(ProductAttributeValueErrors.ProductSkuNotActive);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productAttributeValueRepositoryMock
                .DidNotReceive()
                .RemoveAttributeValueFromSku(
                    Arg.Any<long>(),
                    Arg.Any<long>(),
                    Arg.Any<CancellationToken>());
        }

        #endregion

        #region Validation Scenarios

        [Fact]
        public async Task Handle_Should_CallValidateCanRemoveAttributeValueFromSku_Always()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateCanRemoveAttributeValueFromSku(
                    ProductSkuId,
                    AttributeValueId,
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallValidateCanRemoveAttributeValueFromSku_WithCorrectProductSkuId()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateCanRemoveAttributeValueFromSku(
                    Arg.Is<long>(id => id == ProductSkuId),
                    Arg.Any<long>(),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallValidateCanRemoveAttributeValueFromSku_WithCorrectAttributeValueId()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateCanRemoveAttributeValueFromSku(
                    Arg.Any<long>(),
                    Arg.Is<long>(id => id == AttributeValueId),
                    Arg.Any<CancellationToken>());
        }

        #endregion

        #region Repository Interactions

        [Fact]
        public async Task Handle_Should_CallRemoveAttributeValueFromSku_ExactlyOnce_WhenRemovalIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productAttributeValueRepositoryMock
                .Received(1)
                .RemoveAttributeValueFromSku(
                    Arg.Any<long>(),
                    Arg.Any<long>(),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallRemoveAttributeValueFromSku_AfterValidation_WhenRemovalIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(async () =>
            {
                await _validatorMock.ValidateCanRemoveAttributeValueFromSku(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<CancellationToken>());
                await _productAttributeValueRepositoryMock.RemoveAttributeValueFromSku(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<CancellationToken>());
            });
        }

        #endregion

        #region Edge Cases

        [Fact]
        public async Task Handle_Should_RemoveAssignment_WithDifferentProductSkuIds()
        {
            // Arrange
            long differentProductSkuId = 999;
            var commandWithDifferentSku = _command with { ProductSkuId = differentProductSkuId };

            _validatorMock
                .ValidateCanRemoveAttributeValueFromSku(
                    differentProductSkuId,
                    commandWithDifferentSku.AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            Result result = await _handler.Handle(commandWithDifferentSku, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            await _productAttributeValueRepositoryMock
                .Received(1)
                .RemoveAttributeValueFromSku(
                    differentProductSkuId,
                    Arg.Any<long>(),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_RemoveAssignment_WithDifferentAttributeValueIds()
        {
            // Arrange
            long differentAttributeValueId = 888;
            var commandWithDifferentAttributeValue = _command with { AttributeValueId = differentAttributeValueId };

            _validatorMock
                .ValidateCanRemoveAttributeValueFromSku(
                    commandWithDifferentAttributeValue.ProductSkuId,
                    differentAttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            Result result = await _handler.Handle(commandWithDifferentAttributeValue, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            await _productAttributeValueRepositoryMock
                .Received(1)
                .RemoveAttributeValueFromSku(
                    Arg.Any<long>(),
                    differentAttributeValueId,
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_ExecuteInCorrectOrder_WhenRemovalIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(async () =>
            {
                await _validatorMock.ValidateCanRemoveAttributeValueFromSku(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<CancellationToken>());
                await _productAttributeValueRepositoryMock.RemoveAttributeValueFromSku(Arg.Any<long>(), Arg.Any<long>(), Arg.Any<CancellationToken>());
            });
        }

        [Fact]
        public async Task Handle_Should_PassBothParametersCorrectly_ToValidator()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateCanRemoveAttributeValueFromSku(
                    Arg.Is<long>(skuId => skuId == ProductSkuId),
                    Arg.Is<long>(attrValueId => attrValueId == AttributeValueId),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_PassBothParametersCorrectly_ToRepository()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productAttributeValueRepositoryMock
                .Received(1)
                .RemoveAttributeValueFromSku(
                    Arg.Is<long>(skuId => skuId == ProductSkuId),
                    Arg.Is<long>(attrValueId => attrValueId == AttributeValueId),
                    Arg.Any<CancellationToken>());
        }

        #endregion
    }
}

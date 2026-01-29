using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.AttributeValues.Common.Services;
using Application.Products.AttributeValues.Deactivate;
using Domain.Entities.Products;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Products.AttributeValues.Deactivate
{
    public class DeactivateAttributeValueCommandTests
    {
        private const long AttributeValueId = 1;
        private const long AttributeId = 10;

        private static readonly DeactivateAttributeValueCommand _command = new(AttributeValueId);

        private readonly DeactivateAttributeValueCommandHandler _handler;
        private readonly IAttributeValueRepository _attributeValueRepositoryMock;
        private readonly IAttributeValueValidator _validatorMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public DeactivateAttributeValueCommandTests()
        {
            _attributeValueRepositoryMock = Substitute.For<IAttributeValueRepository>();
            _validatorMock = Substitute.For<IAttributeValueValidator>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _handler = new(
                _attributeValueRepositoryMock,
                _validatorMock,
                _unitOfWorkMock);
        }

        #region Helper Methods

        private AttributeValue CreateActiveAttributeValue()
        {
            return new AttributeValue
            {
                Id = AttributeValueId,
                AttributeId = AttributeId,
                Value = "Red",
                IsActive = true
            };
        }

        private AttributeValue CreateInactiveAttributeValue()
        {
            return new AttributeValue
            {
                Id = AttributeValueId,
                AttributeId = AttributeId,
                Value = "Blue",
                IsActive = false
            };
        }

        private void SetupAttributeValueFound(AttributeValue attributeValue)
        {
            _attributeValueRepositoryMock
                .GetByIdAsync(AttributeValueId, Arg.Any<CancellationToken>())
                .Returns(attributeValue);
        }

        private void SetupSkuValidationSuccess()
        {
            _validatorMock
                .ValidateSkuIsDisabledBeforeDeactivation(
                    AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());
        }

        private void SetupSkuValidationFailure(Error error)
        {
            _validatorMock
                .ValidateSkuIsDisabledBeforeDeactivation(
                    AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        #endregion

        #region SKU Validation Failure Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenAttributeValueHasActiveSkus()
        {
            // Arrange
            SetupSkuValidationFailure(AttributeValueErrors.AttributeValueHasActiveSkus);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AttributeValueErrors.AttributeValueHasActiveSkus);
        }

        [Fact]
        public async Task Handle_Should_NotCallGetByIdAsync_WhenAttributeValueHasActiveSkus()
        {
            // Arrange
            SetupSkuValidationFailure(AttributeValueErrors.AttributeValueHasActiveSkus);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _attributeValueRepositoryMock
                .DidNotReceive()
                .GetByIdAsync(Arg.Any<long>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenAttributeValueHasActiveSkus()
        {
            // Arrange
            SetupSkuValidationFailure(AttributeValueErrors.AttributeValueHasActiveSkus);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _attributeValueRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<AttributeValue>());
        }

        [Fact]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenAttributeValueHasActiveSkus()
        {
            // Arrange
            SetupSkuValidationFailure(AttributeValueErrors.AttributeValueHasActiveSkus);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region Active AttributeValue Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenAttributeValueIsActive()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            SetupSkuValidationSuccess();
            SetupAttributeValueFound(attributeValue);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_DeactivateAttributeValue_WhenAttributeValueIsActive()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            SetupSkuValidationSuccess();
            SetupAttributeValueFound(attributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            attributeValue.IsActive.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_CallUpdate_WhenAttributeValueIsActive()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            SetupSkuValidationSuccess();
            SetupAttributeValueFound(attributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _attributeValueRepositoryMock
                .Received(1)
                .Update(Arg.Any<AttributeValue>());
        }

        [Fact]
        public async Task Handle_Should_CallUpdateWithCorrectAttributeValue_WhenAttributeValueIsActive()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            SetupSkuValidationSuccess();
            SetupAttributeValueFound(attributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _attributeValueRepositoryMock
                .Received(1)
                .Update(attributeValue);
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_WhenAttributeValueIsActive()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            SetupSkuValidationSuccess();
            SetupAttributeValueFound(attributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_AfterUpdate_WhenAttributeValueIsActive()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            SetupSkuValidationSuccess();
            SetupAttributeValueFound(attributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(() =>
            {
                _attributeValueRepositoryMock.Update(Arg.Any<AttributeValue>());
                _unitOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>());
            });
        }

        #endregion

        #region Already Inactive AttributeValue Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenAttributeValueIsAlreadyInactive()
        {
            // Arrange
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            SetupSkuValidationSuccess();
            SetupAttributeValueFound(attributeValue);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_NotChangeIsActive_WhenAttributeValueIsAlreadyInactive()
        {
            // Arrange
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            SetupSkuValidationSuccess();
            SetupAttributeValueFound(attributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            attributeValue.IsActive.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenAttributeValueIsAlreadyInactive()
        {
            // Arrange
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            SetupSkuValidationSuccess();
            SetupAttributeValueFound(attributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _attributeValueRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<AttributeValue>());
        }

        [Fact]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenAttributeValueIsAlreadyInactive()
        {
            // Arrange
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            SetupSkuValidationSuccess();
            SetupAttributeValueFound(attributeValue);

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
        public async Task Handle_Should_CallValidateSkuIsDisabledBeforeDeactivation_Always()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            SetupSkuValidationSuccess();
            SetupAttributeValueFound(attributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateSkuIsDisabledBeforeDeactivation(
                    AttributeValueId,
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallValidateSkuIsDisabledBeforeDeactivation_WithCorrectAttributeValueId()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            SetupSkuValidationSuccess();
            SetupAttributeValueFound(attributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateSkuIsDisabledBeforeDeactivation(
                    Arg.Is<long>(id => id == AttributeValueId),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallGetByIdAsync_AfterSuccessfulSkuValidation()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            SetupSkuValidationSuccess();
            SetupAttributeValueFound(attributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(async () =>
            {
                await _validatorMock.ValidateSkuIsDisabledBeforeDeactivation(Arg.Any<long>(), Arg.Any<CancellationToken>());
                await _attributeValueRepositoryMock.GetByIdAsync(Arg.Any<long>(), Arg.Any<CancellationToken>());
            });
        }

        #endregion

        #region Repository Interaction Scenarios

        [Fact]
        public async Task Handle_Should_CallGetByIdAsync_WhenSkuValidationPasses()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            SetupSkuValidationSuccess();
            SetupAttributeValueFound(attributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _attributeValueRepositoryMock
                .Received(1)
                .GetByIdAsync(AttributeValueId, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallGetByIdAsync_WithCorrectAttributeValueId()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            SetupSkuValidationSuccess();
            SetupAttributeValueFound(attributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _attributeValueRepositoryMock
                .Received(1)
                .GetByIdAsync(
                    Arg.Is<long>(id => id == AttributeValueId),
                    Arg.Any<CancellationToken>());
        }

        #endregion

        #region Edge Cases

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenDifferentAttributeValueIdHasActiveSkus()
        {
            // Arrange
            long differentAttributeValueId = 9999;
            var command = new DeactivateAttributeValueCommand(differentAttributeValueId);

            _validatorMock
                .ValidateSkuIsDisabledBeforeDeactivation(
                    differentAttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(AttributeValueErrors.AttributeValueHasActiveSkus));

            // Act
            Result result = await _handler.Handle(command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AttributeValueErrors.AttributeValueHasActiveSkus);
        }

        [Fact]
        public async Task Handle_Should_PreserveAttributeValueId_AfterDeactivation()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            SetupSkuValidationSuccess();
            SetupAttributeValueFound(attributeValue);
            long originalId = attributeValue.Id;

            // Act
            await _handler.Handle(_command, default);

            // Assert
            attributeValue.Id.Should().Be(originalId);
        }

        [Fact]
        public async Task Handle_Should_PreserveAttributeId_AfterDeactivation()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            SetupSkuValidationSuccess();
            SetupAttributeValueFound(attributeValue);
            long originalAttributeId = attributeValue.AttributeId;

            // Act
            await _handler.Handle(_command, default);

            // Assert
            attributeValue.AttributeId.Should().Be(originalAttributeId);
        }

        [Fact]
        public async Task Handle_Should_OnlyModifyIsActiveProperty_WhenDeactivatingAttributeValue()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            string originalValue = attributeValue.Value!;
            long originalAttributeId = attributeValue.AttributeId;
            
            SetupSkuValidationSuccess();
            SetupAttributeValueFound(attributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            attributeValue.IsActive.Should().BeFalse();
            attributeValue.Value.Should().Be(originalValue);
            attributeValue.AttributeId.Should().Be(originalAttributeId);
        }

        [Fact]
        public async Task Handle_Should_CallDeactivate_OnAttributeValueEntity()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            SetupSkuValidationSuccess();
            SetupAttributeValueFound(attributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            attributeValue.IsActive.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_ExecuteInCorrectOrder_WhenDeactivating()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            SetupSkuValidationSuccess();
            SetupAttributeValueFound(attributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(async () =>
            {
                await _validatorMock.ValidateSkuIsDisabledBeforeDeactivation(Arg.Any<long>(), Arg.Any<CancellationToken>());
                await _attributeValueRepositoryMock.GetByIdAsync(Arg.Any<long>(), Arg.Any<CancellationToken>());
                _attributeValueRepositoryMock.Update(Arg.Any<AttributeValue>());
                await _unitOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>());
            });
        }

        #endregion
    }
}

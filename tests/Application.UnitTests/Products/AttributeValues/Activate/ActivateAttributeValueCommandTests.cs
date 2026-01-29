using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.AttributeValues.Activate;
using Application.Products.AttributeValues.Common.Services;
using Domain.Entities.Products;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Products.AttributeValues.Activate
{
    public class ActivateAttributeValueCommandTests
    {
        private const long AttributeValueId = 1;
        private const long AttributeId = 10;

        private static readonly ActivateAttributeValueCommand _command = new(AttributeValueId);

        private readonly ActivateAttributeValueCommandHandler _handler;
        private readonly IAttributeValueRepository _attributeValueRepositoryMock;
        private readonly IAttributeValueValidator _validatorMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public ActivateAttributeValueCommandTests()
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

        private AttributeValue CreateInactiveAttributeValue()
        {
            return new AttributeValue
            {
                Id = AttributeValueId,
                AttributeId = AttributeId,
                Value = "Red",
                IsActive = false
            };
        }

        private AttributeValue CreateActiveAttributeValue()
        {
            return new AttributeValue
            {
                Id = AttributeValueId,
                AttributeId = AttributeId,
                Value = "Blue",
                IsActive = true
            };
        }

        private void SetupAttributeValueFound(AttributeValue attributeValue)
        {
            _attributeValueRepositoryMock
                .GetByIdAsync(AttributeValueId, Arg.Any<CancellationToken>())
                .Returns(attributeValue);
        }

        private void SetupAttributeValueNotFound()
        {
            _attributeValueRepositoryMock
                .GetByIdAsync(AttributeValueId, Arg.Any<CancellationToken>())
                .ReturnsNull();
        }

        private void SetupAttributeActiveValidation()
        {
            _validatorMock
                .ValidateAttributeIsActiveAsync(
                    AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());
        }

        private void SetupAttributeNotActiveValidationFailure(Error error)
        {
            _validatorMock
                .ValidateAttributeIsActiveAsync(
                    AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        #endregion

        #region AttributeValue Not Found Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenAttributeValueDoesNotExist()
        {
            // Arrange
            SetupAttributeValueNotFound();

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AttributeValueErrors.NotFound(AttributeValueId));
        }

        [Fact]
        public async Task Handle_Should_NotCallValidateAttributeIsActiveAsync_WhenAttributeValueDoesNotExist()
        {
            // Arrange
            SetupAttributeValueNotFound();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .DidNotReceive()
                .ValidateAttributeIsActiveAsync(
                    Arg.Any<long>(),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenAttributeValueDoesNotExist()
        {
            // Arrange
            SetupAttributeValueNotFound();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _attributeValueRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<AttributeValue>());
        }

        [Fact]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenAttributeValueDoesNotExist()
        {
            // Arrange
            SetupAttributeValueNotFound();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region Attribute Not Active Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenAttributeIsNotActive()
        {
            // Arrange
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeNotActiveValidationFailure(AttributeValueErrors.AttributeNotActive);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AttributeValueErrors.AttributeNotActive);
        }

        [Fact]
        public async Task Handle_Should_NotActivateAttributeValue_WhenAttributeIsNotActive()
        {
            // Arrange
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeNotActiveValidationFailure(AttributeValueErrors.AttributeNotActive);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            attributeValue.IsActive.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenAttributeIsNotActive()
        {
            // Arrange
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeNotActiveValidationFailure(AttributeValueErrors.AttributeNotActive);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _attributeValueRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<AttributeValue>());
        }

        [Fact]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenAttributeIsNotActive()
        {
            // Arrange
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeNotActiveValidationFailure(AttributeValueErrors.AttributeNotActive);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region Inactive AttributeValue Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenAttributeValueIsInactive()
        {
            // Arrange
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeActiveValidation();

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_ActivateAttributeValue_WhenAttributeValueIsInactive()
        {
            // Arrange
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeActiveValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            attributeValue.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_CallUpdate_WhenAttributeValueIsInactive()
        {
            // Arrange
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeActiveValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _attributeValueRepositoryMock
                .Received(1)
                .Update(Arg.Any<AttributeValue>());
        }

        [Fact]
        public async Task Handle_Should_CallUpdateWithCorrectAttributeValue_WhenAttributeValueIsInactive()
        {
            // Arrange
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeActiveValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _attributeValueRepositoryMock
                .Received(1)
                .Update(attributeValue);
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_WhenAttributeValueIsInactive()
        {
            // Arrange
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeActiveValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_AfterUpdate_WhenAttributeValueIsInactive()
        {
            // Arrange
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeActiveValidation();

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

        #region Already Active AttributeValue Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenAttributeValueIsAlreadyActive()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeActiveValidation();

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_NotChangeIsActive_WhenAttributeValueIsAlreadyActive()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeActiveValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            attributeValue.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenAttributeValueIsAlreadyActive()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeActiveValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _attributeValueRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<AttributeValue>());
        }

        [Fact]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenAttributeValueIsAlreadyActive()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeActiveValidation();

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
        public async Task Handle_Should_CallValidateAttributeIsActiveAsync_WhenAttributeValueExists()
        {
            // Arrange
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeActiveValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateAttributeIsActiveAsync(
                    AttributeId,
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallValidateAttributeIsActiveAsync_WithCorrectAttributeId()
        {
            // Arrange
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeActiveValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateAttributeIsActiveAsync(
                    Arg.Is<long>(id => id == attributeValue.AttributeId),
                    Arg.Any<CancellationToken>());
        }

        #endregion

        #region Repository Interaction Scenarios

        [Fact]
        public async Task Handle_Should_CallGetByIdAsync_Always()
        {
            // Arrange
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeActiveValidation();

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
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeActiveValidation();

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
        public async Task Handle_Should_ReturnFailure_WhenDifferentAttributeValueIdDoesNotExist()
        {
            // Arrange
            long nonExistentAttributeValueId = 9999;
            var command = new ActivateAttributeValueCommand(nonExistentAttributeValueId);

            _attributeValueRepositoryMock
                .GetByIdAsync(nonExistentAttributeValueId, Arg.Any<CancellationToken>())
                .ReturnsNull();

            // Act
            Result result = await _handler.Handle(command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AttributeValueErrors.NotFound(nonExistentAttributeValueId));
        }

        [Fact]
        public async Task Handle_Should_PreserveAttributeValueId_AfterActivation()
        {
            // Arrange
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeActiveValidation();
            long originalId = attributeValue.Id;

            // Act
            await _handler.Handle(_command, default);

            // Assert
            attributeValue.Id.Should().Be(originalId);
        }

        [Fact]
        public async Task Handle_Should_PreserveAttributeId_AfterActivation()
        {
            // Arrange
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeActiveValidation();
            long originalAttributeId = attributeValue.AttributeId;

            // Act
            await _handler.Handle(_command, default);

            // Assert
            attributeValue.AttributeId.Should().Be(originalAttributeId);
        }

        [Fact]
        public async Task Handle_Should_OnlyModifyIsActiveProperty_WhenActivatingAttributeValue()
        {
            // Arrange
            AttributeValue attributeValue = CreateInactiveAttributeValue();
            string originalValue = attributeValue.Value!;
            long originalAttributeId = attributeValue.AttributeId;

            SetupAttributeValueFound(attributeValue);
            SetupAttributeActiveValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            attributeValue.IsActive.Should().BeTrue();
            attributeValue.Value.Should().Be(originalValue);
            attributeValue.AttributeId.Should().Be(originalAttributeId);
        }

        [Fact]
        public async Task Handle_Should_ValidateAttributeIsActive_BeforeCheckingIfAlreadyActive()
        {
            // Arrange
            AttributeValue attributeValue = CreateActiveAttributeValue();
            SetupAttributeValueFound(attributeValue);
            SetupAttributeActiveValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateAttributeIsActiveAsync(
                    Arg.Any<long>(),
                    Arg.Any<CancellationToken>());
        }

        #endregion
    }
}

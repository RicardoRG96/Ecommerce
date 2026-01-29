using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.AttributeValues.Common.Services;
using Application.Products.AttributeValues.Update;
using Domain.Entities.Products;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Products.AttributeValues.Update
{
    public class UpdateAttributeValueCommandTests
    {
        private const long AttributeValueId = 1;
        private const long AttributeId = 10;
        private const long OriginalAttributeId = 5;
        private const string AttributeValueValue = "Blue";
        private const string OriginalAttributeValueValue = "Red";
        private const string OriginalNormalizedValue = "red";
        private const string ExpectedNewNormalizedValue = "blue";
        private const decimal NumericValue = 200.75m;
        private const decimal OriginalNumericValue = 100.50m;
        private const bool BooleanValue = false;
        private const bool OriginalBooleanValue = true;
        private const int DisplayOrder = 10;
        private const int OriginalDisplayOrder = 1;

        private static readonly UpdateAttributeValueCommand _command = new(
            Id: AttributeValueId,
            AttributeId: AttributeId,
            Value: AttributeValueValue,
            NumericValue: NumericValue,
            BooleanValue: BooleanValue,
            DisplayOrder: DisplayOrder);

        private readonly UpdateAttributeValueCommandHandler _handler;
        private readonly IAttributeValueRepository _attributeValueRepositoryMock;
        private readonly IAttributeValueValidator _validatorMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public UpdateAttributeValueCommandTests()
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

        private AttributeValue CreateExistingAttributeValue()
        {
            return new AttributeValue
            {
                Id = AttributeValueId,
                AttributeId = OriginalAttributeId,
                Value = OriginalAttributeValueValue,
                NormalizedValue = OriginalNormalizedValue,
                NumericValue = OriginalNumericValue,
                BooleanValue = OriginalBooleanValue,
                DisplayOrder = OriginalDisplayOrder,
                IsActive = true
            };
        }

        private void SetupSuccessfulScenario(AttributeValue attributeValue)
        {
            _attributeValueRepositoryMock
                .GetByIdAsync(AttributeValueId, Arg.Any<CancellationToken>())
                .Returns(attributeValue);

            _validatorMock
                .ValidateAttributeIsActiveAsync(
                    _command.AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateAttributeValueIsUniqueAsync(
                    _command.AttributeId,
                    _command.Value,
                    AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());
        }

        private void SetupAttributeValueNotFound()
        {
            _attributeValueRepositoryMock
                .GetByIdAsync(AttributeValueId, Arg.Any<CancellationToken>())
                .Returns((AttributeValue?)null);
        }

        private void SetupAttributeNotActiveFailure(AttributeValue attributeValue, Error error)
        {
            _attributeValueRepositoryMock
                .GetByIdAsync(AttributeValueId, Arg.Any<CancellationToken>())
                .Returns(attributeValue);

            _validatorMock
                .ValidateAttributeIsActiveAsync(
                    _command.AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        private void SetupValueNotUniqueFailure(AttributeValue attributeValue, Error error)
        {
            _attributeValueRepositoryMock
                .GetByIdAsync(AttributeValueId, Arg.Any<CancellationToken>())
                .Returns(attributeValue);

            _validatorMock
                .ValidateAttributeIsActiveAsync(
                    _command.AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateAttributeValueIsUniqueAsync(
                    _command.AttributeId,
                    _command.Value,
                    AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        #endregion

        #region Success Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenAttributeValueExistsAndAllValidationsPass()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupSuccessfulScenario(existingAttributeValue);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_UpdateBasicProperties_WhenAttributeValueIsValid()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupSuccessfulScenario(existingAttributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingAttributeValue.AttributeId.Should().Be(_command.AttributeId);
            existingAttributeValue.Value.Should().Be(_command.Value);
            existingAttributeValue.DisplayOrder.Should().Be(_command.DisplayOrder);
        }

        [Fact]
        public async Task Handle_Should_UpdateNormalizedValue_WhenValueChanges()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupSuccessfulScenario(existingAttributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingAttributeValue.NormalizedValue.Should().Be(ExpectedNewNormalizedValue);
        }

        [Fact]
        public async Task Handle_Should_UpdateNumericValue_WhenAttributeValueIsValid()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupSuccessfulScenario(existingAttributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingAttributeValue.NumericValue.Should().Be(NumericValue);
        }

        [Fact]
        public async Task Handle_Should_UpdateBooleanValue_WhenAttributeValueIsValid()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupSuccessfulScenario(existingAttributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingAttributeValue.BooleanValue.Should().Be(BooleanValue);
        }

        [Fact]
        public async Task Handle_Should_UpdateNumericValueToNull_WhenNotProvided()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            var commandWithoutNumericValue = _command with { NumericValue = null };

            _attributeValueRepositoryMock
                .GetByIdAsync(AttributeValueId, Arg.Any<CancellationToken>())
                .Returns(existingAttributeValue);

            _validatorMock
                .ValidateAttributeIsActiveAsync(
                    commandWithoutNumericValue.AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateAttributeValueIsUniqueAsync(
                    commandWithoutNumericValue.AttributeId,
                    commandWithoutNumericValue.Value,
                    AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithoutNumericValue, default);

            // Assert
            existingAttributeValue.NumericValue.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_UpdateBooleanValueToNull_WhenNotProvided()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            var commandWithoutBooleanValue = _command with { BooleanValue = null };

            _attributeValueRepositoryMock
                .GetByIdAsync(AttributeValueId, Arg.Any<CancellationToken>())
                .Returns(existingAttributeValue);

            _validatorMock
                .ValidateAttributeIsActiveAsync(
                    commandWithoutBooleanValue.AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateAttributeValueIsUniqueAsync(
                    commandWithoutBooleanValue.AttributeId,
                    commandWithoutBooleanValue.Value,
                    AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithoutBooleanValue, default);

            // Assert
            existingAttributeValue.BooleanValue.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_AllowSameValue_WhenUpdatingSameAttributeValue()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            existingAttributeValue.Value = _command.Value;
            existingAttributeValue.AttributeId = _command.AttributeId;

            SetupSuccessfulScenario(existingAttributeValue);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_UpdateAttributeId_WhenChanged()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupSuccessfulScenario(existingAttributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingAttributeValue.AttributeId.Should().Be(AttributeId);
            existingAttributeValue.AttributeId.Should().NotBe(OriginalAttributeId);
        }

        [Fact]
        public async Task Handle_Should_UpdateToZeroDisplayOrder_WhenSpecified()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            var commandWithZeroDisplayOrder = _command with { DisplayOrder = 0 };

            _attributeValueRepositoryMock
                .GetByIdAsync(AttributeValueId, Arg.Any<CancellationToken>())
                .Returns(existingAttributeValue);

            _validatorMock
                .ValidateAttributeIsActiveAsync(
                    commandWithZeroDisplayOrder.AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateAttributeValueIsUniqueAsync(
                    commandWithZeroDisplayOrder.AttributeId,
                    commandWithZeroDisplayOrder.Value,
                    AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithZeroDisplayOrder, default);

            // Assert
            existingAttributeValue.DisplayOrder.Should().Be(0);
        }

        [Fact]
        public async Task Handle_Should_UpdateBooleanToFalse_WhenSpecified()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            existingAttributeValue.BooleanValue = true;

            var commandWithFalseBoolean = _command with { BooleanValue = false };

            _attributeValueRepositoryMock
                .GetByIdAsync(AttributeValueId, Arg.Any<CancellationToken>())
                .Returns(existingAttributeValue);

            _validatorMock
                .ValidateAttributeIsActiveAsync(
                    commandWithFalseBoolean.AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateAttributeValueIsUniqueAsync(
                    commandWithFalseBoolean.AttributeId,
                    commandWithFalseBoolean.Value,
                    AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithFalseBoolean, default);

            // Assert
            existingAttributeValue.BooleanValue.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_UpdateBooleanToTrue_WhenSpecified()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            existingAttributeValue.BooleanValue = false;

            var commandWithTrueBoolean = _command with { BooleanValue = true };

            _attributeValueRepositoryMock
                .GetByIdAsync(AttributeValueId, Arg.Any<CancellationToken>())
                .Returns(existingAttributeValue);

            _validatorMock
                .ValidateAttributeIsActiveAsync(
                    commandWithTrueBoolean.AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateAttributeValueIsUniqueAsync(
                    commandWithTrueBoolean.AttributeId,
                    commandWithTrueBoolean.Value,
                    AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithTrueBoolean, default);

            // Assert
            existingAttributeValue.BooleanValue.Should().BeTrue();
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
        public async Task Handle_Should_NotCallValidateAttributeValueIsUniqueAsync_WhenAttributeValueDoesNotExist()
        {
            // Arrange
            SetupAttributeValueNotFound();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .DidNotReceive()
                .ValidateAttributeValueIsUniqueAsync(
                    Arg.Any<long>(),
                    Arg.Any<string>(),
                    Arg.Any<long?>(),
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

        #region Attribute Not Active Failure Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenAttributeIsNotActive()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupAttributeNotActiveFailure(existingAttributeValue, AttributeValueErrors.AttributeNotActive);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AttributeValueErrors.AttributeNotActive);
        }

        [Fact]
        public async Task Handle_Should_NotCallValidateAttributeValueIsUniqueAsync_WhenAttributeIsNotActive()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupAttributeNotActiveFailure(existingAttributeValue, AttributeValueErrors.AttributeNotActive);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .DidNotReceive()
                .ValidateAttributeValueIsUniqueAsync(
                    Arg.Any<long>(),
                    Arg.Any<string>(),
                    Arg.Any<long?>(),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenAttributeIsNotActive()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupAttributeNotActiveFailure(existingAttributeValue, AttributeValueErrors.AttributeNotActive);

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
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupAttributeNotActiveFailure(existingAttributeValue, AttributeValueErrors.AttributeNotActive);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region Value Not Unique Failure Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenAttributeValueAlreadyExists()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupValueNotUniqueFailure(existingAttributeValue, AttributeValueErrors.Duplicated);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AttributeValueErrors.Duplicated);
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenAttributeValueAlreadyExists()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupValueNotUniqueFailure(existingAttributeValue, AttributeValueErrors.Duplicated);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _attributeValueRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<AttributeValue>());
        }

        [Fact]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenAttributeValueAlreadyExists()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupValueNotUniqueFailure(existingAttributeValue, AttributeValueErrors.Duplicated);

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
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupSuccessfulScenario(existingAttributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _attributeValueRepositoryMock
                .Received(1)
                .GetByIdAsync(AttributeValueId, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallValidateAttributeIsActiveAsync_WhenAttributeValueExists()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupSuccessfulScenario(existingAttributeValue);

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
        public async Task Handle_Should_CallValidateAttributeValueIsUniqueAsync_WhenAttributeIsActive()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupSuccessfulScenario(existingAttributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateAttributeValueIsUniqueAsync(
                    AttributeId,
                    AttributeValueValue,
                    AttributeValueId,
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_PassCorrectAttributeValueIdToValidator_WhenValidating()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupSuccessfulScenario(existingAttributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateAttributeValueIsUniqueAsync(
                    Arg.Any<long>(),
                    Arg.Any<string>(),
                    AttributeValueId,
                    Arg.Any<CancellationToken>());
        }

        #endregion

        #region Repository and UnitOfWork Interactions

        [Fact]
        public async Task Handle_Should_CallUpdate_ExactlyOnce_WhenAttributeValueIsValid()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupSuccessfulScenario(existingAttributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _attributeValueRepositoryMock
                .Received(1)
                .Update(Arg.Any<AttributeValue>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_ExactlyOnce_WhenAttributeValueIsValid()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupSuccessfulScenario(existingAttributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_AfterUpdate_WhenAttributeValueIsValid()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupSuccessfulScenario(existingAttributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(() =>
            {
                _attributeValueRepositoryMock.Update(Arg.Any<AttributeValue>());
                _unitOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>());
            });
        }

        [Fact]
        public async Task Handle_Should_UpdateCorrectAttributeValueInstance_WhenAttributeValueIsValid()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupSuccessfulScenario(existingAttributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _attributeValueRepositoryMock
                .Received(1)
                .Update(existingAttributeValue);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public async Task Handle_Should_UpdateDisplayOrder_ToLargeValue()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            var commandWithLargeDisplayOrder = _command with { DisplayOrder = 999 };

            _attributeValueRepositoryMock
                .GetByIdAsync(AttributeValueId, Arg.Any<CancellationToken>())
                .Returns(existingAttributeValue);

            _validatorMock
                .ValidateAttributeIsActiveAsync(
                    commandWithLargeDisplayOrder.AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateAttributeValueIsUniqueAsync(
                    commandWithLargeDisplayOrder.AttributeId,
                    commandWithLargeDisplayOrder.Value,
                    AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithLargeDisplayOrder, default);

            // Assert
            existingAttributeValue.DisplayOrder.Should().Be(999);
        }

        [Fact]
        public async Task Handle_Should_PreserveAttributeValueId_AfterUpdate()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupSuccessfulScenario(existingAttributeValue);
            long originalId = existingAttributeValue.Id;

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingAttributeValue.Id.Should().Be(originalId);
        }

        [Fact]
        public async Task Handle_Should_UpdateAllPropertiesAtOnce_WhenAllAreChanged()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            SetupSuccessfulScenario(existingAttributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingAttributeValue.AttributeId.Should().Be(_command.AttributeId);
            existingAttributeValue.Value.Should().Be(_command.Value);
            existingAttributeValue.NormalizedValue.Should().Be(ExpectedNewNormalizedValue);
            existingAttributeValue.NumericValue.Should().Be(_command.NumericValue);
            existingAttributeValue.BooleanValue.Should().Be(_command.BooleanValue);
            existingAttributeValue.DisplayOrder.Should().Be(_command.DisplayOrder);
        }

        [Fact]
        public async Task Handle_Should_UpdateNegativeNumericValue_WhenSpecified()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            var commandWithNegativeValue = _command with { NumericValue = -50.25m };

            _attributeValueRepositoryMock
                .GetByIdAsync(AttributeValueId, Arg.Any<CancellationToken>())
                .Returns(existingAttributeValue);

            _validatorMock
                .ValidateAttributeIsActiveAsync(
                    commandWithNegativeValue.AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateAttributeValueIsUniqueAsync(
                    commandWithNegativeValue.AttributeId,
                    commandWithNegativeValue.Value,
                    AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithNegativeValue, default);

            // Assert
            existingAttributeValue.NumericValue.Should().Be(-50.25m);
        }

        [Fact]
        public async Task Handle_Should_NormalizeValueToLowerCase_WhenUpdating()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            var commandWithUpperCase = _command with { Value = "GREEN" };

            _attributeValueRepositoryMock
                .GetByIdAsync(AttributeValueId, Arg.Any<CancellationToken>())
                .Returns(existingAttributeValue);

            _validatorMock
                .ValidateAttributeIsActiveAsync(
                    commandWithUpperCase.AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateAttributeValueIsUniqueAsync(
                    commandWithUpperCase.AttributeId,
                    commandWithUpperCase.Value,
                    AttributeValueId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithUpperCase, default);

            // Assert
            existingAttributeValue.Value.Should().Be("GREEN");
            existingAttributeValue.NormalizedValue.Should().Be("green");
        }

        [Fact]
        public async Task Handle_Should_NotChangeIsActiveProperty_WhenUpdating()
        {
            // Arrange
            AttributeValue existingAttributeValue = CreateExistingAttributeValue();
            existingAttributeValue.IsActive = true;
            SetupSuccessfulScenario(existingAttributeValue);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingAttributeValue.IsActive.Should().BeTrue();
        }

        #endregion
    }
}

using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.AttributeValues.Common.Services;
using Application.Products.AttributeValues.Create;
using Domain.Entities.Products;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Products.AttributeValues.Create
{
    public class CreateAttributeValueCommandTests
    {
        private const long AttributeId = 1;
        private const string AttributeValueValue = "Red";
        private const decimal NumericValue = 100.50m;
        private const bool BooleanValue = true;
        private const int DisplayOrder = 1;
        private const string ExpectedNormalizedValue = "red";

        private static readonly CreateAttributeValueCommand _command = new(
            AttributeId: AttributeId,
            Value: AttributeValueValue,
            NumericValue: NumericValue,
            BooleanValue: BooleanValue,
            DisplayOrder: DisplayOrder,
            IsActive: true);

        private readonly CreateAttributeValueCommandHandler _handler;
        private readonly IAttributeValueRepository _attributeValueRepositoryMock;
        private readonly IAttributeValueValidator _validatorMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public CreateAttributeValueCommandTests()
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

        private void SetupSuccessfulValidation()
        {
            _validatorMock
                .ValidateAttributeIsActiveAsync(
                    _command.AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateAttributeValueIsUniqueAsync(
                    _command.AttributeId,
                    _command.Value,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());
        }

        private void SetupAttributeNotActiveFailure(Error error)
        {
            _validatorMock
                .ValidateAttributeIsActiveAsync(
                    _command.AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        private void SetupValueNotUniqueFailure(Error error)
        {
            _validatorMock
                .ValidateAttributeIsActiveAsync(
                    _command.AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateAttributeValueIsUniqueAsync(
                    _command.AttributeId,
                    _command.Value,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        #endregion

        #region Success Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenAttributeValueIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            Result<long> result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_ReturnAttributeValueId_WhenAttributeValueIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            Result<long> result = await _handler.Handle(_command, default);

            // Assert
            result.Value.Should().BeGreaterThanOrEqualTo(0);
        }

        [Fact]
        public async Task Handle_Should_CreateAttributeValueWithCorrectBasicProperties_WhenAttributeValueIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _attributeValueRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<AttributeValue>(av =>
                    av.AttributeId == _command.AttributeId &&
                    av.Value == _command.Value &&
                    av.DisplayOrder == DisplayOrder &&
                    av.IsActive == _command.IsActive),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateAttributeValueWithNumericValue_WhenProvided()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _attributeValueRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<AttributeValue>(av =>
                    av.NumericValue == NumericValue),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateAttributeValueWithBooleanValue_WhenProvided()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _attributeValueRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<AttributeValue>(av =>
                    av.BooleanValue == BooleanValue),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateAttributeValueWithNormalizedValue_WhenAttributeValueIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _attributeValueRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<AttributeValue>(av =>
                    av.NormalizedValue == ExpectedNormalizedValue),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateAttributeValueWithNullNumericValue_WhenNotProvided()
        {
            // Arrange
            SetupSuccessfulValidation();
            var commandWithoutNumericValue = _command with { NumericValue = null };

            _validatorMock
                .ValidateAttributeValueIsUniqueAsync(
                    commandWithoutNumericValue.AttributeId,
                    commandWithoutNumericValue.Value,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithoutNumericValue, default);

            // Assert
            await _attributeValueRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<AttributeValue>(av =>
                    av.NumericValue == null),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateAttributeValueWithNullBooleanValue_WhenNotProvided()
        {
            // Arrange
            SetupSuccessfulValidation();
            var commandWithoutBooleanValue = _command with { BooleanValue = null };

            _validatorMock
                .ValidateAttributeValueIsUniqueAsync(
                    commandWithoutBooleanValue.AttributeId,
                    commandWithoutBooleanValue.Value,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithoutBooleanValue, default);

            // Assert
            await _attributeValueRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<AttributeValue>(av =>
                    av.BooleanValue == null),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateAttributeValueWithIsActiveFalse_WhenSpecified()
        {
            // Arrange
            SetupSuccessfulValidation();
            var commandWithInactive = _command with { IsActive = false };

            _validatorMock
                .ValidateAttributeValueIsUniqueAsync(
                    commandWithInactive.AttributeId,
                    commandWithInactive.Value,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithInactive, default);

            // Assert
            await _attributeValueRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<AttributeValue>(av =>
                    av.IsActive == false),
                    Arg.Any<CancellationToken>());
        }

        #endregion

        #region Validation Scenarios

        [Fact]
        public async Task Handle_Should_CallValidateAttributeIsActiveAsync_Always()
        {
            // Arrange
            SetupSuccessfulValidation();

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
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateAttributeValueIsUniqueAsync(
                    AttributeId,
                    AttributeValueValue,
                    null,
                    Arg.Any<CancellationToken>());
        }

        #endregion

        #region Attribute Not Active Failure Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenAttributeIsNotActive()
        {
            // Arrange
            SetupAttributeNotActiveFailure(AttributeValueErrors.AttributeNotActive);

            // Act
            Result<long> result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AttributeValueErrors.AttributeNotActive);
        }

        [Fact]
        public async Task Handle_Should_NotCallValidateAttributeValueIsUniqueAsync_WhenAttributeIsNotActive()
        {
            // Arrange
            SetupAttributeNotActiveFailure(AttributeValueErrors.AttributeNotActive);

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
        public async Task Handle_Should_NotCallAddAsync_WhenAttributeIsNotActive()
        {
            // Arrange
            SetupAttributeNotActiveFailure(AttributeValueErrors.AttributeNotActive);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _attributeValueRepositoryMock
                .DidNotReceive()
                .AddAsync(Arg.Any<AttributeValue>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUnitOfWork_WhenAttributeIsNotActive()
        {
            // Arrange
            SetupAttributeNotActiveFailure(AttributeValueErrors.AttributeNotActive);

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
            SetupValueNotUniqueFailure(AttributeValueErrors.Duplicated);

            // Act
            Result<long> result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AttributeValueErrors.Duplicated);
        }

        [Fact]
        public async Task Handle_Should_NotCallAddAsync_WhenAttributeValueAlreadyExists()
        {
            // Arrange
            SetupValueNotUniqueFailure(AttributeValueErrors.Duplicated);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _attributeValueRepositoryMock
                .DidNotReceive()
                .AddAsync(Arg.Any<AttributeValue>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUnitOfWork_WhenAttributeValueAlreadyExists()
        {
            // Arrange
            SetupValueNotUniqueFailure(AttributeValueErrors.Duplicated);

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
        public async Task Handle_Should_CallAddAsync_ExactlyOnce_WhenAttributeValueIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _attributeValueRepositoryMock
                .Received(1)
                .AddAsync(Arg.Any<AttributeValue>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_ExactlyOnce_WhenAttributeValueIsValid()
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
        public async Task Handle_Should_CallSaveChangesAsync_AfterAddAsync_WhenAttributeValueIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(async () =>
            {
                await _attributeValueRepositoryMock.AddAsync(Arg.Any<AttributeValue>(), Arg.Any<CancellationToken>());
                await _unitOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>());
            });
        }

        #endregion

        #region Edge Cases

        [Fact]
        public async Task Handle_Should_CreateAttributeValue_WithZeroDisplayOrder()
        {
            // Arrange
            SetupSuccessfulValidation();
            var commandWithZeroDisplayOrder = _command with { DisplayOrder = 0 };

            _validatorMock
                .ValidateAttributeValueIsUniqueAsync(
                    commandWithZeroDisplayOrder.AttributeId,
                    commandWithZeroDisplayOrder.Value,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            Result<long> result = await _handler.Handle(commandWithZeroDisplayOrder, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            await _attributeValueRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<AttributeValue>(av =>
                    av.DisplayOrder == 0),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateAttributeValue_WithNegativeNumericValue()
        {
            // Arrange
            SetupSuccessfulValidation();
            var commandWithNegativeValue = _command with { NumericValue = -50.25m };

            _validatorMock
                .ValidateAttributeValueIsUniqueAsync(
                    commandWithNegativeValue.AttributeId,
                    commandWithNegativeValue.Value,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            Result<long> result = await _handler.Handle(commandWithNegativeValue, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            await _attributeValueRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<AttributeValue>(av =>
                    av.NumericValue == -50.25m),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateAttributeValue_WithBooleanFalse()
        {
            // Arrange
            SetupSuccessfulValidation();
            var commandWithFalseBoolean = _command with { BooleanValue = false };

            _validatorMock
                .ValidateAttributeValueIsUniqueAsync(
                    commandWithFalseBoolean.AttributeId,
                    commandWithFalseBoolean.Value,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            Result<long> result = await _handler.Handle(commandWithFalseBoolean, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            await _attributeValueRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<AttributeValue>(av =>
                    av.BooleanValue == false),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NormalizeValue_ToLowerCase()
        {
            // Arrange
            SetupSuccessfulValidation();
            var commandWithUpperCase = _command with { Value = "BLUE" };

            _validatorMock
                .ValidateAttributeValueIsUniqueAsync(
                    commandWithUpperCase.AttributeId,
                    commandWithUpperCase.Value,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithUpperCase, default);

            // Assert
            await _attributeValueRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<AttributeValue>(av =>
                    av.Value == "BLUE" &&
                    av.NormalizedValue == "blue"),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateAttributeValue_WithDifferentAttributeIds()
        {
            // Arrange
            long differentAttributeId = 999;
            var commandWithDifferentAttribute = _command with { AttributeId = differentAttributeId };

            _validatorMock
                .ValidateAttributeIsActiveAsync(
                    differentAttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateAttributeValueIsUniqueAsync(
                    differentAttributeId,
                    commandWithDifferentAttribute.Value,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            Result<long> result = await _handler.Handle(commandWithDifferentAttribute, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            await _attributeValueRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<AttributeValue>(av =>
                    av.AttributeId == differentAttributeId),
                    Arg.Any<CancellationToken>());
        }

        #endregion
    }
}

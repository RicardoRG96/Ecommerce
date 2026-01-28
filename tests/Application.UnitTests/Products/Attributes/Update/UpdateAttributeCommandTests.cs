using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.Attributes.Common.Services;
using Application.Products.Attributes.Update;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Products.Attributes.Update
{
    public class UpdateAttributeCommandTests
    {
        private const long AttributeId = 1;
        private const string AttributeCode = "SIZE";
        private const string OriginalAttributeCode = "COLOR";
        private const string AttributeName = "Size";
        private const string OriginalAttributeName = "Color";
        private const string AttributeDescription = "Product size attribute";
        private const string DataType = "String";
        private const int DisplayOrder = 5;

        private static readonly UpdateAttributeCommand _command = new(
            AttributeId: AttributeId,
            Code: AttributeCode,
            Name: AttributeName,
            Description: AttributeDescription,
            DataType: DataType,
            IsVariant: true,
            IsFilterable: true,
            IsRequired: false,
            DisplayOrder: DisplayOrder);

        private readonly UpdateAttributeCommandHandler _handler;
        private readonly IAttributeRepository _attributeRepositoryMock;
        private readonly IUniquenessAttributeCodeValidator _validatorMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public UpdateAttributeCommandTests()
        {
            _attributeRepositoryMock = Substitute.For<IAttributeRepository>();
            _validatorMock = Substitute.For<IUniquenessAttributeCodeValidator>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _handler = new(
                _attributeRepositoryMock,
                _validatorMock,
                _unitOfWorkMock);
        }

        #region Helper Methods

        private Domain.Entities.Products.Attribute CreateExistingAttribute()
        {
            return new Domain.Entities.Products.Attribute
            {
                Id = AttributeId,
                Code = OriginalAttributeCode,
                Name = OriginalAttributeName,
                Description = "Original description",
                DataType = "Integer",
                IsVariant = false,
                IsFilterable = false,
                IsRequired = true,
                DisplayOrder = 1,
                IsActive = true
            };
        }

        private void SetupSuccessfulScenario(Domain.Entities.Products.Attribute attribute)
        {
            _attributeRepositoryMock
                .GetByIdAsync(AttributeId, Arg.Any<CancellationToken>())
                .Returns(attribute);

            _validatorMock
                .ValidateAsync(
                    _command.Code,
                    AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());
        }

        private void SetupAttributeNotFound()
        {
            _attributeRepositoryMock
                .GetByIdAsync(AttributeId, Arg.Any<CancellationToken>())
                .Returns((Domain.Entities.Products.Attribute?)null);
        }

        private void SetupCodeValidationFailure(Domain.Entities.Products.Attribute attribute, Error error)
        {
            _attributeRepositoryMock
                .GetByIdAsync(AttributeId, Arg.Any<CancellationToken>())
                .Returns(attribute);

            _validatorMock
                .ValidateAsync(
                    _command.Code,
                    AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        #endregion

        #region Success Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenAttributeExistsAndAllValidationsPass()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            SetupSuccessfulScenario(existingAttribute);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_UpdateBasicProperties_WhenAttributeIsValid()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            SetupSuccessfulScenario(existingAttribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingAttribute.Code.Should().Be(_command.Code);
            existingAttribute.Name.Should().Be(_command.Name);
            existingAttribute.Description.Should().Be(_command.Description);
            existingAttribute.DataType.Should().Be(_command.DataType);
        }

        [Fact]
        public async Task Handle_Should_UpdateFlags_WhenAttributeIsValid()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            SetupSuccessfulScenario(existingAttribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingAttribute.IsVariant.Should().Be(_command.IsVariant);
            existingAttribute.IsFilterable.Should().Be(_command.IsFilterable);
            existingAttribute.IsRequired.Should().Be(_command.IsRequired);
        }

        [Fact]
        public async Task Handle_Should_UpdateDisplayOrder_WhenAttributeIsValid()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            SetupSuccessfulScenario(existingAttribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingAttribute.DisplayOrder.Should().Be(DisplayOrder);
        }

        [Fact]
        public async Task Handle_Should_AllowSameCode_WhenUpdatingSameAttribute()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            existingAttribute.Code = _command.Code;
            
            SetupSuccessfulScenario(existingAttribute);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_UpdateToFalseFlags_WhenCommandSpecifies()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            existingAttribute.IsVariant = true;
            existingAttribute.IsFilterable = true;
            existingAttribute.IsRequired = true;

            var commandWithFalseFlags = _command with 
            { 
                IsVariant = false, 
                IsFilterable = false, 
                IsRequired = false 
            };

            _attributeRepositoryMock
                .GetByIdAsync(AttributeId, Arg.Any<CancellationToken>())
                .Returns(existingAttribute);

            _validatorMock
                .ValidateAsync(
                    commandWithFalseFlags.Code,
                    AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithFalseFlags, default);

            // Assert
            existingAttribute.IsVariant.Should().BeFalse();
            existingAttribute.IsFilterable.Should().BeFalse();
            existingAttribute.IsRequired.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_UpdateToTrueFlags_WhenCommandSpecifies()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            existingAttribute.IsVariant = false;
            existingAttribute.IsFilterable = false;
            existingAttribute.IsRequired = false;

            var commandWithTrueFlags = _command with 
            { 
                IsVariant = true, 
                IsFilterable = true, 
                IsRequired = true 
            };

            _attributeRepositoryMock
                .GetByIdAsync(AttributeId, Arg.Any<CancellationToken>())
                .Returns(existingAttribute);

            _validatorMock
                .ValidateAsync(
                    commandWithTrueFlags.Code,
                    AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithTrueFlags, default);

            // Assert
            existingAttribute.IsVariant.Should().BeTrue();
            existingAttribute.IsFilterable.Should().BeTrue();
            existingAttribute.IsRequired.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_UpdateDataType_WhenChanged()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            existingAttribute.DataType = "Integer";

            var commandWithNewDataType = _command with { DataType = "Decimal" };

            _attributeRepositoryMock
                .GetByIdAsync(AttributeId, Arg.Any<CancellationToken>())
                .Returns(existingAttribute);

            _validatorMock
                .ValidateAsync(
                    commandWithNewDataType.Code,
                    AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithNewDataType, default);

            // Assert
            existingAttribute.DataType.Should().Be("Decimal");
        }

        #endregion

        #region Attribute Not Found Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenAttributeDoesNotExist()
        {
            // Arrange
            SetupAttributeNotFound();

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AttributeErrors.NotFound(AttributeId));
        }

        [Fact]
        public async Task Handle_Should_NotCallValidateAsync_WhenAttributeDoesNotExist()
        {
            // Arrange
            SetupAttributeNotFound();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .DidNotReceive()
                .ValidateAsync(
                    Arg.Any<string>(),
                    Arg.Any<long?>(),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenAttributeDoesNotExist()
        {
            // Arrange
            SetupAttributeNotFound();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _attributeRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<Domain.Entities.Products.Attribute>());
        }

        [Fact]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenAttributeDoesNotExist()
        {
            // Arrange
            SetupAttributeNotFound();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region Code Validation Failure Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenAttributeCodeAlreadyExists()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            SetupCodeValidationFailure(existingAttribute, AttributeErrors.DuplicatedAttributeCode);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AttributeErrors.DuplicatedAttributeCode);
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenAttributeCodeAlreadyExists()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            SetupCodeValidationFailure(existingAttribute, AttributeErrors.DuplicatedAttributeCode);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _attributeRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<Domain.Entities.Products.Attribute>());
        }

        [Fact]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenAttributeCodeAlreadyExists()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            SetupCodeValidationFailure(existingAttribute, AttributeErrors.DuplicatedAttributeCode);

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
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            SetupSuccessfulScenario(existingAttribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _attributeRepositoryMock
                .Received(1)
                .GetByIdAsync(AttributeId, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallValidateAsync_WhenAttributeExists()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            SetupSuccessfulScenario(existingAttribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateAsync(
                    AttributeCode,
                    AttributeId,
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_PassCorrectAttributeIdToValidator_WhenValidating()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            SetupSuccessfulScenario(existingAttribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateAsync(
                    Arg.Any<string>(),
                    AttributeId,
                    Arg.Any<CancellationToken>());
        }

        #endregion

        #region Repository and UnitOfWork Interactions

        [Fact]
        public async Task Handle_Should_CallUpdate_ExactlyOnce_WhenAttributeIsValid()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            SetupSuccessfulScenario(existingAttribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _attributeRepositoryMock
                .Received(1)
                .Update(Arg.Any<Domain.Entities.Products.Attribute>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_ExactlyOnce_WhenAttributeIsValid()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            SetupSuccessfulScenario(existingAttribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_AfterUpdate_WhenAttributeIsValid()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            SetupSuccessfulScenario(existingAttribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(() =>
            {
                _attributeRepositoryMock.Update(Arg.Any<Domain.Entities.Products.Attribute>());
                _unitOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>());
            });
        }

        [Fact]
        public async Task Handle_Should_UpdateCorrectAttributeInstance_WhenAttributeIsValid()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            SetupSuccessfulScenario(existingAttribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _attributeRepositoryMock
                .Received(1)
                .Update(existingAttribute);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public async Task Handle_Should_UpdateDisplayOrder_ToZero()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            var commandWithZeroDisplayOrder = _command with { DisplayOrder = 0 };

            _attributeRepositoryMock
                .GetByIdAsync(AttributeId, Arg.Any<CancellationToken>())
                .Returns(existingAttribute);

            _validatorMock
                .ValidateAsync(
                    commandWithZeroDisplayOrder.Code,
                    AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithZeroDisplayOrder, default);

            // Assert
            existingAttribute.DisplayOrder.Should().Be(0);
        }

        [Fact]
        public async Task Handle_Should_UpdateDisplayOrder_ToLargeValue()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            var commandWithLargeDisplayOrder = _command with { DisplayOrder = 999 };

            _attributeRepositoryMock
                .GetByIdAsync(AttributeId, Arg.Any<CancellationToken>())
                .Returns(existingAttribute);

            _validatorMock
                .ValidateAsync(
                    commandWithLargeDisplayOrder.Code,
                    AttributeId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithLargeDisplayOrder, default);

            // Assert
            existingAttribute.DisplayOrder.Should().Be(999);
        }

        [Fact]
        public async Task Handle_Should_PreserveAttributeId_AfterUpdate()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            SetupSuccessfulScenario(existingAttribute);
            long originalId = existingAttribute.Id;

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingAttribute.Id.Should().Be(originalId);
        }

        [Fact]
        public async Task Handle_Should_UpdateAllPropertiesAtOnce_WhenAllAreChanged()
        {
            // Arrange
            Domain.Entities.Products.Attribute existingAttribute = CreateExistingAttribute();
            SetupSuccessfulScenario(existingAttribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingAttribute.Code.Should().Be(_command.Code);
            existingAttribute.Name.Should().Be(_command.Name);
            existingAttribute.Description.Should().Be(_command.Description);
            existingAttribute.DataType.Should().Be(_command.DataType);
            existingAttribute.IsVariant.Should().Be(_command.IsVariant);
            existingAttribute.IsFilterable.Should().Be(_command.IsFilterable);
            existingAttribute.IsRequired.Should().Be(_command.IsRequired);
            existingAttribute.DisplayOrder.Should().Be(_command.DisplayOrder);
        }

        #endregion
    }
}

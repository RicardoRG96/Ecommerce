using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.Attributes.Common.Services;
using Application.Products.Attributes.Create;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Products.Attributes.Create
{
    public class CreateAttributeCommandTests
    {
        private const string AttributeCode = "COLOR";
        private const string AttributeName = "Color";
        private const string AttributeDescription = "Product color attribute";
        private const string DataType = "String";
        private const int DisplayOrder = 1;

        private static readonly CreateAttributeCommand _command = new(
            Code: AttributeCode,
            Name: AttributeName,
            Description: AttributeDescription,
            DataType: DataType,
            IsVariant: true,
            IsFilterable: true,
            IsRequired: false,
            DisplayOrder: DisplayOrder,
            IsActive: true);

        private readonly CreateAttributeCommandHandler _handler;
        private readonly IAttributeRepository _attributeRepositoryMock;
        private readonly IUniquenessAttributeCodeValidator _validatorMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public CreateAttributeCommandTests()
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

        private void SetupSuccessfulValidation()
        {
            _validatorMock
                .ValidateAsync(
                    _command.Code,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());
        }

        private void SetupCodeValidationFailure(Error error)
        {
            _validatorMock
                .ValidateAsync(
                    _command.Code,
                    null,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        #endregion

        #region Success Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenAttributeIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            Result<long> result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_ReturnAttributeId_WhenAttributeIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            Result<long> result = await _handler.Handle(_command, default);

            // Assert
            result.Value.Should().BeGreaterThanOrEqualTo(0);
        }

        [Fact]
        public async Task Handle_Should_CreateAttributeWithCorrectBasicProperties_WhenAttributeIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _attributeRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<Domain.Entities.Products.Attribute>(a =>
                    a.Code == _command.Code &&
                    a.Name == _command.Name &&
                    a.Description == _command.Description &&
                    a.DataType == _command.DataType),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateAttributeWithCorrectFlags_WhenAttributeIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _attributeRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<Domain.Entities.Products.Attribute>(a =>
                    a.IsVariant == _command.IsVariant &&
                    a.IsFilterable == _command.IsFilterable &&
                    a.IsRequired == _command.IsRequired &&
                    a.IsActive == _command.IsActive),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateAttributeWithCorrectDisplayOrder_WhenAttributeIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _attributeRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<Domain.Entities.Products.Attribute>(a =>
                    a.DisplayOrder == DisplayOrder),
                    Arg.Any<CancellationToken>());
        }

        #endregion

        #region Validation Scenarios

        [Fact]
        public async Task Handle_Should_CallValidateAsync_Always()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateAsync(
                    AttributeCode,
                    null,
                    Arg.Any<CancellationToken>());
        }

        #endregion

        #region Code Validation Failure Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenAttributeCodeAlreadyExists()
        {
            // Arrange
            SetupCodeValidationFailure(AttributeErrors.DuplicatedAttributeCode);

            // Act
            Result<long> result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AttributeErrors.DuplicatedAttributeCode);
        }

        [Fact]
        public async Task Handle_Should_NotCallAddAsync_WhenAttributeCodeAlreadyExists()
        {
            // Arrange
            SetupCodeValidationFailure(AttributeErrors.DuplicatedAttributeCode);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _attributeRepositoryMock
                .DidNotReceive()
                .AddAsync(Arg.Any<Domain.Entities.Products.Attribute>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUnitOfWork_WhenAttributeCodeAlreadyExists()
        {
            // Arrange
            SetupCodeValidationFailure(AttributeErrors.DuplicatedAttributeCode);

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
        public async Task Handle_Should_CallAddAsync_ExactlyOnce_WhenAttributeIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _attributeRepositoryMock
                .Received(1)
                .AddAsync(Arg.Any<Domain.Entities.Products.Attribute>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_ExactlyOnce_WhenAttributeIsValid()
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
        public async Task Handle_Should_CallSaveChangesAsync_AfterAddAsync_WhenAttributeIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(async () =>
            {
                await _attributeRepositoryMock.AddAsync(Arg.Any<Domain.Entities.Products.Attribute>(), Arg.Any<CancellationToken>());
                await _unitOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>());
            });
        }

        #endregion

        #region Edge Cases

        [Fact]
        public async Task Handle_Should_CreateAttribute_WhenIsVariantIsFalse()
        {
            // Arrange
            SetupSuccessfulValidation();
            var command = _command with { IsVariant = false };

            // Act
            Result<long> result = await _handler.Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            await _attributeRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<Domain.Entities.Products.Attribute>(a =>
                    a.IsVariant == false),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateAttribute_WhenIsFilterableIsFalse()
        {
            // Arrange
            SetupSuccessfulValidation();
            var command = _command with { IsFilterable = false };

            // Act
            Result<long> result = await _handler.Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            await _attributeRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<Domain.Entities.Products.Attribute>(a =>
                    a.IsFilterable == false),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateAttribute_WhenIsRequiredIsTrue()
        {
            // Arrange
            SetupSuccessfulValidation();
            var command = _command with { IsRequired = true };

            // Act
            Result<long> result = await _handler.Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            await _attributeRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<Domain.Entities.Products.Attribute>(a =>
                    a.IsRequired == true),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateAttribute_WhenIsActiveIsFalse()
        {
            // Arrange
            SetupSuccessfulValidation();
            var command = _command with { IsActive = false };

            // Act
            Result<long> result = await _handler.Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            await _attributeRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<Domain.Entities.Products.Attribute>(a =>
                    a.IsActive == false),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateAttribute_WithDifferentDataTypes()
        {
            // Arrange
            SetupSuccessfulValidation();
            var command = _command with { DataType = "Integer" };

            // Act
            Result<long> result = await _handler.Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            await _attributeRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<Domain.Entities.Products.Attribute>(a =>
                    a.DataType == "Integer"),
                    Arg.Any<CancellationToken>());
        }
        #endregion
    }
}

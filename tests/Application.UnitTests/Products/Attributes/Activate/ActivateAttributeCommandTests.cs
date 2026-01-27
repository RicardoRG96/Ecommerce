using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.Attributes.Activate;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Products.Attributes.Activate
{
    public class ActivateAttributeCommandTests
    {
        private const long AttributeId = 1;

        private static readonly ActivateAttributeCommand _command = new(AttributeId);

        private readonly ActivateAttributeCommandHandler _handler;
        private readonly IAttributeRepository _attributeRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public ActivateAttributeCommandTests()
        {
            _attributeRepositoryMock = Substitute.For<IAttributeRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _handler = new(
                _attributeRepositoryMock,
                _unitOfWorkMock);
        }

        #region Helper Methods

        private Domain.Entities.Products.Attribute CreateInactiveAttribute()
        {
            return new Domain.Entities.Products.Attribute
            {
                Id = AttributeId,
                Code = "COLOR",
                Name = "Color",
                IsActive = false
            };
        }

        private Domain.Entities.Products.Attribute CreateActiveAttribute()
        {
            return new Domain.Entities.Products.Attribute
            {
                Id = AttributeId,
                Code = "SIZE",
                Name = "Size",
                IsActive = true
            };
        }

        private void SetupAttributeFound(Domain.Entities.Products.Attribute attribute)
        {
            _attributeRepositoryMock
                .GetByIdAsync(AttributeId, Arg.Any<CancellationToken>())
                .Returns(attribute);
        }

        private void SetupAttributeNotFound()
        {
            _attributeRepositoryMock
                .GetByIdAsync(AttributeId, Arg.Any<CancellationToken>())
                .ReturnsNull();
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

        #region Inactive Attribute Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenAttributeIsInactive()
        {
            // Arrange
            Domain.Entities.Products.Attribute attribute = CreateInactiveAttribute();
            SetupAttributeFound(attribute);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_ActivateAttribute_WhenAttributeIsInactive()
        {
            // Arrange
            Domain.Entities.Products.Attribute attribute = CreateInactiveAttribute();
            SetupAttributeFound(attribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            attribute.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_CallUpdate_WhenAttributeIsInactive()
        {
            // Arrange
            Domain.Entities.Products.Attribute attribute = CreateInactiveAttribute();
            SetupAttributeFound(attribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _attributeRepositoryMock
                .Received(1)
                .Update(Arg.Any<Domain.Entities.Products.Attribute>());
        }

        [Fact]
        public async Task Handle_Should_CallUpdateWithCorrectAttribute_WhenAttributeIsInactive()
        {
            // Arrange
            Domain.Entities.Products.Attribute attribute = CreateInactiveAttribute();
            SetupAttributeFound(attribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _attributeRepositoryMock
                .Received(1)
                .Update(attribute);
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_WhenAttributeIsInactive()
        {
            // Arrange
            Domain.Entities.Products.Attribute attribute = CreateInactiveAttribute();
            SetupAttributeFound(attribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_AfterUpdate_WhenAttributeIsInactive()
        {
            // Arrange
            Domain.Entities.Products.Attribute attribute = CreateInactiveAttribute();
            SetupAttributeFound(attribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(() =>
            {
                _attributeRepositoryMock.Update(Arg.Any<Domain.Entities.Products.Attribute>());
                _unitOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>());
            });
        }

        #endregion

        #region Already Active Attribute Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenAttributeIsAlreadyActive()
        {
            // Arrange
            Domain.Entities.Products.Attribute attribute = CreateActiveAttribute();
            SetupAttributeFound(attribute);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_NotChangeIsActive_WhenAttributeIsAlreadyActive()
        {
            // Arrange
            Domain.Entities.Products.Attribute attribute = CreateActiveAttribute();
            SetupAttributeFound(attribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            attribute.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenAttributeIsAlreadyActive()
        {
            // Arrange
            Domain.Entities.Products.Attribute attribute = CreateActiveAttribute();
            SetupAttributeFound(attribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _attributeRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<Domain.Entities.Products.Attribute>());
        }

        [Fact]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenAttributeIsAlreadyActive()
        {
            // Arrange
            Domain.Entities.Products.Attribute attribute = CreateActiveAttribute();
            SetupAttributeFound(attribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region Repository Interaction Scenarios

        [Fact]
        public async Task Handle_Should_CallGetByIdAsync_Always()
        {
            // Arrange
            Domain.Entities.Products.Attribute attribute = CreateInactiveAttribute();
            SetupAttributeFound(attribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _attributeRepositoryMock
                .Received(1)
                .GetByIdAsync(AttributeId, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallGetByIdAsync_WithCorrectAttributeId()
        {
            // Arrange
            Domain.Entities.Products.Attribute attribute = CreateInactiveAttribute();
            SetupAttributeFound(attribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _attributeRepositoryMock
                .Received(1)
                .GetByIdAsync(
                    Arg.Is<long>(id => id == AttributeId),
                    Arg.Any<CancellationToken>());
        }

        #endregion

        #region Edge Cases

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenDifferentAttributeIdDoesNotExist()
        {
            // Arrange
            long nonExistentAttributeId = 9999;
            var command = new ActivateAttributeCommand(nonExistentAttributeId);

            _attributeRepositoryMock
                .GetByIdAsync(nonExistentAttributeId, Arg.Any<CancellationToken>())
                .ReturnsNull();

            // Act
            Result result = await _handler.Handle(command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AttributeErrors.NotFound(nonExistentAttributeId));
        }

        [Fact]
        public async Task Handle_Should_PreserveAttributeId_AfterActivation()
        {
            // Arrange
            Domain.Entities.Products.Attribute attribute = CreateInactiveAttribute();
            SetupAttributeFound(attribute);
            long originalId = attribute.Id;

            // Act
            await _handler.Handle(_command, default);

            // Assert
            attribute.Id.Should().Be(originalId);
        }

        [Fact]
        public async Task Handle_Should_OnlyModifyIsActiveProperty_WhenActivatingAttribute()
        {
            // Arrange
            Domain.Entities.Products.Attribute attribute = CreateInactiveAttribute();
            string originalCode = attribute.Code!;
            string originalName = attribute.Name!;
            
            SetupAttributeFound(attribute);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            attribute.IsActive.Should().BeTrue();
            attribute.Code.Should().Be(originalCode);
            attribute.Name.Should().Be(originalName);
        }

        #endregion
    }
}

using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.Categories.ChangeCategoryParent;
using Domain.Entities.Products;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Products.Categories.ChangeCategoryParent
{
    public class ChangeCategoryParentCommandTests
    {
        private static readonly ChangeCategoryParentCommand _command = new(
            CategoryId: 1,
            NewParentId: 5);

        private readonly ChangeCategoryParentCommandHandler _handler;
        private readonly ICategoryRepository _categoryRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public ChangeCategoryParentCommandTests()
        {
            _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _handler = new(_categoryRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenCategoryDoesNotExist()
        {
            // Arrange
            _categoryRepositoryMock
                .GetByIdAsync(_command.CategoryId, Arg.Any<CancellationToken>())
                .ReturnsNull();

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CategoryErrors.NotFound(_command.CategoryId));
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenCategoryExists()
        {
            // Arrange
            Category category = new()
            {
                Id = _command.CategoryId,
                ParentId = 2,
                Name = "Electronics"
            };

            _categoryRepositoryMock
                .GetByIdAsync(_command.CategoryId, Arg.Any<CancellationToken>())
                .Returns(category);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_UpdateParentId_WhenCategoryExists()
        {
            // Arrange
            Category category = new()
            {
                Id = _command.CategoryId,
                ParentId = 2,
                Name = "Electronics"
            };

            _categoryRepositoryMock
                .GetByIdAsync(_command.CategoryId, Arg.Any<CancellationToken>())
                .Returns(category);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            category.ParentId.Should().Be(_command.NewParentId);
        }

        [Fact]
        public async Task Handle_Should_CallUpdate_WhenCategoryExists()
        {
            // Arrange
            Category category = new()
            {
                Id = _command.CategoryId,
                ParentId = 2,
                Name = "Electronics"
            };

            _categoryRepositoryMock
                .GetByIdAsync(_command.CategoryId, Arg.Any<CancellationToken>())
                .Returns(category);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _categoryRepositoryMock
                .Received(1)
                .Update(Arg.Is<Category>(c =>
                    c.Id == _command.CategoryId &&
                    c.ParentId == _command.NewParentId));
        }

        [Fact]
        public async Task Handle_Should_CallUnitOfWork_WhenCategoryExists()
        {
            // Arrange
            Category category = new()
            {
                Id = _command.CategoryId,
                ParentId = 2,
                Name = "Electronics"
            };

            _categoryRepositoryMock
                .GetByIdAsync(_command.CategoryId, Arg.Any<CancellationToken>())
                .Returns(category);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_ChangeParentIdToNull_WhenNewParentIdIsZero()
        {
            // Arrange
            ChangeCategoryParentCommand commandWithNullParent = new(
                CategoryId: 1,
                NewParentId: 0);

            Category category = new()
            {
                Id = commandWithNullParent.CategoryId,
                ParentId = 2,
                Name = "Electronics"
            };

            _categoryRepositoryMock
                .GetByIdAsync(commandWithNullParent.CategoryId, Arg.Any<CancellationToken>())
                .Returns(category);

            // Act
            await _handler.Handle(commandWithNullParent, default);

            // Assert
            category.ParentId.Should().Be(0);
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenCategoryDoesNotExist()
        {
            // Arrange
            _categoryRepositoryMock
                .GetByIdAsync(_command.CategoryId, Arg.Any<CancellationToken>())
                .ReturnsNull();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _categoryRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<Category>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUnitOfWork_WhenCategoryDoesNotExist()
        {
            // Arrange
            _categoryRepositoryMock
                .GetByIdAsync(_command.CategoryId, Arg.Any<CancellationToken>())
                .ReturnsNull();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}

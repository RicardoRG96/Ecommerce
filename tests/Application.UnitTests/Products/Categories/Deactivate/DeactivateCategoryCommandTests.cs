using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.Categories.Deactivate;
using Domain.Entities.Products;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Products.Categories.Deactivate
{
    public class DeactivateCategoryCommandTests
    {
        private static readonly DeactivateCategoryCommand _command = new(1);
        private readonly DeactivateCategoryCommandHandler _handler;
        private readonly ICategoryRepository _categoryRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public DeactivateCategoryCommandTests()
        {
            _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _handler = new(_categoryRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenCategoryIdDoesNotExist()
        {
            long notExistingCategoryId = 2500;
            DeactivateCategoryCommand invalidCommand = _command with { CategoryId = notExistingCategoryId };

            _categoryRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == invalidCommand.CategoryId), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result result = await _handler.Handle(invalidCommand, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_DeactivateCategory_WhenCategoryIsActive()
        {
            Category category = new()
            {
                Id = _command.CategoryId,
                IsActive = true
            };

            _categoryRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.CategoryId), Arg.Any<CancellationToken>())
                .Returns(category);

            Result result = await _handler.Handle(_command, default);

            result.IsSuccess.Should().BeTrue();
            category.IsActive.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldCallRepositoryWhenCategoryIsActive()
        {
            Category category = new()
            {
                Id = _command.CategoryId,
                IsActive = true
            };

            _categoryRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.CategoryId), Arg.Any<CancellationToken>())
                .Returns(category);

            await _handler.Handle(_command, default);

            _categoryRepositoryMock
                .Received(1)
                .Update(Arg.Is<Category>(c => c.Id == _command.CategoryId && c.IsActive == false));
        }

        [Fact]
        public async Task Handle_ShouldCallUnitOfWork_WhenCategoryIsActive()
        {
            Category category = new()
            {
                Id = _command.CategoryId,
                IsActive = true
            };

            _categoryRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.CategoryId), Arg.Any<CancellationToken>())
                .Returns(category);

            await _handler.Handle(_command, default);

            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_DoNothing_WhenCategoryIsAlreadyInactive()
        {
            Category category = new()
            {
                Id = _command.CategoryId,
                IsActive = false
            };

            _categoryRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.CategoryId), Arg.Any<CancellationToken>())
                .Returns(category);

            Result result = await _handler.Handle(_command, default);

            result.IsSuccess.Should().BeTrue();

            _categoryRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<Category>());

            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}

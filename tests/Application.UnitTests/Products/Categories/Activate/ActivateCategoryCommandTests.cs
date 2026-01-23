using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.Categories.Activate;
using Domain.Entities.Products;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Products.Categories.Activate
{
    public class ActivateCategoryCommandTests
    {
        private static readonly ActivateCategoryCommand _command = new(1);
        private readonly ActivateCategoryCommandHandler _handler;
        private readonly ICategoryRepository _categoryRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public ActivateCategoryCommandTests()
        {
            _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _handler = new(_categoryRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenCategoryIdDoesNotExist()
        {
            long notExistingCategoryId = 2500;
            ActivateCategoryCommand invalidCommand = _command with { CategoryId = notExistingCategoryId };

            _categoryRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == invalidCommand.CategoryId), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result result = await _handler.Handle(invalidCommand, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_ActivateCategory_WhenCategoryIsInactive()
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
            category.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_CallRepository_WhenCategoryIsInactive()
        {
            Category category = new()
            {
                Id = _command.CategoryId,
                IsActive = false
            };

            _categoryRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.CategoryId), Arg.Any<CancellationToken>())
                .Returns(category);

            await _handler.Handle(_command, default);

            _categoryRepositoryMock
                .Received(1)
                .Update(Arg.Any<Category>());
        }

        [Fact]
        public async Task Handle_Should_CallUnitOfWork_WhenCategoryIsInactive()
        {
            Category category = new()
            {
                Id = _command.CategoryId,
                IsActive = false
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
        public async Task Handle_Should_DoNothing_WhenCategoryIsAlreadyActive()
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
            category.IsActive.Should().BeTrue();

            _categoryRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<Category>());

            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}

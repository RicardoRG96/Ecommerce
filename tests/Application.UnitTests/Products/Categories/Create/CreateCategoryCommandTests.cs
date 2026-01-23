using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.Categories.Create;
using Domain.Entities.Products;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Products.Categories.Create
{
    public class CreateCategoryCommandTests
    {
        private static readonly CreateCategoryCommand _command = new(
            ParentId: 0,
            Name: "Electronics",
            Description: "Electronic devices and accessories",
            ImageUrl: "https://example.com/images/electronics.png",
            Icon: "fa-bolt",
            DisplayOrder: 1,
            IsActive: true,
            IsVisibleInMenu: true,
            MetaTitle: "Electronics - Shop Devices & Accessories",
            MetaDescription: "Browse our wide selection of electronic devices and accessories",
            MetaKeywords: "electronics, devices, gadgets, accessories");

        private readonly CreateCategoryCommandHandler _handler;
        private readonly ICategoryRepository _categoryRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public CreateCategoryCommandTests()
        {
            _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _handler = new(_categoryRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenCategoryIsValid()
        {
            // Arrange
            _categoryRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns((Category?)null);

            // Act
            Result<long> result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenCategoryNameAlreadyExists()
        {
            // Arrange
            Category existingCategory = new()
            {
                Id = 1,
                Name = _command.Name
            };

            _categoryRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns(existingCategory);

            // Act
            Result<long> result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CategoryErrors.CategoryAlreadyExists);
        }

        [Fact]
        public async Task Handle_Should_CallAddAsync_WhenCategoryIsValid()
        {
            // Arrange
            _categoryRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns((Category?)null);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _categoryRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<Category>(c =>
                    c.Name == _command.Name &&
                    c.Description == _command.Description &&
                    c.ParentId == _command.ParentId &&
                    c.IsActive == _command.IsActive &&
                    c.IsVisibleInMenu == _command.IsVisibleInMenu &&
                    c.DisplayOrder == _command.DisplayOrder),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallUnitOfWork_WhenCategoryIsValid()
        {
            // Arrange
            _categoryRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns((Category?)null);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_GenerateSlug_WhenCategoryIsValid()
        {
            // Arrange
            _categoryRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns((Category?)null);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _categoryRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<Category>(c => c.Slug == "electronics"), 
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_ReturnCategoryId_WhenCategoryIsValid()
        {
            // Arrange
            _categoryRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns((Category?)null);

            // Act
            Result<long> result = await _handler.Handle(_command, default);

            // Assert
            result.Value.Should().BeGreaterThanOrEqualTo(0);
        }

        [Fact]
        public async Task Handle_Should_NotCallAddAsync_WhenCategoryNameAlreadyExists()
        {
            // Arrange
            Category existingCategory = new()
            {
                Id = 1,
                Name = _command.Name
            };

            _categoryRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns(existingCategory);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _categoryRepositoryMock
                .DidNotReceive()
                .AddAsync(Arg.Any<Category>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUnitOfWork_WhenCategoryNameAlreadyExists()
        {
            // Arrange
            Category existingCategory = new()
            {
                Id = 1,
                Name = _command.Name
            };

            _categoryRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns(existingCategory);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateCategoryWithAllProperties()
        {
            // Arrange
            _categoryRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns((Category?)null);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _categoryRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<Category>(c =>
                    c.Name == _command.Name &&
                    c.Description == _command.Description &&
                    c.ImageUrl == _command.ImageUrl &&
                    c.Icon == _command.Icon &&
                    c.MetaTitle == _command.MetaTitle &&
                    c.MetaDescription == _command.MetaDescription &&
                    c.MetaKeywords == _command.MetaKeywords),
                    Arg.Any<CancellationToken>());
        }
    }
}

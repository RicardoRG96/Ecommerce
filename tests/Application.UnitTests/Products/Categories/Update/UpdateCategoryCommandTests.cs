using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.Categories.Update;
using Domain.Entities.Products;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Products.Categories.Update
{
    public class UpdateCategoryCommandTests
    {
        private static readonly UpdateCategoryCommand _command = new(
            CategoryId: 1,
            Name: "Home Appliances",
            Description: "Kitchen and home appliances for everyday use",
            ImageUrl: "https://example.com/images/appliances.png",
            Icon: "fa-home",
            DisplayOrder: 3,
            IsVisibleInMenu: true,
            MetaTitle: "Home Appliances - Kitchen & Home",
            MetaDescription: "Browse our collection of home and kitchen appliances",
            MetaKeywords: "appliances, kitchen, home, refrigerators, ovens");

        private readonly UpdateCategoryCommandHandler _handler;
        private readonly ICategoryRepository _categoryRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public UpdateCategoryCommandTests()
        {
            _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _handler = new(_categoryRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenCategoryExistsAndNameIsDifferent()
        {
            // Arrange
            Category existingCategory = new()
            {
                Id = _command.CategoryId,
                Name = "Electronics",
                Description = "Old Description"
            };

            _categoryRepositoryMock
                .GetByIdAsync(_command.CategoryId, Arg.Any<CancellationToken>())
                .Returns(existingCategory);

            _categoryRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns((Category?)null);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenCategoryDoesNotExist()
        {
            // Arrange
            _categoryRepositoryMock
                .GetByIdAsync(_command.CategoryId, Arg.Any<CancellationToken>())
                .Returns((Category?)null);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CategoryErrors.NotFound(_command.CategoryId));
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenNameIsDuplicatedWithDifferentCategory()
        {
            // Arrange
            Category categoryToUpdate = new()
            {
                Id = _command.CategoryId,
                Name = "Electronics"
            };

            Category existingCategory = new()
            {
                Id = 2,
                Name = _command.Name
            };

            _categoryRepositoryMock
                .GetByIdAsync(_command.CategoryId, Arg.Any<CancellationToken>())
                .Returns(categoryToUpdate);

            _categoryRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns(existingCategory);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CategoryErrors.CategoryAlreadyExists);
        }

        [Fact]
        public async Task Handle_Should_AllowSameName_WhenUpdatingSameCategory()
        {
            // Arrange
            Category existingCategory = new()
            {
                Id = _command.CategoryId,
                Name = _command.Name
            };

            _categoryRepositoryMock
                .GetByIdAsync(_command.CategoryId, Arg.Any<CancellationToken>())
                .Returns(existingCategory);

            _categoryRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns(existingCategory);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_UpdateCategoryProperties_WhenCategoryIsValid()
        {
            // Arrange
            Category existingCategory = new()
            {
                Id = _command.CategoryId,
                Name = "Electronics",
                Description = "Old Description",
                ImageUrl = "old-image.png",
                Icon = "old-icon",
                DisplayOrder = 1,
                IsVisibleInMenu = false
            };

            _categoryRepositoryMock
                .GetByIdAsync(_command.CategoryId, Arg.Any<CancellationToken>())
                .Returns(existingCategory);

            _categoryRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns((Category?)null);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingCategory.Name.Should().Be(_command.Name);
            existingCategory.Description.Should().Be(_command.Description);
            existingCategory.ImageUrl.Should().Be(_command.ImageUrl);
            existingCategory.Icon.Should().Be(_command.Icon);
            existingCategory.DisplayOrder.Should().Be(_command.DisplayOrder);
            existingCategory.IsVisibleInMenu.Should().Be(_command.IsVisibleInMenu);
            existingCategory.MetaTitle.Should().Be(_command.MetaTitle);
            existingCategory.MetaDescription.Should().Be(_command.MetaDescription);
            existingCategory.MetaKeywords.Should().Be(_command.MetaKeywords);
        }

        [Fact]
        public async Task Handle_Should_GenerateSlug_WhenUpdatingName()
        {
            // Arrange
            Category existingCategory = new()
            {
                Id = _command.CategoryId,
                Name = "Electronics",
                Slug = "electronics"
            };

            _categoryRepositoryMock
                .GetByIdAsync(_command.CategoryId, Arg.Any<CancellationToken>())
                .Returns(existingCategory);

            _categoryRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns((Category?)null);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingCategory.Slug.Should().Be("home-appliances");
        }

        [Fact]
        public async Task Handle_Should_CallUpdate_WhenCategoryIsValid()
        {
            // Arrange
            Category existingCategory = new()
            {
                Id = _command.CategoryId,
                Name = "Electronics"
            };

            _categoryRepositoryMock
                .GetByIdAsync(_command.CategoryId, Arg.Any<CancellationToken>())
                .Returns(existingCategory);

            _categoryRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns((Category?)null);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _categoryRepositoryMock
                .Received(1)
                .Update(Arg.Is<Category>(c =>
                    c.Id == _command.CategoryId &&
                    c.Name == _command.Name &&
                    c.Description == _command.Description));
        }

        [Fact]
        public async Task Handle_Should_CallUnitOfWork_WhenCategoryIsValid()
        {
            // Arrange
            Category existingCategory = new()
            {
                Id = _command.CategoryId,
                Name = "Electronics"
            };

            _categoryRepositoryMock
                .GetByIdAsync(_command.CategoryId, Arg.Any<CancellationToken>())
                .Returns(existingCategory);

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
        public async Task Handle_Should_NotCallUpdate_WhenCategoryDoesNotExist()
        {
            // Arrange
            _categoryRepositoryMock
                .GetByIdAsync(_command.CategoryId, Arg.Any<CancellationToken>())
                .Returns((Category?)null);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _categoryRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<Category>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenNameIsDuplicated()
        {
            // Arrange
            Category categoryToUpdate = new()
            {
                Id = _command.CategoryId,
                Name = "Electronics"
            };

            Category existingCategory = new()
            {
                Id = 2,
                Name = _command.Name
            };

            _categoryRepositoryMock
                .GetByIdAsync(_command.CategoryId, Arg.Any<CancellationToken>())
                .Returns(categoryToUpdate);

            _categoryRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns(existingCategory);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _categoryRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<Category>());
        }
    }
}

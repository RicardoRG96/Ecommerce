using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.Products.Common.Services;
using Application.Products.Products.Create;
using Domain.Entities.Products;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Products.Products.Create
{
    public class CreateProductCommandTests
    {
        private const long BrandId = 1;
        private const long CategoryId = 2;
        private const long ProductTaxCategoryId = 3;
        private const string ProductName = "iPhone 15 Pro";
        private const string ExpectedSlug = "iphone-15-pro";

        private static readonly CreateProductCommand _command = new(
            Name: ProductName,
            Description: "The latest flagship smartphone from Apple",
            ShortDescription: "Premium smartphone with advanced features",
            BrandId: BrandId,
            CategoryId: CategoryId,
            ProductTaxCategoryId: ProductTaxCategoryId,
            IsActive: true,
            IsFeatured: true,
            IsDigital: false,
            MetaTitle: "iPhone 15 Pro - Buy Now",
            MetaDescription: "Get the latest iPhone 15 Pro with amazing features",
            MetaKeywords: "iphone, apple, smartphone, mobile");

        private readonly CreateProductCommandHandler _handler;
        private readonly IProductRepository _productRepositoryMock;
        private readonly IProductRelatedEntitiesValidator _validatorMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public CreateProductCommandTests()
        {
            _productRepositoryMock = Substitute.For<IProductRepository>();
            _validatorMock = Substitute.For<IProductRelatedEntitiesValidator>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _handler = new(
                _productRepositoryMock, 
                _validatorMock, 
                _unitOfWorkMock);
        }

        #region Helper Methods

        private void SetupSuccessfulValidation()
        {
            _validatorMock
                .ValidateProductNameUniquenessAsync(
                    _command.Name, 
                    null, 
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateRelatedEntitiesAsync(
                    _command.BrandId,
                    _command.CategoryId,
                    _command.ProductTaxCategoryId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());
        }

        private void SetupNameValidationFailure(Error error)
        {
            _validatorMock
                .ValidateProductNameUniquenessAsync(
                    _command.Name, 
                    null, 
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        private void SetupRelatedEntitiesValidationFailure(Error error)
        {
            _validatorMock
                .ValidateProductNameUniquenessAsync(
                    _command.Name, 
                    null, 
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateRelatedEntitiesAsync(
                    _command.BrandId,
                    _command.CategoryId,
                    _command.ProductTaxCategoryId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        #endregion

        #region Success Scenarios

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenProductIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            Result<long> result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_ReturnProductId_WhenProductIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            Result<long> result = await _handler.Handle(_command, default);

            // Assert
            result.Value.Should().BeGreaterThanOrEqualTo(0);
        }

        [Fact]
        public async Task Handle_Should_GenerateSlug_WhenProductIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productRepositoryMock
                .Received(1)
                .AddAsync(
                    Arg.Is<Product>(p => p.Slug == ExpectedSlug), 
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateProductWithCorrectBasicProperties_WhenProductIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<Product>(p =>
                    p.Name == _command.Name &&
                    p.Description == _command.Description &&
                    p.ShortDescription == _command.ShortDescription),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateProductWithCorrectRelationships_WhenProductIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<Product>(p =>
                    p.BrandId == BrandId &&
                    p.CategoryId == CategoryId &&
                    p.ProductTaxCategoryId == ProductTaxCategoryId),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateProductWithCorrectFlags_WhenProductIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<Product>(p =>
                    p.IsActive == _command.IsActive &&
                    p.IsFeatured == _command.IsFeatured &&
                    p.IsDigital == _command.IsDigital &&
                    p.IsPublished == false),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CreateProductWithCorrectSeoProperties_WhenProductIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<Product>(p =>
                    p.MetaTitle == _command.MetaTitle &&
                    p.MetaDescription == _command.MetaDescription &&
                    p.MetaKeywords == _command.MetaKeywords),
                    Arg.Any<CancellationToken>());
        }

        #endregion

        #region Validation Scenarios

        [Fact]
        public async Task Handle_Should_CallValidateProductNameUniquenessAsync_Always()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateProductNameUniquenessAsync(
                    ProductName, 
                    null, 
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallValidateRelatedEntitiesAsync_WhenProductNameIsUnique()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateRelatedEntitiesAsync(
                    BrandId,
                    CategoryId,
                    ProductTaxCategoryId,
                    Arg.Any<CancellationToken>());
        }

        #endregion

        #region Name Validation Failure Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenProductNameAlreadyExists()
        {
            // Arrange
            SetupNameValidationFailure(ProductErrors.DuplicatedProductName);

            // Act
            Result<long> result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductErrors.DuplicatedProductName);
        }

        [Fact]
        public async Task Handle_Should_NotCallValidateRelatedEntitiesAsync_WhenProductNameAlreadyExists()
        {
            // Arrange
            SetupNameValidationFailure(ProductErrors.DuplicatedProductName);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .DidNotReceive()
                .ValidateRelatedEntitiesAsync(
                    Arg.Any<long>(),
                    Arg.Any<long>(),
                    Arg.Any<long>(),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallAddAsync_WhenProductNameAlreadyExists()
        {
            // Arrange
            SetupNameValidationFailure(ProductErrors.DuplicatedProductName);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productRepositoryMock
                .DidNotReceive()
                .AddAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUnitOfWork_WhenProductNameAlreadyExists()
        {
            // Arrange
            SetupNameValidationFailure(ProductErrors.DuplicatedProductName);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region Related Entities Validation Failure Scenarios

        [Theory]
        [InlineData(nameof(ProductErrors.BrandNotActive))]
        [InlineData(nameof(ProductErrors.CategoryNotActive))]
        [InlineData(nameof(ProductErrors.ProductTaxCategoryNotActive))]
        public async Task Handle_Should_ReturnFailure_WhenRelatedEntityValidationFails(string errorPropertyName)
        {
            // Arrange
            var error = GetErrorByPropertyName(errorPropertyName);
            SetupRelatedEntitiesValidationFailure(error);

            // Act
            Result<long> result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(error);
        }

        [Theory]
        [InlineData(nameof(ProductErrors.BrandNotActive))]
        [InlineData(nameof(ProductErrors.CategoryNotActive))]
        [InlineData(nameof(ProductErrors.ProductTaxCategoryNotActive))]
        public async Task Handle_Should_NotCallAddAsync_WhenRelatedEntityValidationFails(string errorPropertyName)
        {
            // Arrange
            var error = GetErrorByPropertyName(errorPropertyName);
            SetupRelatedEntitiesValidationFailure(error);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productRepositoryMock
                .DidNotReceive()
                .AddAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        }

        [Theory]
        [InlineData(nameof(ProductErrors.BrandNotActive))]
        [InlineData(nameof(ProductErrors.CategoryNotActive))]
        [InlineData(nameof(ProductErrors.ProductTaxCategoryNotActive))]
        public async Task Handle_Should_NotCallUnitOfWork_WhenRelatedEntityValidationFails(string errorPropertyName)
        {
            // Arrange
            var error = GetErrorByPropertyName(errorPropertyName);
            SetupRelatedEntitiesValidationFailure(error);

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
        public async Task Handle_Should_CallAddAsync_ExactlyOnce_WhenProductIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _productRepositoryMock
                .Received(1)
                .AddAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_ExactlyOnce_WhenProductIsValid()
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
        public async Task Handle_Should_CallSaveChangesAsync_AfterAddAsync_WhenProductIsValid()
        {
            // Arrange
            SetupSuccessfulValidation();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(async () =>
            {
                await _productRepositoryMock.AddAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
                await _unitOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>());
            });
        }

        #endregion

        #region Helper Methods for Tests

        private static Error GetErrorByPropertyName(string propertyName) => propertyName switch
        {
            nameof(ProductErrors.BrandNotActive) => ProductErrors.BrandNotActive,
            nameof(ProductErrors.CategoryNotActive) => ProductErrors.CategoryNotActive,
            nameof(ProductErrors.ProductTaxCategoryNotActive) => ProductErrors.ProductTaxCategoryNotActive,
            _ => throw new ArgumentException($"Unknown error property: {propertyName}")
        };

        #endregion
    }
}

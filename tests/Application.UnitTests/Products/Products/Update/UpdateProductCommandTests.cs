using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.Products.Common.Services;
using Application.Products.Products.Update;
using Domain.Entities.Products;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Products.Products.Update
{
    public class UpdateProductCommandTests
    {
        private const long ProductId = 1;
        private const long BrandId = 10;
        private const long CategoryId = 20;
        private const long ProductTaxCategoryId = 30;
        private const string ProductName = "iPhone 15 Pro Max";
        private const string OriginalProductName = "iPhone 15 Pro";
        private const string OriginalSlug = "iphone-15-pro";
        private const string ExpectedNewSlug = "iphone-15-pro-max";

        private static readonly UpdateProductCommand _command = new(
            ProductId: ProductId,
            Name: ProductName,
            Description: "The latest and greatest flagship smartphone",
            ShortDescription: "Premium smartphone with advanced camera",
            BrandId: BrandId,
            CategoryId: CategoryId,
            ProductTaxCategoryId: ProductTaxCategoryId,
            IsFeatured: true,
            IsDigital: false,
            MetaTitle: "iPhone 15 Pro Max - Buy Now",
            MetaDescription: "Get the latest iPhone 15 Pro Max with cutting-edge features",
            MetaKeywords: "iphone, apple, smartphone, pro max");

        private readonly UpdateProductCommandHandler _handler;
        private readonly IProductRepository _productRepositoryMock;
        private readonly IProductRelatedEntitiesValidator _validatorMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public UpdateProductCommandTests()
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

        private Product CreateExistingProduct()
        {
            return new Product
            {
                Id = ProductId,
                Name = OriginalProductName,
                Slug = OriginalSlug,
                Description = "Original description",
                ShortDescription = "Original short description",
                BrandId = 5,
                CategoryId = 15,
                ProductTaxCategoryId = 25,
                IsActive = true,
                IsFeatured = false,
                IsDigital = false,
                MetaTitle = "Original Meta Title",
                MetaDescription = "Original Meta Description",
                MetaKeywords = "original, keywords"
            };
        }

        private void SetupSuccessfulScenario(Product product, bool nameChanged = true)
        {
            _productRepositoryMock
                .GetByIdAsync(ProductId, Arg.Any<CancellationToken>())
                .Returns(product);

            _validatorMock
                .ValidateProductNameUniquenessAsync(
                    _command.Name,
                    ProductId,
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

        private void SetupProductNotFound()
        {
            _productRepositoryMock
                .GetByIdAsync(ProductId, Arg.Any<CancellationToken>())
                .Returns((Product?)null);
        }

        private void SetupNameValidationFailure(Product product, Error error)
        {
            _productRepositoryMock
                .GetByIdAsync(ProductId, Arg.Any<CancellationToken>())
                .Returns(product);

            _validatorMock
                .ValidateProductNameUniquenessAsync(
                    _command.Name,
                    ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Failure(error));
        }

        private void SetupRelatedEntitiesValidationFailure(Product product, Error error)
        {
            _productRepositoryMock
                .GetByIdAsync(ProductId, Arg.Any<CancellationToken>())
                .Returns(product);

            _validatorMock
                .ValidateProductNameUniquenessAsync(
                    _command.Name,
                    ProductId,
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
        public async Task Handle_Should_ReturnSuccess_WhenProductExistsAndAllValidationsPass()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            SetupSuccessfulScenario(existingProduct);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_UpdateBasicProperties_WhenProductIsValid()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            SetupSuccessfulScenario(existingProduct);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingProduct.Name.Should().Be(_command.Name);
            existingProduct.Description.Should().Be(_command.Description);
            existingProduct.ShortDescription.Should().Be(_command.ShortDescription);
        }

        [Fact]
        public async Task Handle_Should_UpdateRelationships_WhenProductIsValid()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            SetupSuccessfulScenario(existingProduct);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingProduct.BrandId.Should().Be(BrandId);
            existingProduct.CategoryId.Should().Be(CategoryId);
            existingProduct.ProductTaxCategoryId.Should().Be(ProductTaxCategoryId);
        }

        [Fact]
        public async Task Handle_Should_UpdateFlags_WhenProductIsValid()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            SetupSuccessfulScenario(existingProduct);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingProduct.IsFeatured.Should().Be(_command.IsFeatured);
            existingProduct.IsDigital.Should().Be(_command.IsDigital);
        }

        [Fact]
        public async Task Handle_Should_UpdateSeoProperties_WhenProductIsValid()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            SetupSuccessfulScenario(existingProduct);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingProduct.MetaTitle.Should().Be(_command.MetaTitle);
            existingProduct.MetaDescription.Should().Be(_command.MetaDescription);
            existingProduct.MetaKeywords.Should().Be(_command.MetaKeywords);
        }

        [Fact]
        public async Task Handle_Should_RegenerateSlug_WhenNameChanges()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            SetupSuccessfulScenario(existingProduct);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingProduct.Slug.Should().Be(ExpectedNewSlug);
        }

        [Fact]
        public async Task Handle_Should_NotRegenerateSlug_WhenNameDoesNotChange()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            var commandWithSameName = _command with { Name = OriginalProductName };
            
            _productRepositoryMock
                .GetByIdAsync(ProductId, Arg.Any<CancellationToken>())
                .Returns(existingProduct);

            _validatorMock
                .ValidateProductNameUniquenessAsync(
                    commandWithSameName.Name,
                    ProductId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            _validatorMock
                .ValidateRelatedEntitiesAsync(
                    commandWithSameName.BrandId,
                    commandWithSameName.CategoryId,
                    commandWithSameName.ProductTaxCategoryId,
                    Arg.Any<CancellationToken>())
                .Returns(Result.Success());

            // Act
            await _handler.Handle(commandWithSameName, default);

            // Assert
            existingProduct.Slug.Should().Be(OriginalSlug);
        }

        [Fact]
        public async Task Handle_Should_AllowSameName_WhenUpdatingSameProduct()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            existingProduct.Name = _command.Name;
            
            SetupSuccessfulScenario(existingProduct, nameChanged: false);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        #endregion

        #region Product Not Found Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenProductDoesNotExist()
        {
            // Arrange
            SetupProductNotFound();

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductErrors.NotFound(ProductId));
        }

        [Fact]
        public async Task Handle_Should_NotCallValidateProductNameUniquenessAsync_WhenProductDoesNotExist()
        {
            // Arrange
            SetupProductNotFound();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .DidNotReceive()
                .ValidateProductNameUniquenessAsync(
                    Arg.Any<string>(),
                    Arg.Any<long>(),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallValidateRelatedEntitiesAsync_WhenProductDoesNotExist()
        {
            // Arrange
            SetupProductNotFound();

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
        public async Task Handle_Should_NotCallUpdate_WhenProductDoesNotExist()
        {
            // Arrange
            SetupProductNotFound();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<Product>());
        }

        [Fact]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenProductDoesNotExist()
        {
            // Arrange
            SetupProductNotFound();

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region Name Validation Failure Scenarios

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenProductNameAlreadyExists()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            SetupNameValidationFailure(existingProduct, ProductErrors.DuplicatedProductName);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductErrors.DuplicatedProductName);
        }

        [Fact]
        public async Task Handle_Should_NotCallValidateRelatedEntitiesAsync_WhenProductNameAlreadyExists()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            SetupNameValidationFailure(existingProduct, ProductErrors.DuplicatedProductName);

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
        public async Task Handle_Should_NotCallUpdate_WhenProductNameAlreadyExists()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            SetupNameValidationFailure(existingProduct, ProductErrors.DuplicatedProductName);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<Product>());
        }

        [Fact]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenProductNameAlreadyExists()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            SetupNameValidationFailure(existingProduct, ProductErrors.DuplicatedProductName);

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
            Product existingProduct = CreateExistingProduct();
            var error = GetErrorByPropertyName(errorPropertyName);
            SetupRelatedEntitiesValidationFailure(existingProduct, error);

            // Act
            Result result = await _handler.Handle(_command, default);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(error);
        }

        [Theory]
        [InlineData(nameof(ProductErrors.BrandNotActive))]
        [InlineData(nameof(ProductErrors.CategoryNotActive))]
        [InlineData(nameof(ProductErrors.ProductTaxCategoryNotActive))]
        public async Task Handle_Should_NotCallUpdate_WhenRelatedEntityValidationFails(string errorPropertyName)
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            var error = GetErrorByPropertyName(errorPropertyName);
            SetupRelatedEntitiesValidationFailure(existingProduct, error);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<Product>());
        }

        [Theory]
        [InlineData(nameof(ProductErrors.BrandNotActive))]
        [InlineData(nameof(ProductErrors.CategoryNotActive))]
        [InlineData(nameof(ProductErrors.ProductTaxCategoryNotActive))]
        public async Task Handle_Should_NotCallSaveChangesAsync_WhenRelatedEntityValidationFails(string errorPropertyName)
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            var error = GetErrorByPropertyName(errorPropertyName);
            SetupRelatedEntitiesValidationFailure(existingProduct, error);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        #endregion

        #region Validation Call Verification

        [Fact]
        public async Task Handle_Should_CallValidateProductNameUniquenessAsync_WithCorrectParameters()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            SetupSuccessfulScenario(existingProduct);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _validatorMock
                .Received(1)
                .ValidateProductNameUniquenessAsync(
                    ProductName,
                    ProductId,
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallValidateRelatedEntitiesAsync_WithCorrectParameters()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            SetupSuccessfulScenario(existingProduct);

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

        #region Repository and UnitOfWork Interactions

        [Fact]
        public async Task Handle_Should_CallUpdate_ExactlyOnce_WhenProductIsValid()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            SetupSuccessfulScenario(existingProduct);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productRepositoryMock
                .Received(1)
                .Update(existingProduct);
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_ExactlyOnce_WhenProductIsValid()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            SetupSuccessfulScenario(existingProduct);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallSaveChangesAsync_AfterUpdate_WhenProductIsValid()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            SetupSuccessfulScenario(existingProduct);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            Received.InOrder(() =>
            {
                _productRepositoryMock.Update(existingProduct);
                _unitOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>());
            });
        }

        [Fact]
        public async Task Handle_Should_CallUpdate_WithCorrectProduct()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            SetupSuccessfulScenario(existingProduct);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            _productRepositoryMock
                .Received(1)
                .Update(Arg.Is<Product>(p =>
                    p.Id == ProductId &&
                    p.Name == _command.Name &&
                    p.Description == _command.Description &&
                    p.ShortDescription == _command.ShortDescription &&
                    p.BrandId == _command.BrandId &&
                    p.CategoryId == _command.CategoryId &&
                    p.ProductTaxCategoryId == _command.ProductTaxCategoryId &&
                    p.IsFeatured == _command.IsFeatured &&
                    p.IsDigital == _command.IsDigital &&
                    p.MetaTitle == _command.MetaTitle &&
                    p.MetaDescription == _command.MetaDescription &&
                    p.MetaKeywords == _command.MetaKeywords));
        }

        #endregion

        #region Property Preservation Tests

        [Fact]
        public async Task Handle_Should_PreserveIsActiveProperty_WhenUpdating()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            existingProduct.IsActive = true;
            SetupSuccessfulScenario(existingProduct);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingProduct.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_PreserveIsPublishedProperty_WhenUpdating()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            existingProduct.IsPublished = true;
            SetupSuccessfulScenario(existingProduct);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingProduct.IsPublished.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_PreserveProductId_WhenUpdating()
        {
            // Arrange
            Product existingProduct = CreateExistingProduct();
            SetupSuccessfulScenario(existingProduct);

            // Act
            await _handler.Handle(_command, default);

            // Assert
            existingProduct.Id.Should().Be(ProductId);
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

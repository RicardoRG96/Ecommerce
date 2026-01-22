using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.Brands.Update;
using Domain.Entities.Products;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Products.Brands.Update
{
    public class UpdateBrandCommandTests
    {
        private static readonly UpdateBrandCommand _command = new(
            BrandId: 1,
            Name: "Adidas",
            Description: "Impossible is Nothing - Leading athletic footwear and apparel brand",
            LogoUrl: "https://example.com/logos/adidas.png",
            BannerUrl: "https://example.com/banners/adidas-banner.jpg",
            WebsiteUrl: "https://www.adidas.com",
            IsActive: true,
            IsFeatured: true,
            DisplayOrder: 2,
            MetaTitle: "Adidas - Athletic Shoes & Apparel",
            MetaDescription: "Shop the latest Adidas shoes, clothing and accessories",
            MetaKeywords: "adidas, shoes, athletic, sportswear, sneakers");

        private readonly UpdateBrandCommandHandler _handler;
        private readonly IBrandRepository _brandRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public UpdateBrandCommandTests()
        {
            _brandRepositoryMock = Substitute.For<IBrandRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _handler = new(_brandRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenBrandExistsAndNameIsDifferent()
        {
            Brand existingBrand = new()
            {
                Id = _command.BrandId,
                Name = "Nike",
                Description = "Old Description",
                IsActive = false
            };

            _brandRepositoryMock
                .GetByIdAsync(_command.BrandId, Arg.Any<CancellationToken>())
                .Returns(existingBrand);

            _brandRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns((Brand?)null);

            Result result = await _handler.Handle(_command, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenBrandDoesNotExist()
        {
            _brandRepositoryMock
                .GetByIdAsync(_command.BrandId, Arg.Any<CancellationToken>())
                .Returns((Brand?)null);

            Result result = await _handler.Handle(_command, default);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(BrandErrors.NotFound(_command.BrandId));
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenNameIsDuplicated()
        {
            Brand brandToUpdate = new()
            {
                Id = _command.BrandId,
                Name = _command.Name
            };

            Brand existingBrand = new()
            {
                Id = 2,
                Name = _command.Name
            };

            _brandRepositoryMock
                .GetByIdAsync(_command.BrandId, Arg.Any<CancellationToken>())
                .Returns(brandToUpdate);

            _brandRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns(existingBrand);

            Result result = await _handler.Handle(_command, default);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(BrandErrors.DuplicatedBrandName);
        }

        [Fact]
        public async Task Handle_Should_UpdateBrandProperties_WhenBrandIsValid()
        {
            Brand existingBrand = new()
            {
                Id = _command.BrandId,
                Name = "Nike",
                Description = "Old Description",
                LogoUrl = "old-logo.png",
                IsActive = false,
                IsFeatured = false,
                DisplayOrder = 10
            };

            _brandRepositoryMock
                .GetByIdAsync(_command.BrandId, Arg.Any<CancellationToken>())
                .Returns(existingBrand);

            await _handler.Handle(_command, default);

            existingBrand.Name.Should().Be(_command.Name);
            existingBrand.Description.Should().Be(_command.Description);
            existingBrand.LogoUrl.Should().Be(_command.LogoUrl);
            existingBrand.BannerUrl.Should().Be(_command.BannerUrl);
            existingBrand.WebsiteUrl.Should().Be(_command.WebsiteUrl);
            existingBrand.IsActive.Should().Be(_command.IsActive);
            existingBrand.IsFeatured.Should().Be(_command.IsFeatured);
            existingBrand.DisplayOrder.Should().Be(_command.DisplayOrder);
            existingBrand.MetaTitle.Should().Be(_command.MetaTitle);
            existingBrand.MetaDescription.Should().Be(_command.MetaDescription);
            existingBrand.MetaKeywords.Should().Be(_command.MetaKeywords);
        }

        [Fact]
        public async Task Handle_Should_CallUpdate_WhenBrandIsValid()
        {
            Brand existingBrand = new()
            {
                Id = _command.BrandId,
                Name = "Nike"
            };

            _brandRepositoryMock
                .GetByIdAsync(_command.BrandId, Arg.Any<CancellationToken>())
                .Returns(existingBrand);

            _brandRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns((Brand?)null);

            await _handler.Handle(_command, default);

            _brandRepositoryMock
                .Received(1)
                .Update(Arg.Is<Brand>(b =>
                    b.Id == _command.BrandId &&
                    b.Name == _command.Name &&
                    b.Description == _command.Description));
        }

        [Fact]
        public async Task Handle_Should_CallUnitOfWork_WhenBrandIsValid()
        {
            Brand existingBrand = new()
            {
                Id = _command.BrandId,
                Name = "Nike"
            };

            _brandRepositoryMock
                .GetByIdAsync(_command.BrandId, Arg.Any<CancellationToken>())
                .Returns(existingBrand);

            _brandRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns((Brand?)null);

            await _handler.Handle(_command, default);

            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenBrandDoesNotExist()
        {
            _brandRepositoryMock
                .GetByIdAsync(_command.BrandId, Arg.Any<CancellationToken>())
                .Returns((Brand?)null);
            
            await _handler.Handle(_command, default);

            _brandRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<Brand>());
        }

        [Fact]
        public async Task Handle_Should_NotCallUpdate_WhenNameIsDuplicated()
        {
            Brand brandToUpdate = new()
            {
                Id = _command.BrandId,
                Name = _command.Name
            };

            Brand existingBrand = new()
            {
                Id = 2,
                Name = _command.Name
            };

            _brandRepositoryMock
                .GetByIdAsync(_command.BrandId, Arg.Any<CancellationToken>())
                .Returns(brandToUpdate);

            _brandRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns(existingBrand);

            await _handler.Handle(_command, default);

            _brandRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<Brand>());
        }
    }
}

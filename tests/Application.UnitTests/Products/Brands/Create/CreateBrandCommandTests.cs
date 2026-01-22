using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.Brands.Create;
using Domain.Entities.Products;
using Domain.Errors.Products;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Products.Brands.Create
{
    public class CreateBrandCommandTests
    {
        private static readonly CreateBrandCommand _command = new(
            Name: "Nike",
            Description: "Just Do It - Leading athletic footwear and apparel brand",
            LogoUrl: "https://example.com/logos/nike.png",
            BannerUrl: "https://example.com/banners/nike-banner.jpg",
            WebsiteUrl: "https://www.nike.com",
            IsActive: true,
            IsFeatured: true,
            DisplayOrder: 1,
            MetaTitle: "Nike - Athletic Shoes & Apparel",
            MetaDescription: "Shop the latest Nike shoes, clothing and accessories",
            MetaKeywords: "nike, shoes, athletic, sportswear, sneakers");

        private readonly CreateBrandCommandHandler _handler;
        private readonly IBrandRepository _brandRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public CreateBrandCommandTests()
        {
            _brandRepositoryMock = Substitute.For<IBrandRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _handler = new(_brandRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenBrandIsValid()
        {
            _brandRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns((Brand?)null);

            Result<long> result = await _handler.Handle(_command, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenBrandNameAlreadyExists()
        {
            Brand existingBrand = new()
            {
                Id = 1,
                Name = _command.Name
            };

            _brandRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns(existingBrand);

            Result<long> result = await _handler.Handle(_command, default);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(BrandErrors.DuplicatedBrandName);
        }

        [Fact]
        public async Task Handle_Should_CallAddAsync_WhenBrandIsValid()
        {
            _brandRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns((Brand?)null);

            await _handler.Handle(_command, default);

            await _brandRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<Brand>(b =>
                    b.Name == _command.Name &&
                    b.Description == _command.Description &&
                    b.IsActive == _command.IsActive &&
                    b.IsFeatured == _command.IsFeatured),
                    Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallUnitOfWork_WhenBrandIsValid()
        {
            _brandRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns((Brand?)null);

            await _handler.Handle(_command, default);

            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_NotCallRepository_WhenBrandNameAlreadyExists()
        {
            Brand existingBrand = new()
            {
                Id = 1,
                Name = _command.Name
            };

            _brandRepositoryMock
                .GetByNameAsync(_command.Name, Arg.Any<CancellationToken>())
                .Returns(existingBrand);

            await _handler.Handle(_command, default);

            await _brandRepositoryMock
                .DidNotReceive()
                .AddAsync(Arg.Any<Brand>(), Arg.Any<CancellationToken>());
        }
    }
}

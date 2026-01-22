using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.Brands.Deactivate;
using Domain.Entities.Products;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Products.Brands.Deactivate
{
    public class DeactivateBrandCommandTests
    {
        private static readonly DeactivateBrandCommand _command = new(1);
        private readonly DeactivateBrandCommandHandler _handler;
        private readonly IBrandRepository _brandRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public DeactivateBrandCommandTests()
        {
            _brandRepositoryMock = Substitute.For<IBrandRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _handler = new(_brandRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenBrandIdDoesNotExist()
        {
            long notExistingBrandId = 2500;
            DeactivateBrandCommand invalidCommand = _command with { BrandId = notExistingBrandId };

            _brandRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == invalidCommand.BrandId), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result result = await _handler.Handle(invalidCommand, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_DeactivateBrand_WhenBrandIsActive()
        {
            Brand brand = new()
            {
                Id = _command.BrandId,
                IsActive = true
            };

            _brandRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.BrandId), Arg.Any<CancellationToken>())
                .Returns(brand);

            Result result = await _handler.Handle(_command, default);

            result.IsSuccess.Should().BeTrue();
            brand.IsActive.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldCallRepositoryWhenBrandIsActive()
        {
            Brand brand = new()
            {
                Id = _command.BrandId,
                IsActive = true
            };

            _brandRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.BrandId), Arg.Any<CancellationToken>())
                .Returns(brand);

            await _handler.Handle(_command, default);

            _brandRepositoryMock
                .Received(1)
                .Update(Arg.Is<Brand>(b => b.Id == _command.BrandId && b.IsActive == false));
        }

        [Fact]
        public async Task Handle_ShouldCallUnitOfWork_WhenBrandIsActive()
        {
            Brand brand = new()
            {
                Id = _command.BrandId,
                IsActive = true
            };

            _brandRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.BrandId), Arg.Any<CancellationToken>())
                .Returns(brand);

            await _handler.Handle(_command, default);

            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_DoNothing_WhenBrandIsAlreadyInactive()
        {
            Brand brand = new()
            {
                Id = _command.BrandId,
                IsActive = false
            };

            _brandRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.BrandId), Arg.Any<CancellationToken>())
                .Returns(brand);

            Result result = await _handler.Handle(_command, default);

            result.IsSuccess.Should().BeTrue();

            _brandRepositoryMock
                .DidNotReceive()
                .Update(Arg.Any<Brand>());

            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}

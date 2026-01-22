using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.Brands.Deactivate;
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
    }
}

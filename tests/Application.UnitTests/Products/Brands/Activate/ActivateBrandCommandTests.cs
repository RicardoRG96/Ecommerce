using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Products.Brands.Activate;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Products.Brands.Activate
{
    public class ActivateBrandCommandTests
    {
        private static readonly ActivateBrandCommand _command = new(1);
        private readonly ActivateBrandCommandHandler _handler;
        private readonly IBrandRepository _brandRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public ActivateBrandCommandTests()
        {
            _brandRepositoryMock = Substitute.For<IBrandRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _handler = new(_brandRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenBrandIdDoesNotExist()
        {
            long notExistingBrandId = 2500;
            ActivateBrandCommand invalidCommand = _command with { BrandId = notExistingBrandId };


            _brandRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == invalidCommand.BrandId), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result result = await _handler.Handle(invalidCommand, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
        }
    }
}

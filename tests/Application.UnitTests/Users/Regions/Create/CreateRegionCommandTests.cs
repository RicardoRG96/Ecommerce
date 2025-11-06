using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Users.Regions.Create;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Users.Regions.Create
{
    public class CreateRegionCommandTests
    {
        private static readonly CreateRegionCommand _command = new("testRegion");
        private readonly CreateRegionCommandHandler _handler;
        private readonly IRegionRepository _regionRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public CreateRegionCommandTests()
        {
            _regionRepositoryMock = Substitute.For<IRegionRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _handler = new CreateRegionCommandHandler(_regionRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenRegionNameIsPresent()
        {
            Result<long> result = await _handler.Handle(_command, default);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_CallUnitOfWork_WhenRegionNameIsPresent()
        {
            await _handler.Handle(_command, default);

            await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}

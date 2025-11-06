using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Users.Regions.Delete;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Regions.Delete
{
    public class DeleteRegionCommandTests
    {
        private static readonly DeleteRegionCommand _command = new(1L);
        private readonly Region _region;
        private readonly DeleteRegionCommandHandler _handler;
        private readonly IRegionRepository _regionRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public DeleteRegionCommandTests()
        {
            _regionRepositoryMock = Substitute.For<IRegionRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _region = new Region { RegionId = _command.RegionId, Name = "TestRegion" };
            _handler = new DeleteRegionCommandHandler(_regionRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenRegionDoesNotExist()
        {
            long notExistingRegionId = 2500L;
            DeleteRegionCommand invalidCommand = _command with { RegionId = notExistingRegionId };

            _regionRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == notExistingRegionId), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result result = await _handler.Handle(invalidCommand, default);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(RegionErrors.NotFound(notExistingRegionId));
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenRegionExists()
        {
            _regionRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.RegionId), Arg.Any<CancellationToken>())
                .Returns(_region);

            Result result = await _handler.Handle(_command, default);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_CallRepository_WhenRegionExists()
        {
            _regionRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.RegionId), Arg.Any<CancellationToken>())
                .Returns(_region);

            await _handler.Handle(_command, default);

            _regionRepositoryMock
                .Received(1)
                .Delete(_region);
        }
    }
}

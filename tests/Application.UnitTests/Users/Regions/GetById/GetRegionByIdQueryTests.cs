using Application.Abstractions.Data.Repositories.Users;
using Application.Users.Regions.GetById;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Regions.GetById
{
    public class GetRegionByIdQueryTests
    {
        private static readonly GetRegionByIdQuery _query = new(1L);
        private readonly Region _region;
        private readonly GetRegionByIdQueryHandler _handler;
        private readonly IRegionRepository _regionRepositoryMock;

        public GetRegionByIdQueryTests()
        {
            _regionRepositoryMock = Substitute.For<IRegionRepository>();

            _region = new Region { RegionId = _query.RegionId, Name = "TestRegion" };
            _handler = new(_regionRepositoryMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenRegionDoesNotExist()
        {
            long notExistingRegionId = 2500L;
            GetRegionByIdQuery invalidQuery = _query with { RegionId = notExistingRegionId };

            _regionRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == invalidQuery.RegionId), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result result = await _handler.Handle(invalidQuery, default);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(RegionErrors.NotFound(notExistingRegionId));
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenRegionExists()
        {
            _regionRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _query.RegionId), Arg.Any<CancellationToken>())
                .Returns(_region);

            Result<RegionResponse> result = await _handler.Handle(_query, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.Value.Name.Should().Be(_region.Name);
        }
    }
}

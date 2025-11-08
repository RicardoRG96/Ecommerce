using Application.Abstractions.Data.Repositories.Users;
using Application.Users.Regions.GetWithPagination;
using Domain.Entities.Users;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Users.Regions.GetWithPagination
{
    public class GetRegionsWithPaginationQueryTests
    {
        private static readonly GetRegionsWithPaginationQuery _query = new(1, 2);
        private readonly PaginatedList<Region> _regions;
        private List<Region>? _items;
        private readonly GetRegionsWithPaginationQueryHandler _handler;
        private readonly IRegionRepository _regionRepositoryMock;

        public GetRegionsWithPaginationQueryTests()
        {
            _regionRepositoryMock = Substitute.For<IRegionRepository>();

            CreateRegionItems();
            _regions = PaginatedList<Region>.Create(_items!, _items!.Count, _query.PageNumber, _query.PageSize);
            _handler = new(_regionRepositoryMock);
        }

        private void CreateRegionItems()
        {
            _items = new List<Region>
            {
                new() { RegionId = 1L, Name = "TestRegion1" },
                new() { RegionId = 2L, Name = "TestRegion2" },
                new() { RegionId = 3L, Name = "TestRegion3" }
            };
        }

        [Fact]
        public async Task Handle_Should_ReturnAnEmptyList_WhenThereAreNoRegions()
        {
            PaginatedList<Region> emptyPaginatedList = PaginatedList<Region>.Create([], 0, 1, 3);

            _regionRepositoryMock
                .GetAllAsync(
                    Arg.Is<int>(pn => pn == _query.PageNumber),
                    Arg.Is<int>(ps => ps == _query.PageSize),
                    Arg.Any<CancellationToken>())
                .Returns(emptyPaginatedList);

            Result<PaginatedList<RegionResponse>> result = await _handler.Handle(_query, default);

            result.Value.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_Should_ReturnAListWithElements_WhenThereAreRegions()
        {
            _regionRepositoryMock
                .GetAllAsync(
                    Arg.Is<int>(pn => pn == _query.PageNumber),
                    Arg.Is<int>(ps => ps == _query.PageSize),
                    Arg.Any<CancellationToken>())
                .Returns(_regions);

            Result<PaginatedList<RegionResponse>> result = await _handler.Handle(_query, default);

            result.Value.Items.Count.Should().Be(_items!.Count);
        }
    }
}

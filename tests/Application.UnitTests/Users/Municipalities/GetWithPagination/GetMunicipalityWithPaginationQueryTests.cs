using Application.Abstractions.Data.Repositories.Users;
using Application.Users.Municipalities.GetWithPagination;
using Domain.Entities.Users;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Users.Municipalities.GetWithPagination
{
    public class GetMunicipalityWithPaginationQueryTests
    {
        private static readonly GetMunicipalitiesWithPaginationQuery _query = new(1, 2);
        private readonly PaginatedList<Municipality> _municipalities;
        private List<Municipality>? _items;
        private readonly GetMunicipalitiesWithPaginationQueryHandler _handler;
        private readonly IMunicipalityRepository _municipalityRepositoryMock;

        public GetMunicipalityWithPaginationQueryTests()
        {
            _municipalityRepositoryMock = Substitute.For<IMunicipalityRepository>();

            CreateMunicipalitiesItems();
            _municipalities = PaginatedList<Municipality>.Create(_items!, _items!.Count, _query.PageNumber, _query.PageSize);
            _handler = new(_municipalityRepositoryMock);
        }

        private void CreateMunicipalitiesItems()
        {
            _items = new List<Municipality>
            {
                new() 
                { 
                    MunicipalityId = 1,
                    Name = "TestMunicipality1",
                    RegionId = 1,
                    Region = new() { RegionId = 1, Name = "TestRegion1" } 
                },
                new()
                {
                    MunicipalityId = 2,
                    Name = "TestMunicipality2",
                    RegionId = 2,
                    Region = new() { RegionId = 2, Name = "TestRegion2" }
                },
                new() 
                {
                    MunicipalityId = 3, 
                    Name = "TestMunicipality3", 
                    RegionId = 3, 
                    Region = new() { RegionId = 3, Name = "TestRegion3" }
                }
            };
        }

        [Fact]
        public async Task Handle_Should_ReturnAnEmptyList_WhenThereAreNoMunicipalities()
        {
            PaginatedList<Municipality> emptyPaginatedList = PaginatedList<Municipality>.Create([], 0, 1, 3);

            _municipalityRepositoryMock
                .GetAllAsync(
                    Arg.Is<int>(pn => pn == _query.PageNumber),
                    Arg.Is<int>(ps => ps == _query.PageSize),
                    Arg.Any<CancellationToken>())
                .Returns(emptyPaginatedList);

            Result<PaginatedList<MunicipalityResponse>> result = await _handler.Handle(_query, default);

            result.Value.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_Should_ReturnAListWithElements_WhenThereAreMunicipalities()
        {
            _municipalityRepositoryMock
                .GetAllAsync(
                    Arg.Is<int>(pn => pn == _query.PageNumber),
                    Arg.Is<int>(ps => ps == _query.PageSize),
                    Arg.Any<CancellationToken>())
                .Returns(_municipalities);

            Result<PaginatedList<MunicipalityResponse>> result = await _handler.Handle(_query, default);

            result.Value.Items.Count.Should().Be(_items!.Count);
        }
    }
}

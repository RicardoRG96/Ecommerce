using Application.Abstractions.Data.Repositories.Users;
using Application.Users.Regions.GetByName;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Regions.GetByName
{
    public class GetRegionByNameQueryTests
    {
        private static readonly GetRegionByNameQuery _query = new("TestRegion");
        private readonly Region _region;
        private readonly GetRegionByNameQueryHandler _handler;
        private readonly IRegionRepository _regionRepositoryMock;

        public GetRegionByNameQueryTests()
        {
            _regionRepositoryMock = Substitute.For<IRegionRepository>();

            _region = new Region { RegionId = 1L, Name = _query.Name };
            _handler = new(_regionRepositoryMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenRegionDoesNotExist()
        {
            string notExistingRegionName = "invalidRegionName";
            GetRegionByNameQuery invalidQuery = _query with { Name = notExistingRegionName };

            _regionRepositoryMock
                .GetByNameAsync(Arg.Is<string>(name => name == invalidQuery.Name), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result result = await _handler.Handle(invalidQuery, default);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(RegionErrors.NotFoundByName);
        }
    }
}

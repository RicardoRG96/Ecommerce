using Application.Abstractions.Data.Repositories.Users;
using Application.Users.Municipalities.GetByName;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Municipalities.GetByName
{
    public class GetMunicipalityByNameQueryTests
    {
        private static readonly GetMunicipalityByNameQuery _query = new("TestName");
        private readonly Municipality _municipality;
        private readonly GetMunicipalityByNameQueryHandler _handler;
        private readonly IMunicipalityRepository _municipalityRepositoryMock;

        public GetMunicipalityByNameQueryTests()
        {
            _municipalityRepositoryMock = Substitute.For<IMunicipalityRepository>();

            _municipality = new()
            {
                MunicipalityId = 1,
                Name = _query.Name,
                RegionId = 1,
                Region = new() { RegionId = 1, Name = "TestRegion" }
            };
            _handler = new(_municipalityRepositoryMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenMunicipalityDoesNotExist()
        {
            string notExistingMunicipalityName = "NotExisting";
            GetMunicipalityByNameQuery invalidQuery = _query with { Name =  notExistingMunicipalityName };

            _municipalityRepositoryMock
                .GetByNameAsync(Arg.Is<string>(name => name == invalidQuery.Name), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result result = await _handler.Handle(invalidQuery, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(MunicipalityErrors.NotFoundByName);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenMunicipalityExists()
        {
            _municipalityRepositoryMock
                .GetByNameAsync(Arg.Is<string>(name => name == _query.Name), Arg.Any<CancellationToken>())
                .Returns(_municipality);

            Result result = await _handler.Handle(_query, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }
    }
}

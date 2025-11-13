using Application.Abstractions.Data.Repositories.Users;
using Application.Users.Municipalities.GetById;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Municipalities.GetById
{
    public class GetMunicipalityByIdQueryTests
    {
        private static readonly GetMunicipalityByIdQuery _query = new(1);
        private readonly Municipality _municipality;
        private readonly GetMunicipalityByIdQueryHandler _handler;
        private readonly IMunicipalityRepository _municipalityRepositoryMock;

        public GetMunicipalityByIdQueryTests()
        {
            _municipalityRepositoryMock = Substitute.For<IMunicipalityRepository>();

            _municipality = new() 
            { 
                MunicipalityId = _query.MunicipalityId, 
                Name = "TestName",
                RegionId = 1,
                Region = new() { RegionId = 1, Name = "TestRegion" }
            };
            _handler = new(_municipalityRepositoryMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenMunicipalityDoesNotExist()
        {
            long notExistingMunicipalityId = 2500;
            GetMunicipalityByIdQuery invalidQuery = _query with { MunicipalityId = notExistingMunicipalityId };

            _municipalityRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == invalidQuery.MunicipalityId), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result result = await _handler.Handle(invalidQuery, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(MunicipalityErrors.NotFound(invalidQuery.MunicipalityId));
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenMunicipalityExists()
        {
            _municipalityRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _query.MunicipalityId), Arg.Any<CancellationToken>())
                .Returns(_municipality);

            Result<MunicipalityResponse> result = await _handler.Handle(_query, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }
    }
}

using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Users.Municipalities.Create;
using Application.Users.Municipalities.Update;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Municipalities.Update
{
    public class UpdateMunicipalityCommandTests
    {
        private static readonly UpdateMunicipalityCommand _command = new(1, 1, "UpdatedTestName");
        private readonly Municipality _municipality;
        private readonly Region _region;
        private readonly UpdateMunicipalityCommandHandler _handler;
        private readonly IMunicipalityRepository _municipalityRepositoryMock;
        private readonly IRegionRepository _regionRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public UpdateMunicipalityCommandTests()
        {
            _municipalityRepositoryMock = Substitute.For<IMunicipalityRepository>();
            _regionRepositoryMock = Substitute.For<IRegionRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _region = new() { RegionId = _command.RegionId, Name = "TestRegion" };

            _municipality = new()
            {
                MunicipalityId = _command.MunicipalityId,
                RegionId = _command.RegionId,
                Name = "TestName"
            };
            _handler = new(_municipalityRepositoryMock, _regionRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenMunicipalityIdDoesNotExist()
        {
            long notExistingMunicipalityId = 2500;
            UpdateMunicipalityCommand invalidCommand = _command with { MunicipalityId = notExistingMunicipalityId };

            _municipalityRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == invalidCommand.MunicipalityId), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result result = await _handler.Handle(invalidCommand, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(MunicipalityErrors.NotFound(invalidCommand.MunicipalityId));
        }
    }
}

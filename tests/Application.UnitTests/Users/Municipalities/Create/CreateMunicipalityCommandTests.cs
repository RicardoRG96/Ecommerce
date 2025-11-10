using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Users.Municipalities.Create;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Municipalities.Create
{
    public class CreateMunicipalityCommandTests
    {
        private static readonly CreateMunicipalityCommand _command = new(1, "TestMunicipality");
        private readonly Municipality _municipality;
        private readonly Region _region;
        private readonly CreateMunicipalityCommandHandler _handler;
        private readonly IMunicipalityRepository _municipalityRepositoryMock;
        private readonly IRegionRepository _regionRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public CreateMunicipalityCommandTests()
        {
            _municipalityRepositoryMock = Substitute.For<IMunicipalityRepository>();
            _regionRepositoryMock = Substitute.For<IRegionRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _region = new() { RegionId = _command.RegionId, Name = "TestRegion" };

            _municipality = new() { 
                MunicipalityId = 1, 
                RegionId = _command.RegionId, 
                Name = _command.Name 
            };
            _handler = new(_municipalityRepositoryMock, _regionRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenRegionIdDoesNotExist()
        {
            long notExistingRegionId = 2500;
            CreateMunicipalityCommand invalidCommand = _command with { RegionId = notExistingRegionId };

            _regionRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == invalidCommand.RegionId), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result result = await _handler.Handle(invalidCommand, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RegionErrors.NotFound(invalidCommand.RegionId));
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenRegionIdExists()
        {
            _regionRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.RegionId), Arg.Any<CancellationToken>())
                .Returns(_region);

            Result result = await _handler.Handle(_command, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }
    }
}

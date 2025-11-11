using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
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

        [Fact]
        public async Task Handle_Should_ReturnError_WhenRegionIdDoesNotExist()
        {
            long notExistingRegionId = 2500;
            UpdateMunicipalityCommand invalidCommand = _command with { RegionId = notExistingRegionId };

            _municipalityRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.MunicipalityId), Arg.Any<CancellationToken>())
                .Returns(_municipality);

            _regionRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == invalidCommand.RegionId), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result result = await _handler.Handle(invalidCommand, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RegionErrors.NotFound(invalidCommand.RegionId));
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenMunicipalityIdAndRegionIdExists()
        {
            _municipalityRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.MunicipalityId), Arg.Any<CancellationToken>())
                .Returns(_municipality);

            _regionRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.RegionId), Arg.Any<CancellationToken>())
                .Returns(_region);

            Result result = await _handler.Handle(_command, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            _municipality.Name.Should().Be(_command.Name);
        }

        [Fact]
        public async Task Handle_Should_CallRepository_WhenMunicipalityIdAndRegionIdExists()
        {
            _municipalityRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.MunicipalityId), Arg.Any<CancellationToken>())
                .Returns(_municipality);

            _regionRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.RegionId), Arg.Any<CancellationToken>())
                .Returns(_region);

            await _handler.Handle(_command, default);

            _municipalityRepositoryMock
                .Received(1)
                .Update(_municipality);
        }

        [Fact]
        public async Task Handle_Should_CallUnitOfWork_WhenMunicipalityIdAndRegionIdExists()
        {
            _municipalityRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.MunicipalityId), Arg.Any<CancellationToken>())
                .Returns(_municipality);

            _regionRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.RegionId), Arg.Any<CancellationToken>())
                .Returns(_region);

            await _handler.Handle(_command, default);

            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}

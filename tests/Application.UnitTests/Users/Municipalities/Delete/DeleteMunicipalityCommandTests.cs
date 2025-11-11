using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Users.Municipalities.Delete;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Municipalities.Delete
{
    public class DeleteMunicipalityCommandTests
    {
        private static readonly DeleteMunicipalityCommand _command = new(1);
        private readonly Municipality _municipality;
        private readonly DeleteMunicipalityCommandHandler _handler;
        private readonly IMunicipalityRepository _municipalityRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public DeleteMunicipalityCommandTests()
        {
            _municipalityRepositoryMock = Substitute.For<IMunicipalityRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _municipality = new() { MunicipalityId = _command.MunicipalityId, Name = "TestName" };
            _handler = new(_municipalityRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenMunicipalityDoesNotExist()
        {
            long notExisitingMunicipalityId = 2500;
            DeleteMunicipalityCommand invalidCommand = _command with { MunicipalityId = notExisitingMunicipalityId };

            _municipalityRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == invalidCommand.MunicipalityId), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result result = await _handler.Handle(invalidCommand, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(MunicipalityErrors.NotFound(invalidCommand.MunicipalityId));
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenMunicipalityExists()
        {
            _municipalityRepositoryMock
               .GetByIdAsync(Arg.Is<long>(id => id == _command.MunicipalityId), Arg.Any<CancellationToken>())
               .Returns(_municipality);

            Result result = await _handler.Handle(_command, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }
    }
}

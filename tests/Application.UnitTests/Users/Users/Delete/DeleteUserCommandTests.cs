using Application.Abstractions.Common;
using Application.Abstractions.Data.UnitOfWork;
using Application.Users.Users.Delete;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using Infrastructure.Identity;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Users.Delete
{
    public class DeleteUserCommandTests
    {
        private static readonly DeleteUserCommand _command = new(1L);
        private readonly IDomainUser _user;
        private readonly DeleteUserCommandHandler _handler;
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork _unitOfWorkMock;

        public DeleteUserCommandTests()
        {
            _identityService = Substitute.For<IIdentityService>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _user = new ApplicationUser { Id = _command.UserId, FirstName = "Test", LastName = "User" };
            _handler = new(_identityService, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenUserDoesNotExist()
        {
            long notExistingUserId = 2500L;
            DeleteUserCommand invalidCommand = _command with { UserId = notExistingUserId };

            _identityService
                .GetUserByIdAsync(Arg.Is<long>(id => id == invalidCommand.UserId), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result result = await _handler.Handle(invalidCommand, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.NotFound(invalidCommand.UserId));
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenUserExists()
        {
            _identityService
                .GetUserByIdAsync(Arg.Is<long>(id => id == _command.UserId), Arg.Any<CancellationToken>())
                .Returns(_user);

            Result result = await _handler.Handle(_command, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_CallIdentityService_WhenUserExists()
        {
            _identityService
                .GetUserByIdAsync(Arg.Is<long>(id => id == _command.UserId), Arg.Any<CancellationToken>())
                .Returns(_user);

            await _handler.Handle(_command, default);

            await _identityService
                .Received(1)
                .DeleteUserAsync(_user.Id);
        }

        [Fact]
        public async Task Handle_Should_CallUnitOfWork_WhenUserExists()
        {
            _identityService
                .GetUserByIdAsync(Arg.Is<long>(id => id == _command.UserId), Arg.Any<CancellationToken>())
                .Returns(_user);

            await _handler.Handle(_command, default);

            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}

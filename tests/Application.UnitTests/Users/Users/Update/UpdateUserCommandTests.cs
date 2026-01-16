using Application.Abstractions.Common;
using Application.Abstractions.Data.UnitOfWork;
using Application.Users.Users.Update;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using Infrastructure.Identity;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Users.Update
{
    public class UpdateUserCommandTests
    {
        private static readonly UpdateUserCommand _command = 
            new(1, "UpdatedTestAvatar", "UpdatedTest", "UpdatesUser", "UpdatedTestPhoneNumber");
        private readonly IDomainUser _user;
        private readonly UpdateUserCommandHandler _handler;
        private readonly IIdentityService _identityServiceMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public UpdateUserCommandTests()
        {
            _identityServiceMock = Substitute.For<IIdentityService>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _user = new ApplicationUser 
            { 
                Id = _command.UserId,
                Avatar = "TestAvatar",
                FirstName = "Test", 
                LastName = "User",
                PhoneNumber = "TestPhoneNumber"
            };
            _handler = new(_identityServiceMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenUserDoesNotExist()
        {
            long notExistingUserId = 2500;
            UpdateUserCommand invalidCommand = _command with { UserId  = notExistingUserId };

            _identityServiceMock
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
            _identityServiceMock
                .GetUserByIdAsync(Arg.Is<long>(id => id == _command.UserId), Arg.Any<CancellationToken>())
                .Returns(_user);

            Result result = await _handler.Handle(_command, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            _user.FirstName.Should().Be(_command.FirstName);
        }

        [Fact]
        public async Task Handle_Should_CallRepository_WhenUserExists()
        {
            _identityServiceMock
                .GetUserByIdAsync(Arg.Is<long>(id => id == _command.UserId), Arg.Any<CancellationToken>())
                .Returns(_user);

            await _handler.Handle(_command, default);

            _identityServiceMock
                .Received(1)
                .Update(_user);
        }

        [Fact]
        public async Task Handle_Should_CallUnitOfWork_WhenUserExists()
        {
            _identityServiceMock
                .GetUserByIdAsync(Arg.Is<long>(id => id == _command.UserId), Arg.Any<CancellationToken>())
                .Returns(_user);

            await _handler.Handle(_command, default);

            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}

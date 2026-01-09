using Application.Abstractions.Common;
using Application.Abstractions.Data.UnitOfWork;
using Application.Users.Users.Create;
using Domain.Errors.Users;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Users.Users.Create
{
    public class CreateUserCommandTests
    {
        private static readonly CreateUserCommand _command =
            new("", "TestName", "TestLastName", "Test", "test@example.com", "Test1234", new DateTime(2000, 10, 10), "+56923147859");
        private readonly CreateUserCommandHandler _handler;
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandTests()
        {
            _identityService = Substitute.For<IIdentityService>();
            _unitOfWork = Substitute.For<IUnitOfWork>();

            _handler = new(_identityService, _unitOfWork);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenEmailIsNotUnique()
        {
            string repeatedEmail = "repeatedEmail@example.com";
            CreateUserCommand invalidCommand = _command with { Email = repeatedEmail };

            _identityService
                .IsEmailUnique(Arg.Is<string>(e => e == repeatedEmail), Arg.Any<CancellationToken>())
                .Returns(false);

            Result<long> result = await _handler.Handle(invalidCommand, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.EmailNotUnique);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenUserNameIsNotUnique()
        {
            string repeatedUserName = "repeatedUserName";
            CreateUserCommand invalidCommand = _command with { UserName = repeatedUserName };

            _identityService
                .IsEmailUnique(Arg.Is<string>(e => e == invalidCommand.Email), Arg.Any<CancellationToken>())
                .Returns(true);

            _identityService
                .IsUserNameUnique(Arg.Is<string>(u => u == repeatedUserName), Arg.Any<CancellationToken>())
                .Returns(false);

            Result<long> result = await _handler.Handle(invalidCommand, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.UsernameNotUnique);
        }
    }
}

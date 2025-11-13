using Application.Abstractions.Data.Repositories.Users;
using Application.Users.Users.GetByUsername;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Users.GetByUsername
{
    public class GetByUsernameQueryTests
    {
        private static readonly GetByUsernameQuery _query = new("TestUsername");
        private readonly User _user;
        private readonly GetByUsernameQueryHandler _handler;
        private readonly IUserRepository _userRepositoryMock;

        public GetByUsernameQueryTests()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();

            _user = new() { UserId = 1L, Username = _query.Username };
            _handler = new(_userRepositoryMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenUserDoesNotExist()
        {
            string notExistingUsername = "InvalidUsername";
            GetByUsernameQuery invalidQuery = _query with { Username = notExistingUsername };

            _userRepositoryMock
                .GetByUsernameAsync(Arg.Is<string>(u => u == invalidQuery.Username), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result<UserResponse> result = await _handler.Handle(_query, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.NotFoundByUsername);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenUserExists()
        {
            _userRepositoryMock
                .GetByUsernameAsync(Arg.Is<string>(u => u == _query.Username), Arg.Any<CancellationToken>())
                .Returns(_user);

            Result<UserResponse> result = await _handler.Handle(_query, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.Value.Username.Should().Be(_query.Username);
        }
    }
}

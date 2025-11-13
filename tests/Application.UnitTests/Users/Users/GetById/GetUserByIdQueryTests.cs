using Application.Abstractions.Data.Repositories.Users;
using Application.Users.Users.GetById;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Users.GetById
{
    public class GetUserByIdQueryTests
    {
        private static readonly GetUserByIdQuery _query = new(1);
        private readonly User _user;
        private readonly GetUserByIdQueryHandler _handler;
        private readonly IUserRepository _userRepositoryMock;

        public GetUserByIdQueryTests()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();

            _user = new() { UserId = _query.UserId, FirstName = "Test" };
            _handler = new(_userRepositoryMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenUserDoesNotExist()
        {
            long notExistingUserId = 2500;
            GetUserByIdQuery invalidQuery = _query with { UserId = notExistingUserId };

            _userRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == invalidQuery.UserId), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result<UserResponse> result = await _handler.Handle(invalidQuery, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.NotFound(invalidQuery.UserId));
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenUserExists()
        {
            _userRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _query.UserId), Arg.Any<CancellationToken>())
                .Returns(_user);

            Result<UserResponse> result = await _handler.Handle(_query, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.Value.FirstName.Should().Be(_user.FirstName);
        }
    }
}

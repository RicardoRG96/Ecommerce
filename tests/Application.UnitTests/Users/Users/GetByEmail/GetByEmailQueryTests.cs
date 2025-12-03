using Application.Abstractions.Data.Repositories.Users;
using Application.Users.Users.GetByEmail;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using Infrastructure.Identity;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Users.GetByEmail
{
    public class GetByEmailQueryTests
    {
        private static readonly GetByEmailQuery _query = new("TestUser@test.com");
        private readonly IDomainUser _user;
        private readonly GetByEmailQueryHandler _handler;
        private readonly IUserRepository _userRepositoryMock;

        public GetByEmailQueryTests()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();

            _user = new ApplicationUser { Id = 1L, Email = _query.Email };
            _handler = new(_userRepositoryMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenUserDoesNotExist()
        {
            string notExistingUserEmail = "invalidEmail@Test.com";
            GetByEmailQuery invalidQuery = _query with { Email = notExistingUserEmail };

            _userRepositoryMock
                .GetUserByEmailAsync(Arg.Is<string>(e => e == invalidQuery.Email), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result<UserResponse> result = await _handler.Handle(invalidQuery, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.NotFoundByEmail);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenUserExists()
        {
            _userRepositoryMock
                .GetUserByEmailAsync(Arg.Is<string>(e => e == _query.Email), Arg.Any<CancellationToken>())
                .Returns(_user);

            Result<UserResponse> result = await _handler.Handle(_query, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.Value.Email.Should().Be(_user.Email);
        }
    }
}

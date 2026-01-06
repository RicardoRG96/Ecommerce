using Application.Abstractions.Common;
using Application.Users.Users.GetByUsername;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using Infrastructure.Identity;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Users.GetByUsername
{
    public class GetByUsernameQueryTests
    {
        private static readonly GetByUsernameQuery _query = new("TestUsername");
        private readonly IDomainUser _user;
        private readonly GetByUsernameQueryHandler _handler;
        private readonly IIdentityService _identityServiceMock;

        public GetByUsernameQueryTests()
        {
            _identityServiceMock = Substitute.For<IIdentityService>();

            _user = new ApplicationUser { Id = 1L, UserName = _query.Username };
            _handler = new(_identityServiceMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenUserDoesNotExist()
        {
            string notExistingUsername = "InvalidUsername";
            GetByUsernameQuery invalidQuery = _query with { Username = notExistingUsername };

            _identityServiceMock
                .GetByUsernameAsync(Arg.Is<string>(u => u == invalidQuery.Username))
                .ReturnsNull();

            Result<UserResponse> result = await _handler.Handle(_query, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.NotFoundByUsername);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenUserExists()
        {
            _identityServiceMock
                .GetByUsernameAsync(Arg.Is<string>(u => u == _query.Username))
                .Returns(_user);

            Result<UserResponse> result = await _handler.Handle(_query, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.Value.Username.Should().Be(_query.Username);
        }
    }
}

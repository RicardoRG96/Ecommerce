using Application.Abstractions.Data.Repositories.Users;
using Application.Users.Users.GetWithPagination;
using Domain.Entities.Users;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Users.Users.GetWithPagination
{
    public class GetUsersWithPaginationQueryTests
    {
        private static readonly GetUsersWithPaginationQuery _query = new(1, 2);
        private readonly PaginatedList<User> _users;
        private List<User>? _items;
        private readonly GetUsersWithPaginationQueryHandler _handler;
        private readonly IUserRepository _userRepositoryMock;

        public GetUsersWithPaginationQueryTests()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();

            CreateUserItems();
            _users = PaginatedList<User>.Create(_items!, _items!.Count, _query.PageNumber, _query.PageSize);
            _handler = new(_userRepositoryMock);
        }

        private void CreateUserItems()
        {
            _items = new List<User>
            {
                new() { UserId = 1, Username = "TestUser1" },
                new() { UserId = 2, Username = "TestUser2" },
                new() { UserId = 3, Username = "TestUser3" }
            };
        }

        [Fact]
        public async Task Handle_Should_ReturnAnEmptyList_WhenThereAreNoUsers()
        {
            PaginatedList<User> emptyPaginatedList = PaginatedList<User>.Create([], 0, 1, 3);

            _userRepositoryMock
                .GetAllAsync(
                    Arg.Is<int>(pn => pn == _query.PageNumber),
                    Arg.Is<int>(ps => ps == _query.PageSize),
                    Arg.Any<CancellationToken>())
                .Returns(emptyPaginatedList);

            Result<PaginatedList<UserResponse>> result = await _handler.Handle(_query, default);

            result.Value.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_Should_ReturnAListWithElements_WhenThereAreUsers()
        {
            _userRepositoryMock
                .GetAllAsync(
                    Arg.Is<int>(pn => pn == _query.PageNumber),
                    Arg.Is<int>(ps => ps == _query.PageSize),
                    Arg.Any<CancellationToken>())
                .Returns(_users);

            Result<PaginatedList<UserResponse>> result = await _handler.Handle(_query, default);

            result.Value.Items.Count.Should().Be(_items!.Count);
        }
    }
}

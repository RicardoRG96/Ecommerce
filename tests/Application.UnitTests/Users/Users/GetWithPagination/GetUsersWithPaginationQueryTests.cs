using Application.Abstractions.Common;
using Application.Users.Users.GetWithPagination;
using Domain.Entities.Users;
using FluentAssertions;
using Infrastructure.Identity;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Users.Users.GetWithPagination
{
    public class GetUsersWithPaginationQueryTests
    {
        private static readonly GetUsersWithPaginationQuery _query = new(1, 2);
        private readonly PaginatedList<IDomainUser> _users;
        private List<IDomainUser>? _items;
        private readonly GetUsersWithPaginationQueryHandler _handler;
        private readonly IIdentityService _identityServiceMock;

        public GetUsersWithPaginationQueryTests()
        {
            _identityServiceMock = Substitute.For<IIdentityService>();

            CreateUserItems();
            _users = PaginatedList<IDomainUser>.Create(_items!, _items!.Count, _query.PageNumber, _query.PageSize);
            _handler = new(_identityServiceMock);
        }

        private void CreateUserItems()
        {
            _items = new List<IDomainUser>
            {
                new ApplicationUser { Id = 1, UserName = "TestUser1" },
                new ApplicationUser { Id = 2, UserName = "TestUser2" },
                new ApplicationUser { Id = 3, UserName = "TestUser3" }
            };
        }

        [Fact]
        public async Task Handle_Should_ReturnAnEmptyList_WhenThereAreNoUsers()
        {
            PaginatedList<IDomainUser> emptyPaginatedList = PaginatedList<IDomainUser>.Create([], 0, 1, 3);

            _identityServiceMock
                .GetAllUsersAsync(
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
            _identityServiceMock
                .GetAllUsersAsync(
                    Arg.Is<int>(pn => pn == _query.PageNumber),
                    Arg.Is<int>(ps => ps == _query.PageSize),
                    Arg.Any<CancellationToken>())
                .Returns(_users);

            Result<PaginatedList<UserResponse>> result = await _handler.Handle(_query, default);

            result.Value.Items.Count.Should().Be(_items!.Count);
        }
    }
}

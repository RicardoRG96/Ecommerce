using Application.Abstractions.Messaging;

namespace Application.Users.Users.Get
{
    public sealed record GetUsersWithPaginationQuery() : IQuery<List<UserResponse>>;
}

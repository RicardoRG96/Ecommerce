using Application.Abstractions.Messaging;

namespace Application.Users.Users.Get
{
    public sealed record GetUsersQuery(long UserId) : IQuery<List<UserResponse>>;
}

using Application.Abstractions.Messaging;

namespace Application.Users.Users.GetById
{
    public sealed record GetUserByIdQuery(long UserId) : IQuery<UserResponse>;
}

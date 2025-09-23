using Application.Abstractions.Messaging;

namespace Application.Users.User.GetById
{
    public sealed record GetUserByIdQuery(long UserId) : IQuery<UserResponse>;
}

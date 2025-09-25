using Application.Abstractions.Messaging;

namespace Application.Users.Users.GetByUsername
{
    public sealed record GetByUsernameQuery(string Username) : IQuery<UserResponse>;
}

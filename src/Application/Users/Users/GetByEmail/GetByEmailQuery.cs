using Application.Abstractions.Messaging;

namespace Application.Users.Users.GetByEmail
{
    internal sealed record GetByEmailQuery(string Email) : IQuery<UserResponse>;
}

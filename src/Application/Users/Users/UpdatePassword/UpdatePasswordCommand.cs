using Application.Abstractions.Messaging;

namespace Application.Users.Users.UpdatePassword
{
    public sealed record UpdatePasswordCommand(
        long UserId,
        string CurrentPassword,
        string NewPassword) : ICommand;
}

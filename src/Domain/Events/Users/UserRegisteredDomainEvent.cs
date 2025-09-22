using SharedKernel;

namespace Domain.Events.Users
{
    public sealed record UserRegisteredDomainEvent(long UserId) : IDomainEvent;
}

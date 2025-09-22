using SharedKernel;

namespace Domain.Events.Users
{
    public sealed record UserBirthdayDomainEvent(long UserId) : IDomainEvent;
}

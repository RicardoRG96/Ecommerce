using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel
{
    public abstract class BaseEntity
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        [NotMapped]
        public List<IDomainEvent> DomainEvents => [.. _domainEvents];

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public void Raise(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }
    }
}

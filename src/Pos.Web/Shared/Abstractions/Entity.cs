namespace Pos.Web.Shared.Abstractions
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        // Domain Events support (Wolverine picks these up automatically)
        private readonly List<object> _domainEvents = new();
        public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

        public void RaiseDomainEvent(object domainEvent) => _domainEvents.Add(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();
    }

    public abstract class AuditableEntity : Entity
    {
        public DateTime CreatedOnUtc { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }
        public string? ModifiedBy { get; set; }
    }
}

using System.Collections.Generic;

namespace ECommerce.SharedKernel.Events;

/// <summary>
/// Marker interface for entities that can raise domain events
/// </summary>
public interface IHasDomainEvents
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void AddDomainEvent(IDomainEvent domainEvent);
    void ClearDomainEvents();
}
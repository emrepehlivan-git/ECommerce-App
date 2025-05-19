using MediatR;

namespace ECommerce.SharedKernel.Events;

/// <summary>
/// Marker interface for domain events
/// </summary>
public interface IDomainEvent : INotification
{
}
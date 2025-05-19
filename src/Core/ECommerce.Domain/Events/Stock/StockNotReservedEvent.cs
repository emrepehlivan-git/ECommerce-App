using ECommerce.SharedKernel.Events;

namespace ECommerce.Domain.Events.Stock;

public sealed record StockNotReservedEvent(
    Guid ProductId,
    int RequestedQuantity) : IDomainEvent;
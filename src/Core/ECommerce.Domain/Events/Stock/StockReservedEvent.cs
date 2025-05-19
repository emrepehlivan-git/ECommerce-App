using ECommerce.SharedKernel.Events;

namespace ECommerce.Domain.Events.Stock;

public sealed record StockReservedEvent(
    Guid ProductId,
    int Quantity) : IDomainEvent;
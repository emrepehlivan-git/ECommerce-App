using ECommerce.Domain.Enums;

namespace ECommerce.Application.Features.Orders.DTOs;

public sealed record OrderDto(
    Guid Id,
    Guid UserId,
    DateTime OrderDate,
    OrderStatus Status,
    decimal TotalAmount,
    string? ShippingAddress,
    string? BillingAddress,
    IReadOnlyCollection<OrderItemDto> Items);
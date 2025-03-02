namespace ECommerce.Application.Features.Orders.DTOs;

public sealed record OrderItemDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity,
    decimal TotalPrice);
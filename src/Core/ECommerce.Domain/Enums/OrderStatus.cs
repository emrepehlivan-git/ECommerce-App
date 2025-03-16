namespace ECommerce.Domain.Enums;

public enum OrderStatus : byte
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}
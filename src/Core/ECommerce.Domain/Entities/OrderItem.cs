using ECommerce.Domain.ValueObjects;

namespace ECommerce.Domain.Entities;

public sealed class OrderItem : BaseEntity
{
    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;

    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;

    public Price UnitPrice { get; private set; } = Price.Create(0);
    public int Quantity { get; private set; }
    public Price TotalPrice { get; private set; } = Price.Create(0);

    internal OrderItem()
    {
    }

    private OrderItem(Guid orderId, Guid productId, decimal unitPrice, int quantity)
    {
        OrderId = orderId;
        ProductId = productId;
        UnitPrice = Price.Create(unitPrice);
        Quantity = quantity;
        TotalPrice = UnitPrice * quantity;
    }

    public static OrderItem Create(Guid orderId, Guid productId, decimal unitPrice, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        }

        if (unitPrice < 0)
        {
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));
        }

        return new(orderId, productId, unitPrice, quantity);
    }

    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        }

        Quantity = quantity;
        TotalPrice = UnitPrice * quantity;
    }

    public void UpdateUnitPrice(decimal unitPrice)
    {
        if (unitPrice < 0)
        {
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));
        }

        UnitPrice = Price.Create(unitPrice);
        TotalPrice = UnitPrice * Quantity;
    }
}
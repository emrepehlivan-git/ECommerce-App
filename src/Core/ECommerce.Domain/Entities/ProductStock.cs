namespace ECommerce.Domain.Entities;

public sealed class ProductStock : AuditableEntity
{
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; }
    public int Quantity { get; private set; }

    internal ProductStock()
    {
    }

    private ProductStock(Guid productId, int quantity)
    {
        if (quantity < 0)
        {
            throw new ArgumentException("Stock quantity cannot be negative.", nameof(quantity));
        }

        ProductId = productId;
        Quantity = quantity;
    }

    public static ProductStock Create(Guid productId, int quantity)
    {
        return new(productId, quantity);
    }

    public void UpdateQuantity(int quantity)
    {
        if (quantity < 0)
        {
            throw new ArgumentException("Stock quantity cannot be negative.", nameof(quantity));
        }

        Quantity = quantity;
    }

    public void Reserve(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        }

        if (Quantity < quantity)
        {
            throw new InvalidOperationException("Insufficient stock.");
        }

        Quantity -= quantity;
    }

    public void Release(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        }

        Quantity += quantity;
    }
}

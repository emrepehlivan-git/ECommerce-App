namespace ECommerce.Domain.Entities;

public sealed class ProductStock : AuditableEntity
{
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = new();
    public int Quantity { get; private set; }

    internal ProductStock()
    {
    }

    private ProductStock(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }

    public static ProductStock Create(Guid productId, int quantity)
    {
        return new(productId, quantity);
    }

    private int ValidateQuantity(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Quantity must be greater than 0");

        return quantity;
    }
}

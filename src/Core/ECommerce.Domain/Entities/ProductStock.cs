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
        ValidateQuantity(quantity);
        ProductId = productId;
        Quantity = quantity;
    }

    public static ProductStock Create(Guid productId, int quantity)
    {
        return new(productId, quantity);
    }

    private void ValidateQuantity(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Quantity must be greater than 0");
    }

    public void Reserve(int quantity)
    {
        if (Quantity < quantity)
            throw new InvalidOperationException("Insufficient stock");

        Quantity -= quantity;
    }

    public void Release(int quantity)
    {
        ValidateQuantity(quantity);
        Quantity += quantity;
    }
}

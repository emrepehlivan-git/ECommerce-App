namespace ECommerce.Domain.Entities;

public sealed class Order : AuditableEntity
{
    private readonly List<OrderItem> _items = [];

    public Guid UserId { get; private set; }
    public User User { get; private set; }

    public DateTime OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }

    public string ShippingAddress { get; private set; }
    public string BillingAddress { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    internal Order()
    {
    }

    private Order(Guid userId, string shippingAddress, string billingAddress)
    {
        UserId = userId;
        OrderDate = DateTime.UtcNow;
        Status = OrderStatus.Pending;
        TotalAmount = 0;
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
    }

    public static Order Create(Guid userId, string shippingAddress, string billingAddress)
    {
        return new(userId, shippingAddress, billingAddress);
    }

    public void AddItem(Guid productId, decimal unitPrice, int quantity)
    {
        var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);

        if (existingItem is not null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            var orderItem = OrderItem.Create(Id, productId, unitPrice, quantity);
            _items.Add(orderItem);
        }

        RecalculateTotalAmount();
    }

    public void RemoveItem(Guid productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);

        if (item is not null)
        {
            _items.Remove(item);
            RecalculateTotalAmount();
        }
    }

    public void UpdateItemQuantity(Guid productId, int quantity)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);

        if (item is not null)
        {
            if (quantity <= 0)
            {
                _items.Remove(item);
            }
            else
            {
                item.UpdateQuantity(quantity);
            }

            RecalculateTotalAmount();
        }
    }

    private void RecalculateTotalAmount()
    {
        TotalAmount = _items.Sum(i => i.TotalPrice);
    }

    public void UpdateStatus(OrderStatus status)
    {
        Status = status;
    }

    public void UpdateAddresses(string shippingAddress, string billingAddress)
    {
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
    }
}

public enum OrderStatus : byte
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}
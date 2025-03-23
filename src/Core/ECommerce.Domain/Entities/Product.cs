using ECommerce.Domain.ValueObjects;

namespace ECommerce.Domain.Entities;

public sealed class Product : AuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; set; }
    public Price Price { get; private set; }

    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }

    public int StockQuantity { get; private set; }

    internal Product()
    {
    }

    private Product(string name, string? description, decimal price, Guid categoryId, int initialStock)
    {
        Name = name;
        Description = description;
        Price = Price.Create(price);
        CategoryId = categoryId;
        StockQuantity = initialStock;
    }

    public static Product Create(string name, string? description, decimal price, Guid categoryId, int initialStock = 0)
    {
        return new(name, description, price, categoryId, initialStock);
    }

    public void Update(string name, decimal price, Guid categoryId, string? description)
    {
        Name = name;
        Description = description;
        Price = Price.Create(price);
        CategoryId = categoryId;
    }

    public void UpdateStock(int quantity)
    {
        StockQuantity = quantity;
    }

    public bool HasSufficientStock(int requestedQuantity)
    {
        return StockQuantity >= requestedQuantity;
    }
}


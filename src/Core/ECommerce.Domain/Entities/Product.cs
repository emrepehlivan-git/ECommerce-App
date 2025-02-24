namespace ECommerce.Domain.Entities;

public sealed class Product : AuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; private set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = new();

    internal Product()
    {
    }

    private Product(string name, string? description = null, decimal price = 0, Guid categoryId = default)
    {
        Name = name;
        Description = description;
        Price = price;
        CategoryId = categoryId;
    }

    public static Product Create(string name, string? description = null, decimal price = 0, Guid categoryId = default)
    {
        return new(name, description, price, categoryId);
    }

    public void Update(string name, decimal price, string? description)
    {
        Name = name;
        Description = description;
        Price = price;
    }
}


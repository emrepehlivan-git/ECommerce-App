namespace ECommerce.Domain.Entities;

public sealed class Product : AuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; private set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; }

    internal Product()
    {
    }

    private Product(string name, string? description, decimal price, Guid categoryId)
    {
        Name = name;
        Description = description;
        Price = price;
        CategoryId = categoryId;
    }

    public static Product Create(string name, string? description, decimal price, Guid categoryId)
    {
        return new(name, description, price, categoryId);
    }

    public void Update(string name, decimal price, Guid categoryId, string? description)
    {
        Name = name;
        Description = description;
        Price = price;
        CategoryId = categoryId;
    }
}


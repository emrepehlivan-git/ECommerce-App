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

    private Product(string name, string description, decimal price, Guid categoryId)
    {
        Name = ValidateName(name);
        Description = description;
        Price = ValidatePrice(price);
        CategoryId = categoryId;
    }

    public static Product Create(string name, string description, decimal price, Guid categoryId)
    {
        return new(name, description, price, categoryId);
    }

    private string ValidateName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (name.Length < 3)
            throw new ArgumentException("Name must be at least 3 characters long");

        if (name.Length > 100)
            throw new ArgumentException("Name must be less than 100 characters long");

        return name;
    }

    private decimal ValidatePrice(decimal price)
    {
        if (price <= 0)
            throw new ArgumentException("Price must be greater than 0");

        return price;
    }

    public void Update(string name, decimal price, string? description)
    {
        Name = ValidateName(name);
        Description = description;
        Price = ValidatePrice(price);
    }
}


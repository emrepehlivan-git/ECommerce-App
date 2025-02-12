namespace ECommerce.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public ICollection<Product> Products { get; private set; } = [];

    internal Category()
    {
    }

    private Category(string name)
    {
        Name = ValidateName(name);
    }

    public static Category Create(string name)
    {
        return new(name);
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
}



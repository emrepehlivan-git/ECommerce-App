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
        ValidateName(name);
    }

    public static Category Create(string name)
    {
        return new(name);
    }
    public void UpdateName(string name)
    {
        ValidateName(name);
    }

    private void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        if (name.Length < 3)
            throw new ArgumentException("Name cannot be less than 3 characters.", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Name cannot be longer than 100 characters.", nameof(name));

        Name = name;
    }
}



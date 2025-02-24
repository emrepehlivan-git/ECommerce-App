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
        Name = name;
    }

    public static Category Create(string name)
    {
        return new(name);
    }
    public void UpdateName(string name)
    {
        Name = name;
    }
}



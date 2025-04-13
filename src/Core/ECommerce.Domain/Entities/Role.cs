using Microsoft.AspNetCore.Identity;

namespace ECommerce.Domain.Entities;

public sealed class Role : IdentityRole<Guid>
{
    private Role()
    {
    }

    private Role(string name)
    {
        Validate(name);

        Name = name;
    }

    public static Role Create(string name)
    {
        return new Role(name);
    }

    public void UpdateName(string name)
    {
        Validate(name);

        Name = name;
    }

    private void Validate(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new NullReferenceException("Name cannot be null or empty.");

        if (name.Length < 2)
            throw new ArgumentException("Name cannot be less than 2 characters.");

        if (name.Length > 100)
            throw new ArgumentException("Name cannot be longer than 100 characters.");
    }
}
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Domain.Entities;

public sealed class Role : IdentityRole<Guid>
{
    private Role()
    {
    }

    private Role(string name)
    {
        IsValid(name);
        Name = name;
    }

    public static Role Create(string name)
    {
        return new Role(name);
    }

    public void UpdateName(string name) => Name = name;

    private void IsValid(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidOperationException("Name cannot be empty");

        if (name.Length > 50)
            throw new InvalidOperationException("Name cannot be longer than 50 characters");

        if (name.Length < 3)
            throw new InvalidOperationException("Name cannot be shorter than 3 characters");
    }
}
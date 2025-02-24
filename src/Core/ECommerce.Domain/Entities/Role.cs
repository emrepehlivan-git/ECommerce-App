using Microsoft.AspNetCore.Identity;

namespace ECommerce.Domain.Entities;

public sealed class Role : IdentityRole<Guid>
{
    private Role()
    {
    }

    private Role(string name)
    {
        Name = name;
    }

    public static Role Create(string name)
    {
        return new Role(name);
    }

    public void UpdateName(string name) => Name = name;
}
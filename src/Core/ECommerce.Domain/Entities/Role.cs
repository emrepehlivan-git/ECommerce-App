using Microsoft.AspNetCore.Identity;

namespace ECommerce.Domain.Entities;

public sealed class Role : IdentityRole<Guid>
{
    private readonly List<RolePermission> _rolePermissions = [];

    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

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

    public void AddPermission(RolePermission rolePermission)
    {
        if (rolePermission == null)
            throw new ArgumentNullException(nameof(rolePermission));

        _rolePermissions.Add(rolePermission);
    }

    public void RemovePermission(RolePermission rolePermission)
    {
        if (rolePermission == null)
            throw new ArgumentNullException(nameof(rolePermission));

        _rolePermissions.Remove(rolePermission);
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
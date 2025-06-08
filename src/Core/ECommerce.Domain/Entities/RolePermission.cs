namespace ECommerce.Domain.Entities;

public sealed class RolePermission : BaseEntity
{
    public Guid RoleId { get; private set; }
    public Role Role { get; private set; } = null!;

    public Guid PermissionId { get; private set; }
    public Permission Permission { get; private set; } = null!;

    public bool IsActive { get; private set; }

    private RolePermission()
    {
    }

    private RolePermission(Guid roleId, Guid permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
        IsActive = true;
    }

    internal RolePermission(Role role, Permission permission)
    {
        RoleId = role.Id;
        PermissionId = permission.Id;
        Role = role;
        Permission = permission;
        IsActive = true;
    }

    public static RolePermission Create(Guid roleId, Guid permissionId)
    {
        return new(roleId, permissionId);
    }

    public static RolePermission Create(Role role, Permission permission)
    {
        return new(role, permission);
    }

    public void Deactivate() => IsActive = false;

    public void Activate() => IsActive = true;
}
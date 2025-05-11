namespace ECommerce.Domain.Entities;

public sealed class Permission : BaseEntity
{
    private readonly List<RolePermission> _rolePermissions = [];

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Module { get; private set; } = string.Empty;
    public string Action { get; private set; } = string.Empty;

    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

    private Permission()
    {
    }

    private Permission(string name, string description, string module, string action)
    {
        Validate(name, description, module, action);
        Name = name;
        Description = description;
        Module = module;
        Action = action;
    }

    public static Permission Create(string name, string description, string module, string action)
    {
        return new(name, description, module, action);
    }

    public void Update(string name, string description, string module, string action)
    {
        Validate(name, description, module, action);
        Name = name;
        Description = description;
        Module = module;
        Action = action;
    }

    private void Validate(string name, string description, string module, string action)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Name cannot be longer than 100 characters.", nameof(name));

        if (description.Length > 500)
            throw new ArgumentException("Description cannot be longer than 500 characters.", nameof(description));

        if (string.IsNullOrWhiteSpace(module))
            throw new ArgumentException("Module cannot be null or empty.", nameof(module));

        if (module.Length > 50)
            throw new ArgumentException("Module cannot be longer than 50 characters.", nameof(module));

        if (string.IsNullOrWhiteSpace(action))
            throw new ArgumentException("Action cannot be null or empty.", nameof(action));

        if (action.Length > 50)
            throw new ArgumentException("Action cannot be longer than 50 characters.", nameof(action));
    }
}
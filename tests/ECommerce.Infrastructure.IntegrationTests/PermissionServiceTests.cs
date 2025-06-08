namespace ECommerce.Infrastructure.IntegrationTests;

public class PermissionServiceTests
{
    private readonly Mock<IIdentityService> _identityServiceMock = new();
    private readonly PermissionService _permissionService;

    public PermissionServiceTests()
    {
        _permissionService = new PermissionService(_identityServiceMock.Object);
    }

    [Fact]
    public async Task HasPermissionAsync_ShouldReturnTrue_WhenUserHasPermission()
    {
        var userId = Guid.NewGuid();
        var user = User.Create("test", "user", "test@example.com");
        var permission = Permission.Create("Orders.Read", "", "Orders", "Read");
        var role = Role.Create("Admin");
        var rolePermission = RolePermission.Create(role.Id, permission.Id);
        role.AddPermission(rolePermission);
        _identityServiceMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);
        _identityServiceMock.Setup(x => x.GetUserRolesAsync(user)).ReturnsAsync(new List<string>{role.Name!});
        _identityServiceMock.Setup(x => x.GetAllRolesAsync()).ReturnsAsync(new List<Role>{role});

        var result = await _permissionService.HasPermissionAsync(userId, permission.Name);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task GetUserPermissionsAsync_ShouldReturnUserPermissions()
    {
        var userId = Guid.NewGuid();
        var user = User.Create("test", "user", "test@example.com");
        var permission1 = Permission.Create("Orders.Read", "", "Orders", "Read");
        var permission2 = Permission.Create("Orders.Create", "", "Orders", "Create");
        var role = Role.Create("Admin");
        var rolePermission1 = RolePermission.Create(role.Id, permission1.Id);
        var rolePermission2 = RolePermission.Create(role.Id, permission2.Id);
        role.AddPermission(rolePermission1);
        role.AddPermission(rolePermission2);
        _identityServiceMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);
        _identityServiceMock.Setup(x => x.GetUserRolesAsync(user)).ReturnsAsync(new List<string>{role.Name!});
        _identityServiceMock.Setup(x => x.GetAllRolesAsync()).ReturnsAsync(new List<Role>{role});

        var result = await _permissionService.GetUserPermissionsAsync(userId);

        result.Should().BeEquivalentTo(new[]{permission1.Name, permission2.Name});
    }
}

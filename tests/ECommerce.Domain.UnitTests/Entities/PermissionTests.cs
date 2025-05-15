namespace ECommerce.Domain.UnitTests.Entities;

public sealed class PermissionTests
{
    private const string ValidName = "Test Permission";
    private const string ValidDescription = "Test Description";
    private const string ValidModule = "Test Module";
    private const string ValidAction = "Test Action";

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithInvalidName_ShouldThrowArgumentException(string? name)
    {
        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => Permission.Create(name, ValidDescription, ValidModule, ValidAction);
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or empty.*");
    }

    [Fact]
    public void Create_WithLongName_ShouldThrowArgumentException()
    {
        // Arrange
        var longName = new string('a', 101);

        // Act
        var act = () => Permission.Create(longName, ValidDescription, ValidModule, ValidAction);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be longer than 100 characters.*");
    }

    [Fact]
    public void Create_WithLongDescription_ShouldThrowArgumentException()
    {
        // Arrange
        var longDescription = new string('a', 501);

        // Act
        var act = () => Permission.Create(ValidName, longDescription, ValidModule, ValidAction);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Description cannot be longer than 500 characters.*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithInvalidModule_ShouldThrowArgumentException(string? module)
    {
        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => Permission.Create(ValidName, ValidDescription, module, ValidAction);
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Module cannot be null or empty.*");
    }

    [Fact]
    public void Create_WithLongModule_ShouldThrowArgumentException()
    {
        // Arrange
        var longModule = new string('a', 51);

        // Act
        var act = () => Permission.Create(ValidName, ValidDescription, longModule, ValidAction);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Module cannot be longer than 50 characters.*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithInvalidAction_ShouldThrowArgumentException(string? action)
    {
        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => Permission.Create(ValidName, ValidDescription, ValidModule, action);
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Action cannot be null or empty.*");
    }

    [Fact]
    public void Create_WithLongAction_ShouldThrowArgumentException()
    {
        // Arrange
        var longAction = new string('a', 51);

        // Act
        var act = () => Permission.Create(ValidName, ValidDescription, ValidModule, longAction);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Action cannot be longer than 50 characters.*");
    }

    [Theory]
    [InlineData("Test Permission", "Test Description", "Test Module", "Test Action")]
    [InlineData("User Create", "Create user permission", "Users", "Create")]
    [InlineData("Product Delete", "Delete product permission", "Products", "Delete")]
    public void Create_WithValidParameters_ShouldCreatePermission(string name, string description, string module, string action)
    {
        // Act
        var permission = Permission.Create(name, description, module, action);

        // Assert
        permission.Should().NotBeNull();
        permission.Name.Should().Be(name);
        permission.Description.Should().Be(description);
        permission.Module.Should().Be(module);
        permission.Action.Should().Be(action);
        permission.RolePermissions.Should().NotBeNull();
        permission.RolePermissions.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Update_WithInvalidName_ShouldThrowArgumentException(string? name)
    {
        // Arrange
        var permission = Permission.Create(ValidName, ValidDescription, ValidModule, ValidAction);

        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => permission.Update(name, ValidDescription, ValidModule, ValidAction);
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or empty.*");
    }

    [Fact]
    public void Update_WithLongName_ShouldThrowArgumentException()
    {
        // Arrange
        var permission = Permission.Create(ValidName, ValidDescription, ValidModule, ValidAction);
        var longName = new string('a', 101);

        // Act
        var act = () => permission.Update(longName, ValidDescription, ValidModule, ValidAction);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be longer than 100 characters.*");
    }

    [Fact]
    public void Update_WithLongDescription_ShouldThrowArgumentException()
    {
        // Arrange
        var permission = Permission.Create(ValidName, ValidDescription, ValidModule, ValidAction);
        var longDescription = new string('a', 501);

        // Act
        var act = () => permission.Update(ValidName, longDescription, ValidModule, ValidAction);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Description cannot be longer than 500 characters.*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Update_WithInvalidModule_ShouldThrowArgumentException(string? module)
    {
        // Arrange
        var permission = Permission.Create(ValidName, ValidDescription, ValidModule, ValidAction);

        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => permission.Update(ValidName, ValidDescription, module, ValidAction);
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Module cannot be null or empty.*");
    }

    [Fact]
    public void Update_WithLongModule_ShouldThrowArgumentException()
    {
        // Arrange
        var permission = Permission.Create(ValidName, ValidDescription, ValidModule, ValidAction);
        var longModule = new string('a', 51);

        // Act
        var act = () => permission.Update(ValidName, ValidDescription, longModule, ValidAction);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Module cannot be longer than 50 characters.*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Update_WithInvalidAction_ShouldThrowArgumentException(string? action)
    {
        // Arrange
        var permission = Permission.Create(ValidName, ValidDescription, ValidModule, ValidAction);

        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => permission.Update(ValidName, ValidDescription, ValidModule, action);
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Action cannot be null or empty.*");
    }

    [Fact]
    public void Update_WithLongAction_ShouldThrowArgumentException()
    {
        // Arrange
        var permission = Permission.Create(ValidName, ValidDescription, ValidModule, ValidAction);
        var longAction = new string('a', 51);

        // Act
        var act = () => permission.Update(ValidName, ValidDescription, ValidModule, longAction);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Action cannot be longer than 50 characters.*");
    }

    [Theory]
    [InlineData("Updated Permission", "Updated Description", "Updated Module", "Updated Action")]
    [InlineData("User Update", "Update user permission", "Users", "Update")]
    [InlineData("Product View", "View product permission", "Products", "View")]
    public void Update_WithValidParameters_ShouldUpdatePermission(string name, string description, string module, string action)
    {
        // Arrange
        var permission = Permission.Create(ValidName, ValidDescription, ValidModule, ValidAction);

        // Act
        permission.Update(name, description, module, action);

        // Assert
        permission.Name.Should().Be(name);
        permission.Description.Should().Be(description);
        permission.Module.Should().Be(module);
        permission.Action.Should().Be(action);
    }
}
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Domain.UnitTests.Entities;

public sealed class CategoryTests
{
    private const string ValidName = "Test Category";

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithInvalidName_ShouldThrowArgumentException(string? name)
    {
        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => Category.Create(name);
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or empty.*");
    }

    [Theory]
    [InlineData("AB")]
    [InlineData("A")]
    public void Create_WithShortName_ShouldThrowArgumentException(string name)
    {
        // Act
        var act = () => Category.Create(name);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be less than 3 characters.*");
    }

    [Fact]
    public void Create_WithLongName_ShouldThrowArgumentException()
    {
        // Arrange
        var longName = new string('a', 101);

        // Act
        var act = () => Category.Create(longName);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be longer than 100 characters.*");
    }

    [Theory]
    [InlineData("Test Category")]
    [InlineData("Electronics")]
    [InlineData("Books")]
    public void Create_WithValidName_ShouldCreateCategory(string name)
    {
        // Act
        var category = Category.Create(name);

        // Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(name);
        category.Products.Should().NotBeNull();
        category.Products.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void UpdateName_WithInvalidName_ShouldThrowArgumentException(string? name)
    {
        // Arrange
        var category = Category.Create(ValidName);

        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => category.UpdateName(name);
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or empty.*");
    }

    [Theory]
    [InlineData("AB")]
    [InlineData("A")]
    public void UpdateName_WithShortName_ShouldThrowArgumentException(string name)
    {
        // Arrange
        var category = Category.Create(ValidName);

        // Act
        var act = () => category.UpdateName(name);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be less than 3 characters.*");
    }

    [Fact]
    public void UpdateName_WithLongName_ShouldThrowArgumentException()
    {
        // Arrange
        var category = Category.Create(ValidName);
        var longName = new string('a', 101);

        // Act
        var act = () => category.UpdateName(longName);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be longer than 100 characters.*");
    }

    [Theory]
    [InlineData("Updated Category")]
    [InlineData("New Electronics")]
    [InlineData("New Books")]
    public void UpdateName_WithValidName_ShouldUpdateName(string name)
    {
        // Arrange
        var category = Category.Create(ValidName);

        // Act
        category.UpdateName(name);

        // Assert
        category.Name.Should().Be(name);
    }
}
namespace ECommerce.Domain.UnitTests.Entities;

public sealed class ProductTests
{
    private readonly Guid _categoryId = new Guid("123e4567-e89b-12d3-a456-426614174000");
    private const string ValidName = "Test Product";
    private const string ValidDescription = "Test Description";
    private const decimal ValidPrice = 100m;
    private const int ValidStockQuantity = 10;

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithInvalidName_ShouldThrowArgumentException(string name)
    {
        // Act
        var act = () => Product.Create(name, ValidDescription, ValidPrice, _categoryId, ValidStockQuantity);

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
        var act = () => Product.Create(name, ValidDescription, ValidPrice, _categoryId, ValidStockQuantity);

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
        var act = () => Product.Create(longName, ValidDescription, ValidPrice, _categoryId, ValidStockQuantity);

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
        var act = () => Product.Create(ValidName, longDescription, ValidPrice, _categoryId, ValidStockQuantity);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Description cannot be longer than 500 characters.*");
    }

    [Theory]
    [InlineData("Test Product", "Test Description", 100, 10)]
    [InlineData("Another Product", null, 200, 0)]
    public void Create_WithValidParameters_ShouldCreateProduct(string name, string? description, decimal price, int stockQuantity)
    {
        // Act
        var product = Product.Create(name, description, price, _categoryId, stockQuantity);

        // Assert
        product.Should().NotBeNull();
        product.Name.Should().Be(name);
        product.Description.Should().Be(description);
        product.Price.Should().NotBeNull();
        product.Price.Value.Should().Be(price);
        product.CategoryId.Should().Be(_categoryId);
        product.StockQuantity.Should().Be(stockQuantity);
    }

    [Fact]
    public void Update_WithValidParameters_ShouldUpdateProduct()
    {
        // Arrange
        var product = Product.Create(ValidName, ValidDescription, ValidPrice, _categoryId, ValidStockQuantity);
        var newName = "Updated Product";
        var newDescription = "Updated Description";
        var newPrice = 200m;
        var newCategoryId = new Guid("bf9e6eff-f59a-4bbb-9007-59755e20dc2d");

        // Act
        product.Update(newName, newPrice, newCategoryId, newDescription);

        // Assert
        product.Name.Should().Be(newName);
        product.Description.Should().Be(newDescription);
        product.Price.Value.Should().Be(newPrice);
        product.CategoryId.Should().Be(newCategoryId);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    public void UpdateStock_WithNegativeQuantity_ShouldThrowArgumentException(int quantity)
    {
        // Arrange
        var product = Product.Create(ValidName, ValidDescription, ValidPrice, _categoryId, ValidStockQuantity);

        // Act
        var act = () => product.UpdateStock(quantity);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Stock quantity cannot be negative.*");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(10)]
    public void UpdateStock_WithValidQuantity_ShouldUpdateStock(int quantity)
    {
        // Arrange
        var product = Product.Create(ValidName, ValidDescription, ValidPrice, _categoryId, ValidStockQuantity);

        // Act
        product.UpdateStock(quantity);

        // Assert
        product.StockQuantity.Should().Be(quantity);
    }

    [Theory]
    [InlineData(5, 10, true)]
    [InlineData(10, 10, true)]
    [InlineData(15, 10, false)]
    public void HasSufficientStock_ShouldReturnCorrectResult(int requestedQuantity, int stockQuantity, bool expectedResult)
    {
        // Arrange
        var product = Product.Create(ValidName, ValidDescription, ValidPrice, _categoryId, stockQuantity);

        // Act
        var result = product.HasSufficientStock(requestedQuantity);

        // Assert
        result.Should().Be(expectedResult);
    }
}
namespace ECommerce.Domain.UnitTests.Entities;

public sealed class OrderItemTests
{
    private readonly Guid _orderId = new Guid("ef40322b-1946-472c-97b3-7e90e401c872");
    private readonly Guid _productId = new Guid("8932467a-3c87-49c9-9726-d9f96ff5c1b2");
    private const decimal ValidUnitPrice = 100m;
    private const int ValidQuantity = 2;

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WithInvalidQuantity_ShouldThrowArgumentException(int quantity)
    {
        // Act
        var act = () => OrderItem.Create(_orderId, _productId, ValidUnitPrice, quantity);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Quantity must be greater than zero*");
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-1)]
    public void Create_WithInvalidUnitPrice_ShouldThrowArgumentException(decimal unitPrice)
    {
        // Act
        var act = () => OrderItem.Create(_orderId, _productId, unitPrice, ValidQuantity);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Unit price cannot be negative*");
    }

    [Theory]
    [InlineData(1, 100)]
    [InlineData(2, 50)]
    [InlineData(5, 20)]
    public void Create_WithValidParameters_ShouldCreateOrderItem(int quantity, decimal unitPrice)
    {
        // Act
        var orderItem = OrderItem.Create(_orderId, _productId, unitPrice, quantity);

        // Assert
        orderItem.Should().NotBeNull();
        orderItem.OrderId.Should().Be(_orderId);
        orderItem.ProductId.Should().Be(_productId);
        orderItem.UnitPrice.Should().NotBeNull();
        orderItem.UnitPrice.Value.Should().Be(unitPrice);
        orderItem.Quantity.Should().Be(quantity);
        orderItem.TotalPrice.Should().NotBeNull();
        orderItem.TotalPrice.Value.Should().Be(unitPrice * quantity);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void UpdateQuantity_WithInvalidQuantity_ShouldThrowArgumentException(int quantity)
    {
        // Arrange
        var orderItem = OrderItem.Create(_orderId, _productId, ValidUnitPrice, ValidQuantity);

        // Act
        var act = () => orderItem.UpdateQuantity(quantity);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Quantity must be greater than zero*");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void UpdateQuantity_WithValidQuantity_ShouldUpdateQuantityAndTotalPrice(int quantity)
    {
        // Arrange
        var orderItem = OrderItem.Create(_orderId, _productId, ValidUnitPrice, ValidQuantity);

        // Act
        orderItem.UpdateQuantity(quantity);

        // Assert
        orderItem.Quantity.Should().Be(quantity);
        orderItem.TotalPrice.Value.Should().Be(ValidUnitPrice * quantity);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-1)]
    public void UpdateUnitPrice_WithInvalidUnitPrice_ShouldThrowArgumentException(decimal unitPrice)
    {
        // Arrange
        var orderItem = OrderItem.Create(_orderId, _productId, ValidUnitPrice, ValidQuantity);

        // Act
        var act = () => orderItem.UpdateUnitPrice(unitPrice);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Unit price cannot be negative*");
    }

    [Theory]
    [InlineData(50)]
    [InlineData(150)]
    [InlineData(200)]
    public void UpdateUnitPrice_WithValidUnitPrice_ShouldUpdateUnitPriceAndTotalPrice(decimal unitPrice)
    {
        // Arrange
        var orderItem = OrderItem.Create(_orderId, _productId, ValidUnitPrice, ValidQuantity);

        // Act
        orderItem.UpdateUnitPrice(unitPrice);

        // Assert
        orderItem.UnitPrice.Value.Should().Be(unitPrice);
        orderItem.TotalPrice.Value.Should().Be(unitPrice * ValidQuantity);
    }
}
using ECommerce.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace ECommerce.Domain.UnitTests.ValueObjects;

public sealed class PriceTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Create_WithInvalidValue_ShouldThrowArgumentException(decimal value)
    {
        // Act
        var act = () => Price.Create(value);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Value must be greater than 0.*");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(1000.50)]
    public void Create_WithValidValue_ShouldCreatePrice(decimal value)
    {
        // Act
        var price = Price.Create(value);

        // Assert
        price.Should().NotBeNull();
        price.Value.Should().Be(value);
    }

    [Fact]
    public void Add_WithValidPrice_ShouldReturnNewPrice()
    {
        // Arrange
        var price1 = Price.Create(100);
        var price2 = Price.Create(50);

        // Act
        var result = price1 + price2;

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(150);
    }

    [Fact]
    public void Subtract_WithValidPrice_ShouldReturnNewPrice()
    {
        // Arrange
        var price1 = Price.Create(100);
        var price2 = Price.Create(50);

        // Act
        var result = price1 - price2;

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(50);
    }

    [Fact]
    public void Subtract_WithInvalidResult_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var price1 = Price.Create(50);
        var price2 = Price.Create(100);

        // Act
        var act = () => price1 - price2;

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Subtraction would result in a negative price.");
    }

    [Fact]
    public void Multiply_WithValidMultiplier_ShouldReturnNewPrice()
    {
        // Arrange
        var price = Price.Create(100);
        var multiplier = 2;

        // Act
        var result = price * multiplier;

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(200);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Multiply_WithInvalidMultiplier_ShouldThrowArgumentException(int multiplier)
    {
        // Arrange
        var price = Price.Create(100);

        // Act
        var act = () => price * multiplier;

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Quantity must be greater than zero.*");
    }

    [Fact]
    public void Divide_WithValidDivisor_ShouldReturnNewPrice()
    {
        // Arrange
        var price = Price.Create(100);
        var divisor = 2;

        // Act
        var result = price / divisor;

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(50);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Divide_WithInvalidDivisor_ShouldThrowArgumentException(int divisor)
    {
        // Arrange
        var price = Price.Create(100);

        // Act
        var act = () => price / divisor;

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Quantity must be greater than zero.*");
    }

    [Fact]
    public void Divide_WithInvalidResult_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var price = Price.Create(50);
        var divisor = 100;

        // Act
        var act = () => price / divisor;

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Division would result in a price less than 1.");
    }

    [Fact]
    public void Compare_WithValidPrices_ShouldReturnCorrectResults()
    {
        // Arrange
        var price1 = Price.Create(100);
        var price2 = Price.Create(200);
        var price3 = Price.Create(100);

        // Act & Assert
        (price1 < price2).Should().BeTrue();
        (price1 > price2).Should().BeFalse();
        (price1 <= price3).Should().BeTrue();
        (price1 >= price3).Should().BeTrue();
    }

    [Fact]
    public void ToString_ShouldReturnFormattedValue()
    {
        // Arrange
        var price = Price.Create(100.50m);

        // Act
        var result = price.ToString();

        // Assert
        result.Should().Be("100.50");
    }
}
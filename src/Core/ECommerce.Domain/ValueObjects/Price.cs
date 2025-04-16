namespace ECommerce.Domain.ValueObjects;

public record Price
{
    public decimal Value { get; init; }

    public static readonly Price Zero = new Price(0m);

    private Price(decimal value)
    {
        IsValidValue(value);
        Value = value;
    }

    public static Price Create(decimal value)
    {
        return new Price(value);
    }

    private static void IsValidValue(decimal value)
    {
        if (value < 0)
        {
            throw new ArgumentException("Value cannot be negative.", nameof(value));
        }
    }

    public static Price operator +(Price a, Price b)
    {
        return Create(a.Value + b.Value);
    }

    public static Price operator -(Price a, Price b)
    {
        return Create(a.Value - b.Value);
    }

    public static Price operator *(Price a, int quantity)
    {
        if (quantity < 0)
        {
            throw new ArgumentException("Quantity cannot be negative.", nameof(quantity));
        }
        return Create(a.Value * quantity);
    }

    public static Price operator /(Price a, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        }
        return Create(a.Value / quantity);
    }

    public static bool operator <(Price a, Price b)
    {
        return a.Value < b.Value;
    }

    public static bool operator >(Price a, Price b)
    {
        return a.Value > b.Value;
    }

    public static bool operator <=(Price a, Price b)
    {
        return a.Value <= b.Value;
    }

    public static bool operator >=(Price a, Price b)
    {
        return a.Value >= b.Value;
    }

    public static implicit operator decimal(Price price) => price.Value;

    public override string ToString() => $"{Value.ToString("N2", System.Globalization.CultureInfo.InvariantCulture)}";
}
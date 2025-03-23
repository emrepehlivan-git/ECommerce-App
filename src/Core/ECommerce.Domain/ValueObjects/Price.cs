namespace ECommerce.Domain.ValueObjects;

public record Price
{
    public decimal Value { get; init; }

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
            throw new ArgumentException("Value must be greater than 0.", nameof(value));
        }
    }

    public static Price operator +(Price a, Price b) => Create(a.Value + b.Value);

    public static Price operator -(Price a, Price b)
    {
        var result = a.Value - b.Value;
        if (result < 0)
        {
            throw new InvalidOperationException("Subtraction would result in a negative price.");
        }
        return Create(result);
    }

    public static Price operator *(Price a, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        }
        return Create(a.Value * quantity);
    }

    public static Price operator /(Price a, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        }

        var result = a.Value / quantity;
        if (result < 0)
        {
            throw new InvalidOperationException("Division would result in a negative price.");
        }
        return Create(result);
    }

    public static bool operator <(Price a, Price b) => a.Value < b.Value;
    public static bool operator >(Price a, Price b) => a.Value > b.Value;
    public static bool operator <=(Price a, Price b) => a.Value <= b.Value;
    public static bool operator >=(Price a, Price b) => a.Value >= b.Value;

    public override string ToString() => Value.ToString("N2");
}
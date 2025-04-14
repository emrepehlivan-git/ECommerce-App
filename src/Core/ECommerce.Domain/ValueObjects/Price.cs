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
            throw new ArgumentException("Value cannot be negative.", nameof(value));
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
        if (result < 1)
        {
            throw new InvalidOperationException("Division would result in a price less than 1.");
        }
        return Create(result);
    }

    public static bool operator <(Price a, Price b) => a.Value < b.Value;
    public static bool operator >(Price a, Price b) => a.Value > b.Value;
    public static bool operator <=(Price a, Price b) => a.Value <= b.Value;
    public static bool operator >=(Price a, Price b) => a.Value >= b.Value;

    public static implicit operator decimal(Price price) => price.Value;
    public static implicit operator double(Price price) => (double)price.Value;
    public static implicit operator float(Price price) => (float)price.Value;
    public static implicit operator Price(decimal value) => Create(value);
    public static implicit operator Price(double value) => Create((decimal)value);
    public static implicit operator Price(float value) => Create((decimal)value);
    public override string ToString() => Value.ToString("N2", System.Globalization.CultureInfo.InvariantCulture);
}
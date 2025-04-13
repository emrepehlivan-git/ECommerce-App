namespace ECommerce.Domain.ValueObjects;

public sealed record FullName
{
    public string FirstName { get; init; }
    public string LastName { get; init; }

    private FullName(string firstName, string lastName)
    {
        Validate(firstName, lastName);

        FirstName = firstName;
        LastName = lastName;
    }

    public static FullName Create(string firstName, string lastName)
    {
        return new FullName(firstName.Trim(), lastName.Trim());
    }

    private void Validate(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new NullReferenceException("First name cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new NullReferenceException("Last name cannot be null or empty.");

        if (firstName.Length < 3)
            throw new ArgumentException("First name cannot be less than 3 characters.", nameof(firstName));

        if (lastName.Length < 3)
            throw new ArgumentException("Last name cannot be less than 3 characters.", nameof(lastName));

        if (firstName.Length > 150)
            throw new ArgumentException("First name cannot be longer than 150 characters.", nameof(firstName));

        if (lastName.Length > 150)
            throw new ArgumentException("Last name cannot be longer than 150 characters.", nameof(lastName));
    }

    public override string ToString() => $"{FirstName} {LastName}";
}
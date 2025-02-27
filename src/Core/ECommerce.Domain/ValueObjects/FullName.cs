namespace ECommerce.Domain.ValueObjects;

public sealed record FullName
{
    public string FirstName { get; init; }
    public string LastName { get; init; }

    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static FullName Create(string firstName, string lastName)
    {
        return new FullName(firstName.Trim(), lastName.Trim());
    }

    public override string ToString() => $"{FirstName} {LastName}";
}
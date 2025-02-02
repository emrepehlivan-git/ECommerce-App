using ECommerce.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Domain.Entities;

public sealed class User : IdentityUser<Guid>
{
    public FullName FullName { get; private set; } = null!;
    public bool IsActive { get; private set; }

    private User()
    {
    }

    private User(string email, FullName fullName)
    {
        Email = email;
        UserName = email;
        FullName = fullName;
        IsActive = true;
    }

    public static User Create(string email, string firstName, string lastName)
    {
        return new(email, FullName.Create(firstName, lastName));
    }

    public void Deactivate() => IsActive = false;

    public void Activate() => IsActive = true;

    public void UpdateName(string firstName, string lastName) => FullName = FullName.Create(firstName, lastName);
}
namespace ECommerce.Application.UnitTests.Features.Users;

public abstract class UserQueriesTestBase
{
    protected readonly Mock<IIdentityService> IdentityServiceMock;
    protected readonly Mock<ILazyServiceProvider> LazyServiceProviderMock;
    protected readonly Mock<ILocalizationService> LocalizationServiceMock;
    protected readonly User DefaultUser;

    protected UserQueriesTestBase()
    {
        IdentityServiceMock = new Mock<IIdentityService>();
        LazyServiceProviderMock = new Mock<ILazyServiceProvider>();
        LocalizationServiceMock = new Mock<ILocalizationService>();

        DefaultUser = User.Create("test@example.com", "Test User", "Password123!");
    }

    protected void SetupUserExists(bool exists = true)
    {
        IdentityServiceMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(exists ? DefaultUser : null);
    }

    protected void SetupUsersQuery(IEnumerable<User> users)
    {
        var queryable = users.AsQueryable();
        IdentityServiceMock
            .Setup(x => x.Users)
            .Returns(queryable);
    }
}
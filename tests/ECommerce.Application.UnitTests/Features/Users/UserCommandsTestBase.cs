using ECommerce.Application.Features.Users;

namespace ECommerce.Application.UnitTests.Features.Users;

public class UserCommandsTestBase
{
    protected Guid UserId = Guid.Parse("e64db34c-7455-41da-b255-a9a7a46ace54");
    protected User DefaultUser => User.Create("test@example.com", "Test User", "Password123!");

    protected Mock<IIdentityService> IdentityServiceMock;
    protected Mock<ILazyServiceProvider> LazyServiceProviderMock;
    protected Mock<ILocalizationService> LocalizationServiceMock;

    protected LocalizationHelper Localizer;

    protected UserCommandsTestBase()
    {
        IdentityServiceMock = new Mock<IIdentityService>();
        LazyServiceProviderMock = new Mock<ILazyServiceProvider>();
        LocalizationServiceMock = new Mock<ILocalizationService>();

        Localizer = new LocalizationHelper(LocalizationServiceMock.Object);

        SetupDefaultLocalizationMessages();
    }

    protected void SetupDefaultLocalizationMessages()
    {
        LocalizationServiceMock
            .Setup(x => x.GetLocalizedString(UserConsts.NotFound))
            .Returns("User not found");
    }

    protected void SetupUserExists(bool exists = true)
    {
        IdentityServiceMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(exists ? DefaultUser : null);
    }

    protected void SetupLocalizedMessage(string message)
    {
        LocalizationServiceMock
            .Setup(x => x.GetLocalizedString(message))
            .Returns(message);
    }
}
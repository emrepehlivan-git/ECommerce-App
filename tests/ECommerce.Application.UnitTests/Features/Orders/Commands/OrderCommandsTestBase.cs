using ECommerce.Application.Features.Orders;
using ECommerce.Application.Helpers;
using ECommerce.Application.Interfaces;

namespace ECommerce.Application.UnitTests.Features.Orders.Commands;

public abstract class OrderCommandsTestBase
{
    protected readonly Mock<IOrderRepository> OrderRepositoryMock;
    protected readonly Mock<IProductRepository> ProductRepositoryMock;
    protected readonly Mock<IOrderItemRepository> OrderItemRepositoryMock;
    protected readonly Mock<IStockRepository> StockRepositoryMock;
    protected readonly Mock<IIdentityService> IdentityServiceMock;
    protected readonly Mock<ILazyServiceProvider> LazyServiceProviderMock;
    protected readonly Mock<ILocalizationService> LocalizationServiceMock;
    protected readonly LocalizationHelper Localizer;

    protected readonly Guid UserId = Guid.Parse("e64db34c-7455-41da-b255-a9a7a46ace54");
    protected readonly Order DefaultOrder;

    protected OrderCommandsTestBase()
    {
        OrderRepositoryMock = new Mock<IOrderRepository>();
        ProductRepositoryMock = new Mock<IProductRepository>();
        OrderItemRepositoryMock = new Mock<IOrderItemRepository>();
        StockRepositoryMock = new Mock<IStockRepository>();
        IdentityServiceMock = new Mock<IIdentityService>();
        LazyServiceProviderMock = new Mock<ILazyServiceProvider>();
        LocalizationServiceMock = new Mock<ILocalizationService>();
        Localizer = new LocalizationHelper(LocalizationServiceMock.Object);

        LazyServiceProviderMock
            .Setup(x => x.LazyGetRequiredService<LocalizationHelper>())
            .Returns(Localizer);

        SetupDefaultLocalizationMessages();

        DefaultOrder = Order.Create(UserId, "Test Shipping", "Test Billing");
    }

    protected void SetupOrderRepositoryGetByIdAsync(Order? order = null)
    {
        OrderRepositoryMock
            .Setup(x => x.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<Expression<Func<IQueryable<Order>, IQueryable<Order>>>>?(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(order ?? DefaultOrder);
    }

    protected void SetupDefaultLocalizationMessages()
    {
        LocalizationServiceMock.Setup(x => x.GetLocalizedString(OrderConsts.NotFound)).Returns("Order not found");
        LocalizationServiceMock.Setup(x => x.GetLocalizedString(OrderConsts.ProductNotFound)).Returns("Product not found");
        LocalizationServiceMock.Setup(x => x.GetLocalizedString(OrderConsts.OrderCannotBeModified)).Returns("Order cannot be modified");
        LocalizationServiceMock.Setup(x => x.GetLocalizedString(OrderConsts.OrderCannotBeCancelled)).Returns("Order cannot be cancelled");
        LocalizationServiceMock.Setup(x => x.GetLocalizedString(OrderConsts.QuantityMustBeGreaterThanZero)).Returns("Quantity must be greater than zero");
        LocalizationServiceMock.Setup(x => x.GetLocalizedString(OrderConsts.UserNotFound)).Returns("User not found");
        LocalizationServiceMock.Setup(x => x.GetLocalizedString(OrderConsts.OrderItemNotFound)).Returns("Order item not found");
        LocalizationServiceMock.Setup(x => x.GetLocalizedString(OrderConsts.ShippingAddressRequired)).Returns("Shipping address is required");
        LocalizationServiceMock.Setup(x => x.GetLocalizedString(OrderConsts.BillingAddressRequired)).Returns("Billing address is required");
        LocalizationServiceMock.Setup(x => x.GetLocalizedString(OrderConsts.EmptyOrder)).Returns("Order cannot be empty");
    }
}

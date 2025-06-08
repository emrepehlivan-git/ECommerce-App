namespace ECommerce.Infrastructure.IntegrationTests.Repositories;

public class OrderRepositoryTests : RepositoryTestBase
{
    private readonly OrderRepository _repository;

    public OrderRepositoryTests()
    {
        _repository = new OrderRepository(Context);
    }

    [Fact]
    public async Task GetUserOrdersAsync_ReturnsOrdersOrderedDescending()
    {
        var userId = Guid.NewGuid();
        var order1 = Order.Create(userId, "s1", "b1");
        order1.AddItem(Guid.NewGuid(), Price.Create(10m), 1);
        await Task.Delay(10); // ensure later order date
        var order2 = Order.Create(userId, "s2", "b2");
        order2.AddItem(Guid.NewGuid(), Price.Create(20m), 2);
        var otherOrder = Order.Create(Guid.NewGuid(), "s3", "b3");

        Context.Orders.AddRange(order1, order2, otherOrder);
        await Context.SaveChangesAsync();

        var result = await _repository.GetUserOrdersAsync(userId);

        result.Should().HaveCount(2);
        result.First().Id.Should().Be(order2.Id);
        result.Last().Id.Should().Be(order1.Id);
        result.First().Items.Should().NotBeEmpty();
    }
}

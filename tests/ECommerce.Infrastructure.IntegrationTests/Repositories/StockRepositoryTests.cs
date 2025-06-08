namespace ECommerce.Infrastructure.IntegrationTests.Repositories;

public class StockRepositoryTests : RepositoryTestBase
{
    private readonly StockRepository _repository;

    public StockRepositoryTests()
    {
        _repository = new StockRepository(Context);
    }

    [Fact]
    public async Task ReserveStockAsync_DecreasesQuantity()
    {
        var stock = ProductStock.Create(Guid.NewGuid(), 10);
        Context.ProductStocks.Add(stock);
        await Context.SaveChangesAsync();

        await _repository.ReserveStockAsync(stock.ProductId, 4);

        var updated = await Context.ProductStocks.FirstAsync();
        updated.Quantity.Should().Be(6);
    }

    [Fact]
    public async Task ReserveStockAsync_Throws_WhenInsufficient()
    {
        var stock = ProductStock.Create(Guid.NewGuid(), 2);
        Context.ProductStocks.Add(stock);
        await Context.SaveChangesAsync();

        var act = async () => await _repository.ReserveStockAsync(stock.ProductId, 5);

        await act.Should().ThrowAsync<BusinessException>();
    }

    [Fact]
    public async Task ReleaseStockAsync_IncreasesQuantity()
    {
        var stock = ProductStock.Create(Guid.NewGuid(), 5);
        Context.ProductStocks.Add(stock);
        await Context.SaveChangesAsync();

        await _repository.ReleaseStockAsync(stock.ProductId, 3);

        var updated = await Context.ProductStocks.FirstAsync();
        updated.Quantity.Should().Be(8);
    }
}

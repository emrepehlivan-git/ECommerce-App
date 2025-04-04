using ECommerce.Application.Features.Products.Queries;

namespace ECommerce.Application.UnitTests.Features.Products.Queries;

public sealed class GetProductStockInfoTest : ProductQueriesTestsBase
{
    private readonly GetProductStockInfoHandler Handler;
    private readonly GetProductStockInfo Query;

    public GetProductStockInfoTest()
    {
        Query = new GetProductStockInfo(Guid.NewGuid());

        Handler = new GetProductStockInfoHandler(
            ProductRepositoryMock.Object,
            LazyServiceProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidProduct_ShouldReturnStockQuantity()
    {
        SetupProductExists(true);
        DefaultProduct.UpdateStock(10);

        var result = await Handler.Handle(Query, CancellationToken.None);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(10);
    }

    [Fact]
    public async Task Handle_WithNonExistentProduct_ShouldReturnNotFound()
    {
        SetupProductExists(false);

        var result = await Handler.Handle(Query, CancellationToken.None);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(ResultStatus.NotFound);
    }
}
using ECommerce.Application.Features.Products;
using ECommerce.Application.Features.Products.Queries;

namespace ECommerce.Application.UnitTests.Features.Products.Queries;

public sealed class GetProductByIdQueryTest : ProductQueriesTestsBase
{
    private readonly GetProductByIdQueryHandler Handler;
    private readonly GetProductByIdQuery Query;

    public GetProductByIdQueryTest()
    {
        Query = new GetProductByIdQuery(Guid.NewGuid());

        Handler = new GetProductByIdQueryHandler(
            ProductRepositoryMock.Object,
            LazyServiceProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidQuery_ShouldReturnProduct()
    {
        // Arrange
        SetupProductExists(true);

        // Act
        var result = await Handler.Handle(Query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(DefaultProduct.Id);
    }

    [Fact]
    public async Task Handle_WithInvalidQuery_ShouldReturnNotFound()
    {
        // Arrange
        SetupProductExists(false);

        // Act
        var result = await Handler.Handle(Query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(ResultStatus.NotFound);
        result.Errors.Should().ContainSingle()
            .Which.Should().Be(Localizer[ProductConsts.NotFound]);
    }
}

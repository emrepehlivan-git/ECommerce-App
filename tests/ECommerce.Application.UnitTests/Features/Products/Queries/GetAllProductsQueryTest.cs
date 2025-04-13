using ECommerce.Application.Features.Products.Queries;
using ECommerce.Application.Common.Parameters;
using ECommerce.Application.Features.Products.DTOs;

namespace ECommerce.Application.UnitTests.Features.Products.Queries;

public sealed class GetAllProductsQueryTest : ProductQueriesTestsBase
{
    private readonly GetAllProductsQueryHandler Handler;
    private readonly GetAllProductsQuery Query;

    public GetAllProductsQueryTest()
    {
        Query = new GetAllProductsQuery(new PageableRequestParams());

        Handler = new GetAllProductsQueryHandler(
            ProductRepositoryMock.Object,
            LazyServiceProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidQuery_ShouldReturnPagedProducts()
    {
        var products = new List<Product> { DefaultProduct };
        var queryable = products.AsQueryable();

        ProductRepositoryMock
            .Setup(x => x.Query(
                It.IsAny<Expression<Func<Product, bool>>>(),
                It.IsAny<Expression<Func<IQueryable<Product>, IOrderedQueryable<Product>>>>(),
                It.IsAny<Expression<Func<IQueryable<Product>, IQueryable<Product>>>>(),
                It.IsAny<bool>()))
            .Returns(queryable);

        var result = await Handler.Handle(Query, CancellationToken.None);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_WithEmptyProducts_ShouldReturnEmptyList()
    {
        var emptyProducts = new List<Product>();
        var queryable = emptyProducts.AsQueryable();

        ProductRepositoryMock
            .Setup(x => x.Query(
                It.IsAny<Expression<Func<Product, bool>>>(),
                It.IsAny<Expression<Func<IQueryable<Product>, IOrderedQueryable<Product>>>>(),
                It.IsAny<Expression<Func<IQueryable<Product>, IQueryable<Product>>>>(),
                It.IsAny<bool>()))
            .Returns(queryable);

        var result = await Handler.Handle(Query, CancellationToken.None);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEmpty();
    }
}
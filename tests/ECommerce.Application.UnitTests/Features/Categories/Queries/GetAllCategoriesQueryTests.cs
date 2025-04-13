using ECommerce.Application.Common.Parameters;
using ECommerce.Application.Features.Categories;
using ECommerce.Application.Features.Categories.Queries;
using Mapster;

namespace ECommerce.Application.UnitTests.Features.Categories.Queries;

public sealed class GetAllCategoriesQueryTests : CategoryQueriesTestBase
{
    private readonly GetAllCategoriesQueryHandler Handler;
    private readonly GetAllCategoriesQuery Query;
    private readonly List<Category> Categories;

    public GetAllCategoriesQueryTests()
    {
        Categories = new List<Category>
        {
            Category.Create("Category 1"),
            Category.Create("Category 2"),
            Category.Create("Category 3")
        };

        Query = new GetAllCategoriesQuery(new PageableRequestParams(Page: 1, PageSize: 10));
        Handler = new GetAllCategoriesQueryHandler(CategoryRepositoryMock.Object, LazyServiceProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidQuery_ShouldReturnCategories()
    {
        // Arrange
        var queryable = Categories.AsQueryable();
        CategoryRepositoryMock
            .Setup(x => x.Query(
                It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Expression<Func<IQueryable<Category>, IOrderedQueryable<Category>>>>(),
                It.IsAny<Expression<Func<IQueryable<Category>, IQueryable<Category>>>>(),
                It.IsAny<bool>()))
            .Returns(queryable);

        // Act
        var result = await Handler.Handle(Query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(3);
    }

    [Fact]
    public async Task Handle_WithPagination_ShouldReturnPaginatedCategories()
    {
        // Arrange
        var query = new GetAllCategoriesQuery(new PageableRequestParams(Page: 1, PageSize: 2));
        var queryable = Categories.AsQueryable();
        CategoryRepositoryMock
            .Setup(x => x.Query(
                It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Expression<Func<IQueryable<Category>, IOrderedQueryable<Category>>>>(),
                It.IsAny<Expression<Func<IQueryable<Category>, IQueryable<Category>>>>(),
                It.IsAny<bool>()))
            .Returns(queryable);

        // Act
        var result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WithOrderBy_ShouldReturnOrderedCategories()
    {
        // Arrange
        var query = new GetAllCategoriesQuery(new PageableRequestParams(Page: 1, PageSize: 10), OrderBy: "Name desc");
        var orderedCategories = Categories.OrderByDescending(x => x.Name).ToList();
        var queryable = orderedCategories.AsQueryable();
        CategoryRepositoryMock
            .Setup(x => x.Query(
                It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Expression<Func<IQueryable<Category>, IOrderedQueryable<Category>>>>(),
                It.IsAny<Expression<Func<IQueryable<Category>, IQueryable<Category>>>>(),
                It.IsAny<bool>()))
            .Returns(queryable);

        // Act
        var result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeInDescendingOrder(x => x.Name);
    }
}

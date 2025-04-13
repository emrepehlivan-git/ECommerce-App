using ECommerce.Application.Features.Categories.Commands;
namespace ECommerce.Application.UnitTests.Features.Categories.Commands;

public sealed class DeleteBulkCategoriesCommandTests : CategoryCommandsTestBase
{
    private readonly DeleteBulkCategoriesCommandHandler Handler;
    private readonly DeleteBulkCategoriesCommand Command;
    private readonly List<Guid> CategoryIds;

    public DeleteBulkCategoriesCommandTests()
    {
        CategoryIds = [
            new("e150053f-2c4c-4c8a-a1ea-d83b7ba89d1a"),
            new("c4b7c572-2e58-4db9-9177-4fa199573d21") ];
        Command = new DeleteBulkCategoriesCommand(CategoryIds);

        Handler = new DeleteBulkCategoriesCommandHandler(
            CategoryRepositoryMock.Object,
            LazyServiceProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldDeleteCategories()
    {
        // Arrange
        var categories = CategoryIds.Select(id => Category.Create($"Category {id}")).ToList();
        CategoryRepositoryMock
            .Setup(x => x.Query(
                It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Expression<Func<IQueryable<Category>, IOrderedQueryable<Category>>>>(),
                It.IsAny<Expression<Func<IQueryable<Category>, IQueryable<Category>>>>(),
                It.IsAny<bool>()))
            .Returns(categories.AsQueryable());

        // Act
        var result = await Handler.Handle(Command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithNonExistentCategories_ShouldReturnNotFound()
    {
        // Arrange
        CategoryRepositoryMock
            .Setup(x => x.Query(
                It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Expression<Func<IQueryable<Category>, IOrderedQueryable<Category>>>>(),
                It.IsAny<Expression<Func<IQueryable<Category>, IQueryable<Category>>>>(),
                It.IsAny<bool>()))
            .Returns(new List<Category>().AsQueryable());

        // Act
        var result = await Handler.Handle(Command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(ResultStatus.NotFound);
    }
}
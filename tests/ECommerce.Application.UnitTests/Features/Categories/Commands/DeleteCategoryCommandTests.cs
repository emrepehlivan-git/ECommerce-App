using ECommerce.Application.Features.Categories.Commands;

namespace ECommerce.Application.UnitTests.Features.Categories.Commands;

public sealed class DeleteCategoryCommandTests : CategoryCommandsTestBase
{
    private readonly DeleteCategoryCommandHandler Handler;
    private readonly DeleteCategoryCommand Command;
    private readonly Guid CategoryId = new("e150053f-2c4c-4c8a-a1ea-d83b7ba89d1a");

    public DeleteCategoryCommandTests()
    {
        Command = new DeleteCategoryCommand(CategoryId);

        Handler = new DeleteCategoryCommandHandler(
            CategoryRepositoryMock.Object,
            LazyServiceProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldDeleteCategory()
    {
        // Arrange
        var existingCategory = Category.Create("Test Category");
        SetupCategoryRepositoryGetByIdAsync(existingCategory);

        // Act
        var result = await Handler.Handle(Command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithNonExistentCategory_ShouldReturnNotFound()
    {
        // Arrange
        SetupCategoryRepositoryGetByIdAsync(null);

        // Act
        var result = await Handler.Handle(Command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Should().Be("Category not found");
    }
}
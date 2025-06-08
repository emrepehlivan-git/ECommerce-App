namespace ECommerce.Infrastructure.IntegrationTests.Repositories;

public class CategoryRepositoryTests : RepositoryTestBase
{
    private readonly CategoryRepository _repository;

    public CategoryRepositoryTests()
    {
        _repository = new CategoryRepository(Context);
    }

    [Fact]
    public async Task HasProductsAsync_ReturnsTrue_WhenProductExists()
    {
        var category = Category.Create("Electronics");
        category.Id = Guid.NewGuid();
        var product = Product.Create("Phone", "desc", 10m, category.Id, 5);

        Context.Categories.Add(category);
        Context.Products.Add(product);
        await Context.SaveChangesAsync();

        var result = await _repository.HasProductsAsync(category.Id);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task HasProductsAsync_ReturnsFalse_WhenNoProducts()
    {
        var category = Category.Create("Books");
        category.Id = Guid.NewGuid();

        Context.Categories.Add(category);
        await Context.SaveChangesAsync();

        var result = await _repository.HasProductsAsync(category.Id);

        result.Should().BeFalse();
    }
}

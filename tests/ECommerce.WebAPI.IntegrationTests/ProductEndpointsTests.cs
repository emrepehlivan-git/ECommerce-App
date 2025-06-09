namespace ECommerce.WebAPI.IntegrationTests;

public class ProductEndpointsTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private HttpClient _client = default!;

    public ProductEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    public async Task InitializeAsync()
    {
        await _factory.InitializeAsync();
        _client = _factory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        await (_factory as IAsyncLifetime).DisposeAsync();
    }

    [Fact]
    public async Task GetProducts_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/Product");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateProduct_PersistsProduct()
    {
        // Arrange - create category directly
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var category = Category.Create("Integration");
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var command = new
        {
            Name = "Phone",
            Description = "Smart phone",
            Price = 100m,
            CategoryId = category.Id,
            StockQuantity = 3
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Product", command);
        response.EnsureSuccessStatusCode();

        // Assert DB
        var product = await context.Products.Include(p => p.Stock).FirstOrDefaultAsync();
        product.Should().NotBeNull();
        product!.Name.Should().Be("Phone");
        product.Stock.Quantity.Should().Be(3);
    }
}

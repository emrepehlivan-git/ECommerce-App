namespace ECommerce.WebAPI.IntegrationTests.Endpoints;

public class CategoryEndpointsTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private HttpClient _client = default!;

    public CategoryEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    public async Task InitializeAsync()
    {
        await _factory.InitializeAsync();
        _client = _factory.CreateClient();
    }

    public async Task DisposeAsync() => await Task.CompletedTask;

    [Fact]
    public async Task GetCategories_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/Category");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetCategoryById_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var category = Category.Create("IntegrationCat");
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var response = await _client.GetAsync($"/api/Category/{category.Id}");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CreateCategory_RequiresAuthorization()
    {
        var command = new { Name = "New Category" };
        var response = await _client.PostAsJsonAsync("/api/Category", command);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task UpdateCategory_RequiresAuthorization()
    {
        var command = new { Name = "Updated" };
        var response = await _client.PutAsJsonAsync($"/api/Category/{Guid.NewGuid()}", command);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteCategory_RequiresAuthorization()
    {
        var response = await _client.DeleteAsync($"/api/Category/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}

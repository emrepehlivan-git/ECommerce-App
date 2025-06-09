namespace ECommerce.WebAPI.IntegrationTests;

public class UsersEndpointsTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private HttpClient _client = default!;

    public UsersEndpointsTests(CustomWebApplicationFactory factory)
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
    public async Task GetUsers_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/Users");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetUser_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var user = User.Create("john@example.com", "John", "Doe");
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var response = await _client.GetAsync($"/api/Users/{user.Id}");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task ActivateUser_ActivatesUser()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var user = User.Create("john2@example.com", "John", "Smith");
        user.Deactivate();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var response = await _client.PostAsync($"/api/Users/activate/{user.Id}", null);
        response.EnsureSuccessStatusCode();

        var updated = await context.Users.FindAsync(user.Id);
        updated!.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task DeactivateUser_DeactivatesUser()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var user = User.Create("jane@example.com", "Jane", "Doe");
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var response = await _client.PostAsync($"/api/Users/deactivate/{user.Id}", null);
        response.EnsureSuccessStatusCode();

        var updated = await context.Users.FindAsync(user.Id);
        updated!.IsActive.Should().BeFalse();
    }
}

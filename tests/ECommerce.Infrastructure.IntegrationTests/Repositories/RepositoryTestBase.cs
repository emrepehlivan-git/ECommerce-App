using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.IntegrationTests.Repositories;

public abstract class RepositoryTestBase : IDisposable
{
    protected readonly ApplicationDbContext Context;

    protected RepositoryTestBase()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        Context = new ApplicationDbContext(options);
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}

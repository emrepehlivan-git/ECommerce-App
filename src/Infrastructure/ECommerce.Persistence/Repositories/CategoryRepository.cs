using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistence.Repositories;

public sealed class CategoryRepository(ApplicationDbContext context) : BaseRepository<Category>(context), ICategoryRepository
{
    public async Task<bool> HasProductsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Products.AnyAsync(p => p.CategoryId == id, cancellationToken);
    }
}

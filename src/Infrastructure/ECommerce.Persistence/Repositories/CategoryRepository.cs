using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Contexts;

namespace ECommerce.Persistence.Repositories;

public sealed class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }
}

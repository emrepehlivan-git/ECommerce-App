using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Contexts;
using ECommerce.SharedKernel;

namespace ECommerce.Persistence.Repositories;

public sealed class CategoryRepository : BaseRepository<Category>, ICategoryRepository, IScopedDependency
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }
}

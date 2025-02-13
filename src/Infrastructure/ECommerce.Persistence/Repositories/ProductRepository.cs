using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Contexts;
using ECommerce.SharedKernel;

namespace ECommerce.Persistence.Repositories;

public sealed class ProductRepository : BaseRepository<Product>, IProductRepository, IScopedDependency
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }
}

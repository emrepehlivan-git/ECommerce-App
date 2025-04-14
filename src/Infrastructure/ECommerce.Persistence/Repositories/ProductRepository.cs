using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Contexts;

namespace ECommerce.Persistence.Repositories;

public sealed class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }
}

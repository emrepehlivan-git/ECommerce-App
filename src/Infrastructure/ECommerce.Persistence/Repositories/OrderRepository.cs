using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistence.Repositories;

public sealed class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Order>> GetUserOrdersAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Order>()
            .AsNoTracking()
            .Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }
}
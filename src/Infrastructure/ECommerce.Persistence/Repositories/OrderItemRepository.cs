using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Contexts;
using ECommerce.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistence.Repositories;

public sealed class OrderItemRepository : BaseRepository<OrderItem>, IOrderItemRepository, IScopedDependency
{
    public OrderItemRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<OrderItem>> GetOrderItemsAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<OrderItem>()
            .AsNoTracking()
            .Include(i => i.Product)
            .Where(i => i.OrderId == orderId)
            .ToListAsync(cancellationToken);
    }
}
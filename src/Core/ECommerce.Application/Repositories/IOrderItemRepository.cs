using ECommerce.Domain.Entities;
using ECommerce.SharedKernel;

namespace ECommerce.Application.Repositories;

public interface IOrderItemRepository : IRepository<OrderItem>
{
    Task<IEnumerable<OrderItem>> GetOrderItemsAsync(Guid orderId, CancellationToken cancellationToken = default);
}
using ECommerce.Domain.Entities;
using ECommerce.SharedKernel;

namespace ECommerce.Application.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetUserOrdersAsync(Guid userId, CancellationToken cancellationToken = default);
}
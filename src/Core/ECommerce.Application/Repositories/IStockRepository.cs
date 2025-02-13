
namespace ECommerce.Application.Repositories;

public interface IStockRepository
{
    Task ReserveStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default);
    Task ReleaseStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default);
}

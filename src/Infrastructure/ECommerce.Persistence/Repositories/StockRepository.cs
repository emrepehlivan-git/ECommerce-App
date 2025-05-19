using ECommerce.Application.Exceptions;
using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Contexts;

namespace ECommerce.Persistence.Repositories;

public sealed class StockRepository(ApplicationDbContext context) :
 BaseRepository<ProductStock>(context),
  IStockRepository
{
    public async Task ReserveStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        try
        {
            var stock = Query(x => x.ProductId == productId).FirstOrDefault()
            ?? throw new NotFoundException($"Stock not found for product {productId}");

            if (stock.Quantity < quantity)
                throw new BusinessException($"Insufficient stock for product {productId}");

            stock.Reserve(quantity);
            Update(stock);
            await Context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new BusinessException($"Error reserving stock for product {productId}", ex);
        }
    }

    public async Task ReleaseStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        try
        {
            var stock = Query(x => x.ProductId == productId).FirstOrDefault()
            ?? throw new NotFoundException($"Stock not found for product {productId}");

            stock.Release(quantity);
            Update(stock);
            await Context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new BusinessException($"Error releasing stock for product {productId}", ex);
        }
    }
}

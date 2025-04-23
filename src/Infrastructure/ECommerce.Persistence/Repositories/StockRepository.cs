using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Repositories;
using ECommerce.Persistence.Contexts;
using Microsoft.Extensions.Logging;

namespace ECommerce.Persistence.Repositories;

public sealed class StockRepository(ApplicationDbContext context, ILogger<StockRepository> logger) : IStockRepository
{
    public async Task ReserveStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        var stock = await context.ProductStocks.FindAsync([productId], cancellationToken);
        if (stock is null)
        {
            logger.LogError("Stock not found for product {ProductId}", productId);
            throw new NotFoundException($"Stock not found for product {productId}");
        }

        if (stock.Quantity < quantity)
        {
            logger.LogError("Insufficient stock for product {ProductId}. Available: {Available}, Requested: {Requested}",
                productId, stock.Quantity, quantity);
            throw new BusinessException($"Insufficient stock for product {productId}");
        }

        stock.Reserve(quantity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task ReleaseStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        var stock = await context.ProductStocks.FindAsync([productId], cancellationToken);
        if (stock is null)
        {
            logger.LogError("Stock not found for product {ProductId}", productId);
            throw new NotFoundException($"Stock not found for product {productId}");
        }

        stock.Release(quantity);
        await context.SaveChangesAsync(cancellationToken);
    }
}
